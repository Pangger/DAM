using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Windows;
using DAM.NativeDependencies;

namespace DAM.ViewModels
{
    public class MapConfigViewModel : MvxNavigationViewModel<string>
    {
        private readonly IWindowsHelper _windowsHelper;

        public MvxCommand BackCommand { get; }
        public MvxCommand SaveToClipboardCommand { get; }

        private string _mapConfig;
        public string MapConfig
        {
            get => _mapConfig;
            set => SetProperty(ref _mapConfig, value);
        }

        public MapConfigViewModel(
            ILoggerFactory logFactory, 
            IMvxNavigationService navigationService,
            IWindowsHelper windowsHelper) 
            : base(logFactory, navigationService)
        {
            _windowsHelper = windowsHelper;

            BackCommand = new MvxCommand(BackCommandExecute);
            SaveToClipboardCommand = new MvxCommand(SaveToClipboardCommandExecute);
        }

        public override void Prepare(string parameter)
        {
            MapConfig = parameter;
        }

        private void BackCommandExecute()
        {
            NavigationService.Navigate<MainViewModel>();
        }

        private void SaveToClipboardCommandExecute()
        {
            _windowsHelper.SaveToClipboard(MapConfig);
        }
    }
}
