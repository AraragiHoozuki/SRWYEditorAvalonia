using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using System.Xml.Linq;

namespace SRWYEditorAvalonia.ViewModels
{
    public partial class DataEditorWindowViewModel : ViewModelBase
    {
        private readonly DataBaseType dataBaseType;
        private readonly IFileService fileService;
        private readonly IMasterDataService masterService;
        public ObservableCollection<NodeViewModel> RootNodes { get; } = new();
        public ObservableCollection<NodeViewModel> NodePath { get; } = new();
        private NodeViewModel? selectedNode;
        public NodeViewModel? SelectedNode
        {
            get => selectedNode;
            set
            {
                if (selectedNode != value)
                {
                    selectedNode = value;
                    NodePath.Clear();
                    while (value != null)
                    {
                        if (!NodePath.Contains(value))
                        {
                            NodePath.Insert(0, value);
                        }
                        value = value.Container;
                    }
                    OnPropertyChanged();
                }
            }
        }
        [ObservableProperty]
        private string searchText = string.Empty;
        private string lastSearchText = string.Empty;
        private IEnumerator<NodeViewModel>? searchEnumerator;
        private readonly Func<NodeViewModel, string, bool> matchPredicate = (node, text) => node.DisplayName.Contains(text, StringComparison.OrdinalIgnoreCase);
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
        [RelayCommand]
        private void SelectNode(NodeViewModel node)
        {
            var current = node.Container;
            while (current != null)
            {
                current.IsExpanded = true;
                current = current.Container;
            }
            SelectedNode = node;
        }
        [RelayCommand]
        private void FindNext()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return;
            }
            if (!string.Equals(SearchText, lastSearchText, StringComparison.Ordinal) || searchEnumerator == null)
            {
                lastSearchText = SearchText;
                searchEnumerator?.Dispose();
                searchEnumerator = SearchDFS(null, SearchText).GetEnumerator();
            }
            if (searchEnumerator.MoveNext())
            {
                SelectNode(searchEnumerator.Current);
            }
            else
            {
                searchEnumerator.Dispose();

                IEnumerable<NodeViewModel> searchSequence = SearchDFS(null, SearchText);

                searchEnumerator = searchSequence.GetEnumerator();

                if (searchEnumerator.MoveNext())
                {
                    SelectNode(searchEnumerator.Current);
                }
            }

        }

        private IEnumerable<NodeViewModel> SearchDFS(NodeViewModel? node, string searchText)
        {
            if (node is null)
            {
                node = new DummyNodeViewModel();
                node.DisplayName = "Root";
                foreach (var root in RootNodes)
                {
                    node.Children.Add(root);
                }
            }
            if (node.IsChildrenLoaded == false)
            {
                node.ForceLoadChildren();
            }
            if (matchPredicate(node, searchText))
            {
                yield return node;
            }
            foreach (var child in node.Children)
            {
                foreach (var descendant in SearchDFS(child, searchText))
                {
                    yield return descendant;
                }
            }
        }
    }
}
