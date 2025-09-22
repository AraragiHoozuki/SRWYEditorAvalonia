using SRWYEditor.Models;
using SRWYEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.Services
{
    public interface IDataEditorWindowViewModelFactory
    {
        DataEditorWindowViewModel Create(DataBaseType dataBaseType);
    }

    public class DataEditorWindowViewModelFactory : IDataEditorWindowViewModelFactory
    {
        private readonly IFileService fileService;
        private readonly IMasterDataService masterService;

        public DataEditorWindowViewModelFactory(IFileService fileService, IMasterDataService masterService)
        {
            this.fileService = fileService;
            this.masterService = masterService;
        }

        public DataEditorWindowViewModel Create(DataBaseType dataBaseType)
        {
            return new DataEditorWindowViewModel(dataBaseType, fileService, masterService);
        }
    }
}
