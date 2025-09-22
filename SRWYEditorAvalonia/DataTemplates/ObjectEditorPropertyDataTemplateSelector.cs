using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using SRWYEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.DataTemplates
{
    public class ObjectEditorPropertyDataTemplateSelector : IDataTemplate
    {
        [Content]
        public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();

        // Build the DataTemplate here
        public Control Build(object? param)
        {
            string key;
            param = (param as PropertyNodeViewModel)?.Value ?? null;
            if (param is string)
            {
                key = "String";
            } else if (param is int)
            {
                key = "Int";
            } else if (param is byte)
            {
                key = "Byte";
            } else {
                key = "Other";
            }
            return AvailableTemplates[key].Build(param); // finally we look up the provided key and let the System build the DataTemplate for us
        }

        // Check if we can accept the provided data
        public bool Match(object? data)
        {
            // Our Keys in the dictionary are strings, so we call .ToString() to get the key to look up
            return true;
        }
    }
}
