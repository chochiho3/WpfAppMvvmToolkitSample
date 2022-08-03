using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppListBindingTest
{    
    internal partial class SubWindow2ViewModel : ObservableValidator
    {
        private readonly ClientTcp cli;

        [ObservableProperty]
        string _recvMsg;

        public SubWindow2ViewModel(ClientTcp cli)
        {
            WeakReferenceMessenger.Default.Register<string>(
                this,
                (r, m) =>
                {
                    RecvMsg += $":{m}";
                }
            );
            this.cli = cli;
            cli.OnRecvMsg += (s, e) => RecvMsg += $"Sock:{e}";
        }

        [RelayCommand]
        async Task TestFunc()
        {
            System.Diagnostics.Debug.Write("Test Func");
        }
    }
}
