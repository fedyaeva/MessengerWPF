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
<<<<<<< Updated upstream:ChatApp/ChatApp1/Views/MainWindow.xaml.cs
=======

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            var vm = (ViewModels.MainViewModel)this.DataContext;
            var authWindow = new AuthWindow();
            if (authWindow.ShowDialog() == true)
            {
                vm.Email = authWindow.Email;
                vm.Password = authWindow.Password;
            }
        }
        
>>>>>>> Stashed changes:MessengerWPF/MessengerWPF/Views/MainWindow.xaml.cs
    }
}