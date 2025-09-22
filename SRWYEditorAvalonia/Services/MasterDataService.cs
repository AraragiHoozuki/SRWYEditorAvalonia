using Avalonia;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using SRWYEditor.Models;
using SRWYEditorAvalonia.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Services
{
    public interface IMasterDataService
    {
        void DeserializeRobot(string json);
        void DeserializePilot(string json);
        void DeserializeStatusAttach(string json);
        Task SaveMonoBehaviorForUABEAAsync(object data);
        RobotBasicDatas RobotBasicDatas { get; }
        PilotBasicDatas PilotBasicDatas { get; }
        StatusAttachDatas StatusAttachDatas { get; }
    }
    public class MasterDataService(IFileService fileService) : IMasterDataService
    {
        private const string RobotDataAssetUri = "avares://SRWYEditorAvalonia/Assets/OriginalJson/RobotBasicDatas.json";
        private const string PilotDataAssetUri = "avares://SRWYEditorAvalonia/Assets/OriginalJson/PilotBasicDatas.json";
        private const string StatusAttachDataAssetUri = "avares://SRWYEditorAvalonia/Assets/OriginalJson/StatusAttachDatas.json";
        private readonly IFileService fileService = fileService;
        public RobotBasicDatas RobotBasicDatas { get; private set; }
        public PilotBasicDatas PilotBasicDatas { get; private set; }
        public StatusAttachDatas StatusAttachDatas { get; private set; }

        public void DeserializeRobot(string json)
        {
            RobotBasicDatas = JsonSerializer.Deserialize<RobotBasicDatas>(json, RobotBasicDatasJsonContext.Default.RobotBasicDatas)!;
        }
        public void DeserializePilot(string json)
        {
            PilotBasicDatas = JsonSerializer.Deserialize<PilotBasicDatas>(json, PilotBasicDatasJsonContext.Default.PilotBasicDatas)!;
        }
        public void DeserializeStatusAttach(string json)
        {
            StatusAttachDatas = JsonSerializer.Deserialize<StatusAttachDatas>(json, StatusAttachDatasJsonContext.Default.StatusAttachDatas)!;
        }

        private async Task<string> ReadAssetJsonAsync(string uriString)
        {
            using var stream = AssetLoader.Open(new Uri(uriString));
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }
        public async Task LoadOriginalDataAsync()
        {
            try
            {
                Task<string> robotJsonTask = ReadAssetJsonAsync(RobotDataAssetUri);
                Task<string> pilotJsonTask = ReadAssetJsonAsync(PilotDataAssetUri);
                Task<string> statusAttachJsonTask = ReadAssetJsonAsync(StatusAttachDataAssetUri);

                await Task.WhenAll(robotJsonTask, pilotJsonTask, statusAttachJsonTask);

                var robotJson = await robotJsonTask;
                var pilotJson = await pilotJsonTask;
                var statusAttachJson = await statusAttachJsonTask;

                DeserializeRobot(robotJson);
                DeserializePilot(pilotJson);
                DeserializeStatusAttach(statusAttachJson);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred while loading original data: {ex.Message}");

            }

        }

        public static string GetPropertyTypeUABEAName(PropertyInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(UABEAPropertyTypeNameAttribute), false);
            if (attrs.Length > 0)
            {
                UABEAPropertyTypeNameAttribute attr = (UABEAPropertyTypeNameAttribute)attrs[0];
                return attr.Name;
            } 
            return GetFriendlyTypeName(p.PropertyType);
        }
        public static string GetFriendlyTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }
            string genericTypeName = type.Name.Split('`')[0];
            var genericArgNames = type.GetGenericArguments().Select(GetFriendlyTypeName);
            return $"{genericTypeName}<{string.Join(", ", genericArgNames)}>";
        }
        public async Task SaveMonoBehaviorForUABEAAsync(object data)
        {
            var file = await fileService.PickSaveFileAsync();
            if (file is not null)
            {
                try
                {
                    await using var stream = await file.OpenWriteAsync();
                    using var streamWriter = new StreamWriter(stream);
                    int indent = 0;
                    await streamWriter.WriteLineAsync($"0 MonoBehaviour Base");
                    indent++;
                    var props = data.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    foreach (var prop in props)
                    {
                        await WritePropertyAsync(streamWriter, indent, data, prop);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save file. Error: {ex.Message}");
                    // await ShowErrorDialogAsync("Error Saving File", ex.Message);
                }
            }
        }

        async Task WritePropertyAsync(StreamWriter writer, int indent, object instance, PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                string s = property.GetValue(instance) as string ?? string.Empty;
                s = s.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
                await writer.WriteLineAsync($"{new string(' ', indent)}1 string {property.Name} = \"{s}\"");
            }
            else if (property.PropertyType == typeof(int))
            {
                await writer.WriteLineAsync($"{new string(' ', indent)}0 int {property.Name} = {property.GetValue(instance)}");
            }
            else if (property.PropertyType == typeof(Int64))
            {
                await writer.WriteLineAsync($"{new string(' ', indent)}0 SInt64 {property.Name} = {property.GetValue(instance)}");
            }
            else if (property.PropertyType == typeof(byte))
            {
                await writer.WriteLineAsync($"{new string(' ', indent)}1 UInt8 {property.Name} = {property.GetValue(instance)}");
            }
            else if (property.PropertyType.IsEnum)
            {
                await writer.WriteLineAsync($"{new string(' ', indent)}0 int {property.Name} = {(int)property.GetValue(instance)!}");
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type elementType = property.PropertyType.GetGenericArguments()[0];
                int count = (int)(property.PropertyType.GetProperty("Count")?.GetValue(property.GetValue(instance), null) ?? 0);
                await writer.WriteLineAsync($"{new string(' ', indent)}0 {elementType.Name} {property.Name}");
                indent++;
                await writer.WriteLineAsync($"{new string(' ', indent)}0 Array Array ({count} items)");
                indent++;
                await writer.WriteLineAsync($"{new string(' ', indent)}0 int size = {count}");

                object? list = property.GetValue(instance);
                if (list is not null)
                {
                    if (list is IList ilist)
                    {
                        int index = 0;
                        foreach (var item in ilist)
                        {
                            await writer.WriteLineAsync($"{new string(' ', indent)}[{index}]");
                            index++;
                            indent++;
                            await writer.WriteLineAsync($"{new string(' ', indent)}0 {elementType.Name} data");
                            indent++;
                            var itemProperties = elementType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            foreach (var itemProperty in itemProperties)
                            {
                                await WritePropertyAsync(writer, indent, item, itemProperty);
                            }
                            indent -= 2;
                        }
                    }
                }
                indent -= 2;
            }
            else if (property.PropertyType.IsArray)
            {
                Array? arrayValue = property.GetValue(instance) as Array;
                int count = arrayValue?.Length ?? 0;
                await writer.WriteLineAsync($"{new string(' ', indent)}0 vector {property.Name}");
                indent++;
                await writer.WriteLineAsync($"{new string(' ', indent)}1 Array Array ({count} items)");
                indent++;
                await writer.WriteLineAsync($"{new string(' ', indent)}0 int size = {count}");
                if (arrayValue is not null)
                {
                    int index = 0;
                    foreach (var element in arrayValue)
                    {
                        await writer.WriteLineAsync($"{new string(' ', indent)}[{index}]");
                        index++;
                        indent++;
                        object value = element;
                        if (element.GetType() == typeof(string))
                        {
                            value = (element as string)?.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"") ?? string.Empty;
                            await writer.WriteLineAsync($"{new string(' ', indent)}1 string data = \"{value}\"");
                        } else if (element.GetType() == typeof(int) || element.GetType().IsEnum) {
                            await writer.WriteLineAsync($"{new string(' ', indent)}0 int data = {(int)value}");
                        } else if (element.GetType().IsPrimitive && !element.GetType().IsClass)
                        {
                            await writer.WriteLineAsync($"{new string(' ', indent)}0 {property.PropertyType.GetElementType()} data = {value}");
                        } else
                        {
                            var elementProperties = element.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                            await writer.WriteLineAsync($"{new string(' ', indent)}0 {element.GetType().Name} data");
                            indent++;
                            foreach (var elementProperty in elementProperties)
                            {
                                await WritePropertyAsync(writer, indent, element, elementProperty);
                            }
                            indent--;
                        }
                        indent--;
                    }
                }
                indent -= 2;
            }
            else
            {
                var propertyValue = property.GetValue(instance);
                if (propertyValue is not null)
                {
                    await writer.WriteLineAsync($"{new string(' ', indent)}0 {GetPropertyTypeUABEAName(property)} {property.Name}");
                    indent++;
                    var subProperties = property.PropertyType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    foreach (var subProperty in subProperties)
                    {
                        await WritePropertyAsync(writer, indent, propertyValue, subProperty);
                    }
                    indent--;
                }
            }
        }
        
    }



    [JsonSerializable(typeof(RobotBasicDatas))]
    public partial class RobotBasicDatasJsonContext : JsonSerializerContext;

    [JsonSerializable(typeof(PilotBasicDatas))]
    public partial class PilotBasicDatasJsonContext : JsonSerializerContext;
    [JsonSerializable(typeof(StatusAttachDatas))]
    public partial class StatusAttachDatasJsonContext : JsonSerializerContext;
}
