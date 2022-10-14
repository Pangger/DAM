using DAM.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace DAM.UI.Views
{
    [MvxContentPresentation(WindowIdentifier = nameof(MainWindow), StackNavigation = false)]
    [MvxViewFor(typeof(MainViewModel))]
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}
