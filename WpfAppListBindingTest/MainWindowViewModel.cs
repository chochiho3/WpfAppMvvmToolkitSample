using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppListBindingTest
{
    public partial class MainWindowViewModel : ObservableValidator
    {
        readonly ClientTcp _tcpClient;

        [ObservableProperty]
        ObservableCollection<LogObj> _logObjs;

        [ObservableProperty]
        [Required]
        [NotifyPropertyChangedFor(nameof(FullAddress))]
        [NotifyCanExecuteChangedFor(nameof(ConnectToServerCommand))]
        [CustomValidation(typeof(ValidatorBundle), nameof(ValidatorBundle.CustomValidation))]
        string _ipAddress;

        [ObservableProperty]
        [Required]
        [NotifyPropertyChangedFor(nameof(FullAddress))]
        [NotifyCanExecuteChangedFor(nameof(ConnectToServerCommand))]
        int? _port;

        public string FullAddress => $"{IpAddress}:{Port}";

        [ObservableProperty]
        FrameworkElement _subContent;

        [ObservableProperty]
        string eventString;

        public MainWindowViewModel(ClientTcp tcpClient)
        {
            _tcpClient = tcpClient;

            _logObjs = new();
            _tcpClient.OnRecvMsg += (sender, e) =>
                App.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        LogObjs.Insert(
                            0,
                            new LogObj()
                            {
                                Writer = $"Sample{LogObjs.Count}",
                                Message = e,
                                RecordTime = DateTime.Now
                            }
                        );
                    }
                );
        }

        bool IsCanTryConnect => IPAddress.TryParse(_ipAddress, out var temp) && _port > 0;

        [RelayCommand]
        void ClearLogs() => LogObjs.Clear();

        [RelayCommand]
        void CheckValidation() => ValidateAllProperties();

        [RelayCommand(CanExecute = nameof(IsCanTryConnect))]
        async Task ConnectToServer()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                await _tcpClient.ServiceStart(IPAddress.Parse(_ipAddress), _port ?? 0);
            }
        }

        [RelayCommand]
        async Task DisconnectFromServer() => await _tcpClient.ServiceStop();

        [RelayCommand]
        void DeleteOneItem(LogObj target) => LogObjs.Remove(target);

        int cnt = 0;

        [RelayCommand]
        async Task<string> CommandReturn()
        {
            await Task.Delay(1000);
            return $"Hello!!! 1000 ms 지났다아아아!!!! {cnt++}";
        }

        [RelayCommand]
        async Task SendMessage(string msg)
        {
            WeakReferenceMessenger.Default.Send<string>(msg);
        }

        [RelayCommand]
        private async Task SetSubView(string typeName)
        {
            await Task.Delay(500);
            if (string.IsNullOrEmpty(typeName))
            {
                SubContent = null;
                return;
            }
            var type = Type.GetType($"WpfAppListBindingTest.{typeName}");
            //Type만 가지고 동적으로 객체생성해주는 Activator
            //SubContent = Activator.CreateInstance(type) as FrameworkElement;
            SubContent = App.Current.Services.GetRequiredService(type) as FrameworkElement;
        }

        [RelayCommand]
        async Task BubblingEvent(System.Windows.Input.KeyEventArgs args)
        {
            Debug.Write($"{args.Key} ");
            eventString = DateTime.Now.ToString();
        }
    }

    public partial class LogObj : ObservableValidator
    {
        [ObservableProperty]
        DateTime _recordTime;

        [ObservableProperty]
        string _message;

        [ObservableProperty]
        string _writer;

        [RelayCommand]
        async Task AddString()
        {
            await Task.Delay(1000);
            Message += "#TEST";
        }
    }
}
