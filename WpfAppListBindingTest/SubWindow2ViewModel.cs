using CommunityToolkit.Mvvm.ComponentModel;
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
        [ObservableProperty]
        string _recvMsg;

        public SubWindow2ViewModel()
        {
            WeakReferenceMessenger.Default.Register<string>(
                this,
                (r, m) =>
                {
                    RecvMsg += $":{m}";
                }
            );
        }
    }
}
