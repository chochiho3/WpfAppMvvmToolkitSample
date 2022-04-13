using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppListBindingTest
{
    //이건 아직 확인이 잘 안됨... 추가 확인 필요.
    //[INotifyPropertyChanged]
    //internal partial class MainWindowViewModel
    internal partial class MainWindowViewModel : ObservableValidator
    {
        readonly ClientTcp _tcpClient;

        [ObservableProperty]
        ObservableCollection<LogObj> _logObjs;

        [ObservableProperty]
        [Required]
        [AlsoNotifyChangeFor(nameof(FullAddress))]
        [AlsoNotifyCanExecuteFor(nameof(ConnectToServerCommand))]
        [CustomValidation(typeof(ValidatorBundle), nameof(ValidatorBundle.CustomValidation))]
        string _ipAddress;

        [ObservableProperty]
        [Required]
        [AlsoNotifyChangeFor(nameof(FullAddress))]
        [AlsoNotifyCanExecuteFor(nameof(ConnectToServerCommand))]
        int? _port;

        public string FullAddress => $"{IpAddress}:{Port}";

        [ObservableProperty]
        FrameworkElement _subContent;

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

        [ICommand]
        void ClearLogs() => LogObjs.Clear();

        [ICommand]
        void CheckValidation() => ValidateAllProperties();

        [ICommand(CanExecute = nameof(IsCanTryConnect))]
        async Task ConnectToServer()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                await _tcpClient.ServiceStart(IPAddress.Parse(_ipAddress), _port ?? 0);
            }
        }

        [ICommand]
        async Task DisconnectFromServer() => await _tcpClient.ServiceStop();

        [ICommand]
        void DeleteOneItem(LogObj target) => LogObjs.Remove(target);

        int cnt = 0;

        [ICommand]
        async Task<string> CommandReturn()
        {
            await Task.Delay(1000);
            return $"Hello!!! 1000 ms 지났다아아아!!!! {cnt++}";
        }

        [ICommand]
        async Task SendMessage(string msg)
        {
            WeakReferenceMessenger.Default.Send<string>(msg);
        }

        int _subContentIndex = 0;

        [ICommand]
        async Task ChangeSubContent()
        {
            if (_subContentIndex % 2 == 0)
            {
                SubContent = new SubWindow();
            }
            else
            {
                SubContent = new SubWindow2();
            }
            _subContentIndex++;
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

        [ICommand]
        async Task AddString()
        {
            await Task.Delay(1000);
            Message += "#TEST";
        }
    }
}
