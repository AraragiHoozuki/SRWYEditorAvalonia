using Microsoft.Extensions.DependencyInjection;
using SRWYEditor.Models;
using SRWYEditorAvalonia.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRWYEditorAvalonia.ViewModels
{
    public partial class DataEditorWindowViewModel : ViewModelBase
    {
        private readonly DataBaseType dataBaseType;
        private readonly IFileService fileService;
        private readonly IMasterDataService masterService;
        public ObservableCollection<NodeViewModel> RootNodes { get; } = new();
        public DataEditorWindowViewModel(DataBaseType dataBaseType, IFileService fileService, IMasterDataService masterService)
        {
            this.dataBaseType = dataBaseType;
            this.fileService = fileService;
            this.masterService = masterService;
            LoadData();
        }

        public void LoadData()
        {
            object data = dataBaseType switch
            {
                DataBaseType.Pilot => masterService.PilotBasicDatas,
                DataBaseType.Robot => masterService.RobotBasicDatas,
                DataBaseType.StatusAttach => masterService.StatusAttachDatas,
                _ => throw new NotImplementedException(),
            };
            RootNodes.Clear();
            var nodes = ObjectEditorViewModelFactory.BuildViewModelNodes(data);
            foreach (var node in nodes)
            {
                RootNodes.Add(node);
            }
        }
    }
}
