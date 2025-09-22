using Microsoft.Extensions.DependencyInjection;
using SRWYEditorAvalonia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Services
{
    public interface IConfigsService
    {
        Configs CurrentConfigs { get; }
        void Load();
        void Save();
    }
    public class ConfigsService(IPathHelperService pathHelperService) : IConfigsService
    {
        private readonly IPathHelperService pathHelperService = pathHelperService;
        public Configs CurrentConfigs { get; private set; } = new Configs();
        public void Load()
        {
            try
            {
                var path = pathHelperService.GetLocalFilePath("config.json");
                if (System.IO.File.Exists(path))
                {
                    var json = System.IO.File.ReadAllText(path);
                    var configs =  JsonSerializer.Deserialize(json, ConfigsJsonContext.Default.Configs);
                    if (configs is not null)
                    {
                        CurrentConfigs = configs;
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors and return default configs
            }
        }

        public void Save()
        {
            try
            {
                var path = pathHelperService.GetLocalFilePath("config.json");
                var json = JsonSerializer.Serialize(CurrentConfigs, ConfigsJsonContext.Default.Configs);
                System.IO.File.WriteAllText(path, json);
            }
            catch (Exception)
            {
                // Ignore errors
            }
        }
    }
}
