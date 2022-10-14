using DAM.Core.Services;
using DAM.Core.Services.Interfaces;
using MvvmCross;
using MvvmCross.ViewModels;

namespace DAM.ViewModels
{
    public class Setup : MvxApplication
    {
        public override void Initialize()
        {
            RegisterTypes();

            RegisterAppStart<MainViewModel>();
        }

        private void RegisterTypes()
        {
            Mvx.IoCProvider.RegisterType<ISolutionProcessorService, SolutionProcessorService>();
            Mvx.IoCProvider.RegisterType<IProjectHelper, ProjectHelper>();
            Mvx.IoCProvider.RegisterType<IMapConfigBuilder, MapConfigBuilder>();
        }
    }
}
