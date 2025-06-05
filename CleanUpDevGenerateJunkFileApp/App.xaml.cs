using CleanUpDevGenerateJunkFileApp.Views;
using Microsoft.Win32;
using Prism.Ioc;
using System.Windows;
using System.Windows.Threading;

namespace CleanUpDevGenerateJunkFileApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<OpenFolderDialog>();
            containerRegistry.RegisterSingleton<Dispatcher>(_=> Current.Dispatcher);
        }
    }
}
