using DAM.Core.Models.MapperModels;
using DAM.Core.Models.SolutionModels;
using DAM.Core.Services.Interfaces;
using DAM.NativeDependencies;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DAM.Core.Models.MapperModels.MapPairItems;
using MvvmCross;
using MvvmCross.Presenters.Hints;

namespace DAM.ViewModels
{
    public enum ModelType
    {
        Source,
        Destination,
    }

    public enum MapPairType
    {
        Simple,
        Ignore,
        Order
    }

    public class MainViewModel : MvxNavigationViewModel
    {
        private readonly IFileHelper _fileHelper;
        private readonly ISolutionProcessorService _dllProcessorService;
        private readonly IProjectHelper _projectHelper;
        private readonly IMapConfigBuilder _mapConfigBuilder;

        public MvxAsyncCommand LoadSolutionCommand { get; }
        public MvxCommand<Model> OpenSourceModelCommand { get; }
        public MvxCommand<Model> OpenDestinationModelCommand { get; }
        public MvxCommand AddMapConfigItemCommand { get; }
        public MvxCommand AddIgnoreMapPairItemCommand { get; }
        public MvxCommand AddOrderedMapPairItemCommand { get; }
        public MvxCommand<string> RemoveMapConfigItemCommand { get; }
        public MvxCommand<ModelType> RemoveMapModelCommand { get; }
        public MvxCommand SaveMapPairCommand { get; }
        public MvxCommand OpenMapPairCommand { get; }
        public MvxCommand CreateMapConfigCommand { get; }

        private Solution _solution;

        public Solution CurrentSolution
        {
            get => _solution;
            set => SetProperty(ref _solution, value);
        }

        private Item _selectedModel;
        public Item SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        private MapPair _selectedMapPair;
        public MapPair SelectedMapPair
        {
            get => _selectedMapPair;
            set => SetProperty(ref _selectedMapPair, value);
        }

        private Model _sourceModel;
        public Model SourceModel
        {
            get => _sourceModel;
            set => SetProperty(ref _sourceModel, value);
        }

        private Model _destinationModel;
        public Model DestinationModel
        {
            get => _destinationModel;
            set => SetProperty(ref _destinationModel, value);
        }

        private bool _isSavedPairsVisible;
        public bool IsSavedPairsVisible
        {
            get => _isSavedPairsVisible;
            set => SetProperty(ref _isSavedPairsVisible, value);
        }

        public MvxObservableCollection<Item> SolutionItems { get; } = new MvxObservableCollection<Item>();

        public MvxObservableCollection<Property> SourceProperties { get; } = new MvxObservableCollection<Property>();
        public MvxObservableCollection<Property> DestinationProperties { get; } = new MvxObservableCollection<Property>();

        public MvxObservableCollection<MapPair> MapPairs { get; } = new MvxObservableCollection<MapPair>();
        public MvxObservableCollection<MapPairItemBase> MapItems { get; } = new MvxObservableCollection<MapPairItemBase>();

        public MainViewModel(ILoggerFactory logFactory,
            IMvxNavigationService navigationService,
            IFileHelper solutionLoaderHelper,
            ISolutionProcessorService dllProcessorService,
            IProjectHelper projectHelper,
            IMapConfigBuilder mapConfigBuilder)
            : base(logFactory, navigationService)
        {
            _fileHelper = solutionLoaderHelper;
            _dllProcessorService = dllProcessorService;
            _projectHelper = projectHelper;
            _mapConfigBuilder = mapConfigBuilder;

            IsSavedPairsVisible = true;

            LoadSolutionCommand = new MvxAsyncCommand(LoadSolutionCommandExecute);
            OpenSourceModelCommand = new MvxCommand<Model>(OpenSourceModelCommandExecute);
            OpenDestinationModelCommand = new MvxCommand<Model>(OpenDestinationModelCommandExecute);
            AddMapConfigItemCommand = new MvxCommand(AddMapConfigItemCommandExecute);
            AddIgnoreMapPairItemCommand = new MvxCommand(AddIgnoreMapPairItemCommandExecute);
            AddOrderedMapPairItemCommand = new MvxCommand(AddOrderedMapPairItemCommandExecute);
            RemoveMapConfigItemCommand = new MvxCommand<string>(RemoveMapConfigItemCommandExecute);
            RemoveMapModelCommand = new MvxCommand<ModelType>(RemoveMapModelCommandExecute);
            SaveMapPairCommand = new MvxCommand(SaveMapPairCommandExecute);
            OpenMapPairCommand = new MvxCommand(OpenMapPairCommandExecute);
            CreateMapConfigCommand = new MvxCommand(CreateMapConfigCommandExecute);
        }

