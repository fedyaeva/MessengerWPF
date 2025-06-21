using System.Collections.ObjectModel;
using System.Diagnostics;
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
            Debug.WriteLine($"currentChatID после вызовa API: {CurrentUser.currentChatID}");
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
            var vm = (ViewModels.MainViewModel)this.DataContext;
            var authWindow = new AuthWindow();
            if (authWindow.ShowDialog() == true)
            {
                vm.Email = authWindow.Email;
                vm.Password = authWindow.Password;
            }
        }

        private void ParticipantsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mainViewModel = this.DataContext as MainViewModel;
            if (mainViewModel == null)
                return;

            var listBox = sender as ListBox;
            if (listBox == null)
                return;

            var participant = listBox.SelectedItem as Participant;
            if (participant != null)
            {
                mainViewModel.CreatePersonalChat(participant.Id);
            }
        }
    }
}