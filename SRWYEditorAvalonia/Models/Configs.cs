using Microsoft.Extensions.DependencyInjection;
using SRWYEditor.Models;
using SRWYEditorAvalonia.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Models
{
    public class Configs
    {
        public string? RobotDataPath { get; set; }
        public string? PilotDataPath { get; set; }

        public string ? StatusAttachDataPath { get; set; }
    }

    [JsonSerializable(typeof(Configs))]
    public partial class ConfigsJsonContext : JsonSerializerContext;
}
