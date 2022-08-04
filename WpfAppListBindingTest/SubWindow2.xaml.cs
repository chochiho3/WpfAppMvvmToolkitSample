using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppListBindingTest
{
    /// <summary>
    /// SubWindow2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubWindow2 : UserControl
    {
        public SubWindow2(SubWindow2ViewModel viewModel)
        {
            InitializeComponent();
            //DataContext = App.Current.Services.GetService(typeof(SubWindow2ViewModel));
            DataContext = viewModel;
        }
    }
}
