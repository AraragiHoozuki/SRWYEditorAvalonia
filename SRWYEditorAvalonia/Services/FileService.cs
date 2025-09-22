using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Services
{
    public interface IFileService
    {
        Task<IStorageFile?> OpenFileAsync();
        Task<IStorageFile?> PickSaveFileAsync();
    }
    public class FileService : IFileService
    {
        private readonly TopLevel _topLevel;

        public FileService(TopLevel topLevel)
        {
            _topLevel = topLevel;
        }

        public async Task<IStorageFile?> OpenFileAsync()
        {
            var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "选择一个文件...",
                AllowMultiple = false,
            });

            return files?.FirstOrDefault();
        }
        public async Task<IStorageFile?> PickSaveFileAsync()
        {
            var file = await _topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "保存文件...",
                DefaultExtension = "txt",
                ShowOverwritePrompt = true,
            });

            return file;
        }
    }
}
