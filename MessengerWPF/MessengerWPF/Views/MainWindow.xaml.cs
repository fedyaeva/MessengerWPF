using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using ChatApp.ViewModels;
using MessengerWPF;

namespace ChatApp
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Nickname_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ViewModels.MainViewModel)this.DataContext;

            var dialog = new NicknameWindow();
            if (dialog.ShowDialog() == true)
            {
                vm.Nickname = dialog.Nickname;
            }
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            var authWindow = new AuthWindow();
            authWindow.Show();
        }
        
    }
}