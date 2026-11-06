using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DesktopTools.ViewModels;
using DesktopTools.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopTools;

public partial class App : Application
{
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var serviceCollection = new ServiceCollection();
            
            Utils.Services.RegisterDependencies(serviceCollection);

            serviceCollection.AddTransient<MainWindowViewModel>();

            Services = serviceCollection.BuildServiceProvider();

            var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}