using DAM.NativeDependencies;
using DAM.UI.NativeDependencies;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace DAM.UI
{
    public class Setup : MvxWpfSetup<ViewModels.Setup>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void RegisterDefaultSetupDependencies(IMvxIoCProvider iocProvider)
        {
            base.RegisterDefaultSetupDependencies(iocProvider);

            iocProvider.RegisterType<IFileHelper, FileHelper>();
            iocProvider.RegisterType<IWindowsHelper, WindowsHelper>();
        }
    }
}
