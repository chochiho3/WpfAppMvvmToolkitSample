using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppListBindingTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //services
            services.AddSingleton<ClientTcp>();

            //view models
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<SubWindowViewModel>();
            services.AddSingleton<SubWindow2ViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
