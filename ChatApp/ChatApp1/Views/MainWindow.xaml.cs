using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Authorize_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ViewModels.MainViewModel)this.DataContext;

            var dialog = new NicknameWindow();
            if (dialog.ShowDialog() == true)
            {
                vm.Nickname = dialog.Nickname;
            }
        }
    }
}