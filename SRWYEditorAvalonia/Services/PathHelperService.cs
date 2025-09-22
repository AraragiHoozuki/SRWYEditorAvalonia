using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Services
{
    public interface IPathHelperService
    {
        string GetLocalFilePath(string fileName);
    }
    public class PathHelperService : IPathHelperService
    {
        private string localFolder = string.Empty;
        private string LocalFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(localFolder))
                {
                    return localFolder;
                }
                localFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(SRWYEditorAvalonia));
                if (!Directory.Exists(localFolder))
                {
                    Directory.CreateDirectory(localFolder);
                }
                return localFolder;
            }
        }

        public string GetLocalFilePath(string fileName)
        {
            return Path.Combine(LocalFolder, fileName);
        }
    }
}
