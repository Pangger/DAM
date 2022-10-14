using DAM.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace DAM.UI.Views
{
    [MvxContentPresentation(WindowIdentifier = nameof(MainWindow), StackNavigation = false)]
    [MvxViewFor(typeof(MapConfigViewModel))]
    public partial class MapConfigView
    {
        public MapConfigView()
        {
            InitializeComponent();
        }
    }
}
