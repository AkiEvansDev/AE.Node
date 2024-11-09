using AE.Dal;
using AE.Node;
using AE.Node.Interface;
using AE.Node.Items;
using AE.Note.Example.Services;
using AE.Note.Example.ViewModels.Pages;
using AE.Note.Example.ViewModels.Windows;
using AE.Note.Example.Views.Pages;
using AE.Note.Example.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;

namespace AE.Note.Example
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? "") ?? ""); })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
            }).Build();

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            WorkflowManager.Init(new WorkflowSettings());

            WorkflowManager.AddNodeType(new TestNode());

            _host.Start();
        }

        public class TestNode : INodeType
        {
            public string DefaultTitle => "Test All Pins";
            public string Name => "TEST";
            public string Group => "Test";
            public string DefaultColor => new ColorPath(ColorKey.PaleViolet, FactorType.Shade, 2).ToColor().Name;

            public INodeBuilder Builder { get; }

            public PinItem[] In { get; } =
            [
                new PinItem { Type = "*", Title = "Pin Any" },
                new PinItem { Type = "THREAD", Title = "Pin Thread"  },
                new PinItem { Type = "BOOLEAN", Title = "Pin Bool" },
                new PinItem { Type = "BYTE", Title = "Pin Byte" },
                new PinItem { Type = "INT", Title = "Pin Int" },
                new PinItem { Type = "FLOAT", Title = "Pin Float" },
                new PinItem { Type = "DOUBLE", Title = "Pin Double" },
                new PinItem { Type = "STRING", Title = "Pin String" },
                new PinItem { Type = "POINT", Title = "Pin Point" },
                new PinItem { Type = "RANGE", Title = "Pin Range" },
                new PinItem { Type = "COLOR", Title = "Pin Color" },
                new PinItem { Type = "KEY", Title = "Pin Key" },
                new PinItem { Type = "KEY_MODIFIER", Title = "Pin Key Mod" },
                new PinItem { Type = "MOUSE_BUTTON", Title = "Pin Mouse Btn" },
                new PinItem { Type = "MOUSE_EVENT", Title = "Pin Mouse Evt" }
            ];

            public PinItem[] Out { get; } =
            [
                new PinItem { Type = "*", Title = "Pin Any" },
                new PinItem { Type = "INT", Title = "Pin Int" },
            ];

            public PinValue[] Values { get; } =
            [
                new PinValue { Type = "BOOLEAN", Title = "Pin Bool" },
                new PinValue { Type = "BYTE", Title = "Pin Byte" },
                new PinValue { Type = "INT", Title = "Value Int" },
                new PinValue { Type = "DOUBLE", Title = "Value Double" },
                new PinValue { Type = "STRING", Title = "Value String" },
                new PinValue { Type = "POINT", Title = "Value Point" },
                new PinValue { Type = "RANGE", Title = "Value Range" },
                new PinValue { Type = "COLOR", Title = "Value Color" },
                new PinValue { Type = "KEY", Title = "Value Key" },
                new PinValue { Type = "KEY_MODIFIER", Title = "Value Key Mod" },
                new PinValue { Type = "MOUSE_BUTTON", Title = "Value Mouse Btn" },
                new PinValue { Type = "MOUSE_EVENT", Title = "Value Mouse Evt" }
            ];
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
