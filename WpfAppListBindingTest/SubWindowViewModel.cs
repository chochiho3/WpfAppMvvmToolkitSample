using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System.Threading.Tasks;

namespace WpfAppListBindingTest
{
    internal partial class SubWindowViewModel : ObservableValidator
    {
        [ObservableProperty]
        private string _recvMsg;

        public SubWindowViewModel()
        {
            WeakReferenceMessenger.Default.Register<string>(
                this,
                (r, m) =>
                {
                    RecvMsg = m;
                }
            );
        }
    }
}
