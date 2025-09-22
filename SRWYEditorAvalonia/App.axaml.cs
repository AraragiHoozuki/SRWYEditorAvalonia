using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SRWYEditorAvalonia.Services;
using SRWYEditorAvalonia.ViewModels;
using SRWYEditorAvalonia.Views;
using System.Linq;

namespace SRWYEditorAvalonia
{
    public partial class App : Application
    {
        public static ServiceProvider? Services { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var services = new ServiceCollection();
                services.AddSingleton<IPathHelperService, PathHelperService>();
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IMasterDataService, MasterDataService>();
                services.AddSingleton<IDataEditorWindowViewModelFactory, DataEditorWindowViewModelFactory>();
                services.AddSingleton<IConfigsService, ConfigsService>();



                services.AddTransient<MainWindowViewModel>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<BundleNamesWindow>();
                
                services.AddSingleton(provider => (TopLevel)provider.GetRequiredService<MainWindow>());
                Services = services.BuildServiceProvider();

                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}