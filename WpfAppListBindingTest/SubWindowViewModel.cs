﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System.Threading.Tasks;

namespace WpfAppListBindingTest
{
    internal partial class SubWindowViewModel : ObservableValidator
    {
        private readonly ClientTcp _clientTcp;

        [ObservableProperty]
        private string _recvMsg;

        public SubWindowViewModel(ClientTcp clientTcp)
        {
            WeakReferenceMessenger.Default.Register<string>(
                this,
                (r, m) =>
                {
                    RecvMsg += m;
                }
            );
            _clientTcp = clientTcp;
            _clientTcp.OnRecvMsg += (s, e) => RecvMsg += e;
        }
    }
}
