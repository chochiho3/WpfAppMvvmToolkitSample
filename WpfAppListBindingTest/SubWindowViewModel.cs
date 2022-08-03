using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System.Threading.Tasks;

namespace WpfAppListBindingTest
{    
    internal partial class SubWindowViewModel : ObservableValidator
    {
        readonly ClientTcp _clientTcp;

        [ObservableProperty]
        string _recvMsg;

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
