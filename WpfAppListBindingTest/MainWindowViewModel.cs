using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace WpfAppListBindingTest
{
    //이건 아직 확인이 잘 안됨... 추가 확인 필요.
    //[INotifyPropertyChanged]
    //internal partial class MainWindowViewModel
    internal partial class MainWindowViewModel : ObservableValidator
    {
        private readonly ClientTcp _tcpClient;

        [ObservableProperty]
        private ObservableCollection<LogObj> _logObjs;

        [ObservableProperty]
        [Required]
        [AlsoNotifyChangeFor(nameof(FullAddress))]
        [AlsoNotifyCanExecuteFor(nameof(ConnectToServerCommand))]
        [CustomValidation(typeof(ValidatorBundle), nameof(ValidatorBundle.CustomValidation))]
        private string _ipAddress;

        [ObservableProperty]
        [Required]
        [AlsoNotifyChangeFor(nameof(FullAddress))]
        [AlsoNotifyCanExecuteFor(nameof(ConnectToServerCommand))]
        private int? _port;

        public string FullAddress => $"{IpAddress}:{Port}";

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

        private bool IsCanTryConnect => IPAddress.TryParse(_ipAddress, out var temp) && _port > 0;

        [ICommand]
        private void ClearLogs() => LogObjs.Clear();

        [ICommand]
        private void CheckValidation() => ValidateAllProperties();

        [ICommand(CanExecute = nameof(IsCanTryConnect))]
        private async Task ConnectToServer()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                await _tcpClient.ServiceStart(IPAddress.Parse(_ipAddress), _port ?? 0);
            }
        }

        [ICommand]
        private async Task DisconnectFromServer() => await _tcpClient.ServiceStop();

        [ICommand]
        private void DeleteOneItem(LogObj target) => LogObjs.Remove(target);

        private int cnt = 0;

        [ICommand]
        private async Task<string> CommandReturn()
        {
            await Task.Delay(1000);
            return $"Hello!!! 1000 ms 지났다아아아!!!! {cnt++}";
        }

        [ICommand]
        private async Task SendMessage(string msg)
        {
            WeakReferenceMessenger.Default.Send<string>(msg);
        }
    }

    public partial class LogObj : ObservableValidator
    {
        [ObservableProperty]
        private DateTime _recordTime;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private string _writer;

        [ICommand]
        private async Task AddString()
        {
            await Task.Delay(1000);
            Message += "#TEST";
        }
    }
}
