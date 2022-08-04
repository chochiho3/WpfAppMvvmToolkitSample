using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using WpfAppListBindingTest.HostServices;

namespace WpfAppListBindingTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// https://docs.microsoft.com/ko-kr/dotnet/communitytoolkit/mvvm/
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;


        public new static App Current => (App)Application.Current;
        public IServiceProvider Services => _host.Services;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();
            _host.StartAsync();


            var isKor = true;
            if (isKor)
            {
                Thread.CurrentThread.CurrentUICulture = new("kr");
            }
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            //services
            services.AddHostedService<TestHostService>();
            services.AddSingleton<ClientTcp>();

            //views
            //view models
            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();

            services.AddTransient<SubWindow>();
            services.AddTransient<SubWindowViewModel>();

            services.AddTransient<SubWindow2>();
            services.AddSingleton<SubWindow2ViewModel>();


        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
