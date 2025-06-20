using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MessengerWPF;

namespace ChatApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Participants { get; set; } = new();
        public ObservableCollection<Message> Messages { get; set; } = new();


        private string _nickname;

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged();
                if (!string.IsNullOrWhiteSpace(value) && !Participants.Contains(value))
                    Participants.Add(value);
            }
        }

        private string _messageText;

        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged();
                (SendMessageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand SendMessageCommand { get; }

        public MainViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
        }

        private void SendMessage()
        {
            Messages.Add(new Message
            {
                Sender = Nickname,
                Content = MessageText,
                IsOwnMessage = true
            });
            MessageText = "";
        }

        private bool CanSendMessage() =>
            !string.IsNullOrWhiteSpace(Nickname) && !string.IsNullOrWhiteSpace(MessageText);

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


        public class Message
        {
            public string Sender { get; set; }
            public string Content { get; set; }
            public bool IsOwnMessage { get; set; }
        }
    }
}