        #region Commands

        private async Task LoadSolutionCommandExecute()
        {
            string dllPath = _fileHelper.GetSolutionPath();
            if (!string.IsNullOrEmpty(dllPath))
            {
                CurrentSolution = await _dllProcessorService.GetSolutionProjects(dllPath);
                if (CurrentSolution.Items.Any())
                    SolutionItems.ReplaceWith(CurrentSolution.Items);
            }
        }

        private void OpenSourceModelCommandExecute(Model model) =>
            OpenModelBase(model, ModelType.Source);

        private void OpenDestinationModelCommandExecute(Model model) =>
            OpenModelBase(model, ModelType.Destination);

        private void AddMapConfigItemCommandExecute()
        {
            if (IsReadyToAddMapPairItem())
                MapItems.Add(new SimpleMapPairItem());
        }

        private void AddIgnoreMapPairItemCommandExecute()
        {
            if (IsReadyToAddMapPairItem())
                MapItems.Add(new IgnoreMapPairItem());
        }

        private void AddOrderedMapPairItemCommandExecute()
        {
            if (IsReadyToAddMapPairItem())
                MapItems.Add(new OrderedMapPairItem());
        }

        private void RemoveMapConfigItemCommandExecute(string id)
        {
            MapItems.Remove(MapItems.FirstOrDefault(item => item.Id == id));
        }

        private void RemoveMapModelCommandExecute(ModelType modelType)
        {
            switch (modelType)
            {
                case ModelType.Source:
                    SourceModel = default;
                    SourceProperties.Clear();
                    break;
                case ModelType.Destination:
                    DestinationModel = default;
                    DestinationProperties.Clear();
                    break;
            }
            MapItems.Clear();
        }

        private void SaveMapPairCommandExecute()
        {
            if (SourceModel != null && DestinationModel != null)
            {
                SaveCurrentMapPair();
                ResetCurrentMapPair();
            }
        }

        private void OpenMapPairCommandExecute()
        {
            if (SelectedMapPair != null)
            {
                OpenModelBase(SelectedMapPair.SourceModel, ModelType.Source);
                OpenModelBase(SelectedMapPair.DestinationModel, ModelType.Destination);
                MapItems.ReplaceWith(SelectedMapPair.MapItems);
                MapPairs.Remove(MapPairs.FirstOrDefault(pair => pair.Id == SelectedMapPair.Id));
            }
        }

        private void CreateMapConfigCommandExecute()
        {
            if (!MapPairs.Any() && SourceModel != null && DestinationModel != null)
                SaveCurrentMapPair();

            if (MapPairs.Any())
                NavigationService
                    .Navigate<MapConfigViewModel, string>(
                        _mapConfigBuilder
                        .AddMapPairs(MapPairs.ToList())
                        .BuildMapConfig());
        }

        #endregion

        #region Helpers

        private bool IsReadyToAddMapPairItem()
            => SourceModel != null && DestinationModel != null && SourceProperties.Any() && DestinationProperties.Any();

        private void OpenModelBase(Model model, ModelType modelType)
        {
            if (model != null)
            {
                var properties = _projectHelper.GetModelProperties(model);
                if (properties != null)
                {
                    switch (modelType)
                    {
                        case ModelType.Source:
                            if (SourceModel != default)
                                MapItems.Clear();
                            SourceModel = model;
                            SourceProperties.ReplaceWith(properties);
                            break;
                        case ModelType.Destination:
                            if (DestinationModel != default)
                                MapItems.Clear();
                            DestinationModel = model;
                            DestinationProperties.ReplaceWith(properties);
                            break;
                    }
                }
            }
        }

        private void SaveCurrentMapPair()
        {
            MapPairs.Add(new MapPair() { SourceModel = SourceModel, DestinationModel = DestinationModel, MapItems = MapItems.ToList() });
        }

        private void ResetCurrentMapPair()
        {
            SourceModel = default;
            DestinationModel = default;
            SourceProperties.Clear();
            DestinationProperties.Clear();
            MapItems.Clear();
        }

        #endregion
    }
}
