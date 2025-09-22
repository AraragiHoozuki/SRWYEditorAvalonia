using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SRWYEditor.Models;
using SRWYEditorAvalonia.Services;
using SRWYEditorAvalonia.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.Security;
using System.Security.AccessControl;
using System.Text.Json;
using System.Threading.Tasks;
using Ursa.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SRWYEditorAvalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IFileService fileService;
        private readonly IMasterDataService masterService;
        private readonly IDataEditorWindowViewModelFactory dataEditorWindowViewModelFactory;
        private readonly IConfigsService configsService;
        private readonly IPathHelperService pathHelperService;
        [ObservableProperty]
        private string? robotDataPath;
        [ObservableProperty]
        private string? pilotDataPath;
        [ObservableProperty]
        private string? statusAttachDataPath;
        public ObservableCollection<NodeViewModel> RootNodes { get; } = new();

        public MainWindowViewModel(IFileService fileService, IMasterDataService masterService, IDataEditorWindowViewModelFactory dataEditorWindowViewModelFactory, IConfigsService configsService, IPathHelperService pathHelperService)
        {
            this.fileService = fileService;
            this.masterService = masterService;
            this.dataEditorWindowViewModelFactory = dataEditorWindowViewModelFactory;
            this.pathHelperService = pathHelperService;
            this.configsService = configsService;
            this.configsService.Load();
            RobotDataPath = configsService.CurrentConfigs.RobotDataPath;
            PilotDataPath = configsService.CurrentConfigs.PilotDataPath;
            StatusAttachDataPath = configsService.CurrentConfigs.StatusAttachDataPath;
            try
            {
                if (!string.IsNullOrEmpty(RobotDataPath))
                {
                    string json = File.ReadAllText(RobotDataPath);
                    masterService.DeserializeRobot(json);
                }
                if (!string.IsNullOrEmpty(PilotDataPath))
                {
                    string json = File.ReadAllText(PilotDataPath);
                    masterService.DeserializePilot(json);
                }
                if (!string.IsNullOrEmpty(StatusAttachDataPath))
                {
                    string json = File.ReadAllText(StatusAttachDataPath);
                    masterService.DeserializeStatusAttach(json);
                }
            }
            catch (System.Exception)
            {
                // Ignore errors
            }

        }
        [RelayCommand]
        async Task Test()
        {
            var file = await App.Services!.GetRequiredService<IFileService>().OpenFileAsync();
            if (file is not null)
            {
                var json = File.ReadAllText(file.Path.AbsolutePath);
                var master = App.Services!.GetRequiredService<IMasterDataService>();
                master.DeserializePilot(json);
                RootNodes.Clear();
                var nodes = ObjectEditorViewModelFactory.BuildViewModelNodes(master.PilotBasicDatas);
                foreach (var node in nodes)
                {
                    RootNodes.Add(node);
                }
            }
        }
        [RelayCommand]
        private async Task SelectJson(int type)
        {
            var file = await fileService.OpenFileAsync();
            if (file is not null)
            {
                var json = File.ReadAllText(file.Path.AbsolutePath);
                switch ((DataBaseType)type)
                {
                    case DataBaseType.Robot:
                        RobotDataPath = file.Path.AbsolutePath;
                        configsService.CurrentConfigs.RobotDataPath = RobotDataPath;
                        masterService.DeserializeRobot(json);
                        break;
                    case DataBaseType.Pilot:
                        PilotDataPath = file.Path.AbsolutePath;
                        configsService.CurrentConfigs.PilotDataPath = PilotDataPath;
                        masterService.DeserializePilot(json);
                        break;
                    case DataBaseType.StatusAttach:
                        StatusAttachDataPath = file.Path.AbsolutePath;
                        configsService.CurrentConfigs.StatusAttachDataPath = StatusAttachDataPath;
                        masterService.DeserializeStatusAttach(json);
                        break;
                    default:
                        break;
                }
                configsService.Save();
            }
        }
        [RelayCommand]
        private void OpenEditorWindow(int type)
        {
            switch ((DataBaseType)type)
            {
                case DataBaseType.Robot:
                    var vm = dataEditorWindowViewModelFactory.Create(DataBaseType.Robot);
                    vm.LoadData();
                    var window = new DataEditorWindow
                    {
                        DataContext = vm
                    };
                    window.Show();
                    break;
                case DataBaseType.Pilot:
                    var vmPilot = dataEditorWindowViewModelFactory.Create(DataBaseType.Pilot);
                    vmPilot.LoadData();
                    var windowPilot = new DataEditorWindow
                    {
                        DataContext = vmPilot
                    };
                    windowPilot.Show();
                    break;
                case DataBaseType.StatusAttach:
                    var vmStatus = dataEditorWindowViewModelFactory.Create(DataBaseType.StatusAttach);
                    vmStatus.LoadData();
                    var windowStatus = new DataEditorWindow
                    {
                        DataContext = vmStatus
                    };
                    windowStatus.Show();
                    break;
                default:
                    break;
            }
        }
        [RelayCommand]
        private async Task Dump(int type)
        {

            switch ((DataBaseType)type)
            {
                case DataBaseType.Robot:
                    await masterService.SaveMonoBehaviorForUABEAAsync(masterService.RobotBasicDatas);
                    break;
                case DataBaseType.Pilot:
                    await masterService.SaveMonoBehaviorForUABEAAsync(masterService.PilotBasicDatas);
                    break;
                case DataBaseType.StatusAttach:
                    await masterService.SaveMonoBehaviorForUABEAAsync(masterService.StatusAttachDatas);
                    break;
                default:
                    break;
            }
            
        }
        [RelayCommand]
        private async Task SaveJson()
        {
            try
            {
                if (configsService.CurrentConfigs.RobotDataPath is not null)
                {
                    await File.WriteAllTextAsync(configsService.CurrentConfigs.RobotDataPath, JsonSerializer.Serialize(masterService.RobotBasicDatas));
                }
                if (configsService.CurrentConfigs.PilotDataPath is not null)
                {
                    await File.WriteAllTextAsync(configsService.CurrentConfigs.PilotDataPath, JsonSerializer.Serialize(masterService.PilotBasicDatas));
                }
                if (configsService.CurrentConfigs.StatusAttachDataPath is not null)
                {
                    await File.WriteAllTextAsync(configsService.CurrentConfigs.StatusAttachDataPath, JsonSerializer.Serialize(masterService.StatusAttachDatas));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save file. Error: {ex.Message}");
            }
        }
        [RelayCommand]
        private async Task RevertMaster()
        {
            using Stream stream = AssetLoader.Open(new Uri("avares://SRWYEditorAvalonia/Assets/OriginalJson/RobotBasicDatas.json"));
            using StreamReader reader = new StreamReader(stream);
            string? robots = await reader.ReadToEndAsync();
            masterService.DeserializeRobot(robots);

            using Stream stream_pilot = AssetLoader.Open(new Uri("avares://SRWYEditorAvalonia/Assets/OriginalJson/PilotBasicDatas.json"));
            using StreamReader reader_pilot = new StreamReader(stream_pilot);
            string? pilots = await reader_pilot.ReadToEndAsync();
            masterService.DeserializePilot(pilots);

            using Stream stream_status = AssetLoader.Open(new Uri("avares://SRWYEditorAvalonia/Assets/OriginalJson/StatusAttachDatas.json"));
            using StreamReader reader_status = new StreamReader(stream_status);
            string? status = await reader_status.ReadToEndAsync();
            masterService.DeserializeStatusAttach(status);

        }

        [RelayCommand]
        private async Task InitMaster()
        {
            try
            {
                using Stream stream = AssetLoader.Open(new Uri("avares://SRWYEditorAvalonia/Assets/OriginalJson/RobotBasicDatas.json"));
                using StreamReader reader = new StreamReader(stream);
                string? robots = await reader.ReadToEndAsync();
                string robotPath = pathHelperService.GetLocalFilePath("RobotBasicDatas.json");
                await File.WriteAllTextAsync(robotPath, robots);
                RobotDataPath = robotPath;
                configsService.CurrentConfigs.RobotDataPath = RobotDataPath;

                using Stream stream_pilot = AssetLoader.Open(new Uri("avares://SRWYEditorAvalonia/Assets/OriginalJson/PilotBasicDatas.json"));
                using StreamReader reader_pilot = new StreamReader(stream_pilot);
                string? pilots = await reader_pilot.ReadToEndAsync();
                string pilotPath = pathHelperService.GetLocalFilePath("PilotBasicDatas.json");
                await File.WriteAllTextAsync(pilotPath, pilots);
                PilotDataPath = pilotPath;
                configsService.CurrentConfigs.PilotDataPath = PilotDataPath;

                using Stream stream_status = AssetLoader.Open(new Uri("avares://SRWYEditorAvalonia/Assets/OriginalJson/StatusAttachDatas.json"));
                using StreamReader reader_status = new StreamReader(stream_status);
                string? status = await reader_status.ReadToEndAsync();
                string statusPath = pathHelperService.GetLocalFilePath("StatusAttachDatas.json");
                await File.WriteAllTextAsync(statusPath, status);
                StatusAttachDataPath = statusPath;
                configsService.CurrentConfigs.StatusAttachDataPath = StatusAttachDataPath;

                await RevertMaster();
                configsService.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save original master json files. Error: {ex.Message}");
            }
        }
        [RelayCommand]
        private void ShowBundleNamesWindow()
        {
            var window = App.Services!.GetRequiredService<BundleNamesWindow>();
            window.Show();
        }
    }
}
