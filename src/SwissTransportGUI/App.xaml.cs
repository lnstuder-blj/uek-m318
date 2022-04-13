using System;
using System.Windows;
using Prism.Unity;
using Prism.Ioc;
using SwissTransport.Core;
using SwissTransportGUI.Services;
using SwissTransportGUI.Services.Interfaces;

namespace SwissTransportGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ITransport, Transport>();
            containerRegistry.Register<IStationAutoComplete, StationAutoComplete>();
        }
    }
}
