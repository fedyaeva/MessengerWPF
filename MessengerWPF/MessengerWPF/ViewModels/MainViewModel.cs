using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MessengerWPF;

namespace ChatApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        APIRequests APIrequest = new APIRequests();
        Participant _participant = new Participant();
        public ObservableCollection<Participant> Participants { get; set; } = new();
        public ObservableCollection<Message> Messages { get; set; } = new();
        public ObservableCollection<string> Chats { get; set; } = new ObservableCollection<string>();

        
        public MainViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
            CreatePersonalChatCommand = new RelayCommand1(param =>
            {
                if (param is Participant participant)
                    CreatePersonalChat(participant.Id);
            });
            StartPeriodicUpdates();
            LoadInitialData();
        }
        
        private string _nickname;

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged();
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
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand { get; }
        public ICommand CreatePersonalChatCommand { get; }

/// <summary>
/// Отправка сообщений
/// </summary>
        private void SendMessage()
        {
            var messageContent = MessageText;
            if (string.IsNullOrWhiteSpace(messageContent))
                return;

            Messages.Add(new Message
            {
                Sender = Nickname,
                Content = messageContent,
                IsOwnMessage = true
            });
            MessageText = "";
            Debug.WriteLine($"currentChatID перед вызовом API: {CurrentUser.currentChatID}");
            APIrequest.POSTSendMessage(CurrentUser.currentChatID, messageContent);
            Debug.WriteLine($"currentChatID gjckt вызовом API: {CurrentUser.currentChatID}");
            APIrequest.GetChatMessages(CurrentUser.currentChatID);
            Debug.WriteLine($"currentChatID после вызовом API: {CurrentUser.currentChatID}");
            LoadInitialData();
        }

        /// <summary>
        /// Создание личного чата
        /// </summary>
        /// <param name="participant"></param>
        public void CreatePersonalChat(int userId)
        {
            Debug.WriteLine($"currentChatID перед вызовом API: {CurrentUser.currentChatID}");
            var result = APIrequest.PostCreatePersonalChat(userId, "Личный чат");
            if (result != null && result.id != null)
            {
                CurrentUser.currentChatID = result.id;
                LoadInitialData();
            }
        }

        /// <summary>
        /// Загрузка данных в чате
        /// </summary>
        public void LoadInitialData()
        {
            Debug.WriteLine($"CurrentUser.auth: {CurrentUser.auth}");
            RefreshParticipants();
            RefreshMessages();
            if (CurrentUser.auth)
            {
                RefreshUserChats();
            }
        }

        /// <summary>
        /// Обновление списка участников чата
        /// </summary>
        private void RefreshParticipants()
        {
            var participantsList = APIrequest.GETUsers();
            if (participantsList != null)
            {
                Participants.Clear();
                foreach (var p in participantsList)
                    Participants.Add(new Participant { Id = p.id, UserName = p.userName });
            }
            /*
            if ((CurrentUser.currentChatID = 0) != null)
            {
                var participantsList = APIrequest.GETUsers();
                if (participantsList != null)
                {
                    Participants.Clear();
                    foreach (var p in participantsList)
                        Participants.Add(new Participant { Id = p.id, UserName = p.userName });
                }
            } else{
            var participantsList = APIrequest.GETChatUsers(CurrentUser.id_user);
            if (participantsList != null)
            {
                Participants.Clear();
                foreach (var p in participantsList)
                    Participants.Add(new Participant { Id = p.id, UserName = p.userName });
            }
            }*/
        }

        private Participant selectedParticipant;

        public Participant SelectedParticipant
        {
            get => selectedParticipant;
            set
            {
                selectedParticipant = value;
                OnPropertyChanged();
                if (selectedParticipant != null)
                {
                    CreatePersonalChat(selectedParticipant.Id);
                }
            }
        }

        /// <summary>
        /// Обновление сообщений в чате
        /// </summary>
        private void RefreshMessages()
        {
            var messagesList = APIrequest.GetChatMessages(CurrentUser.currentChatID);
            if (messagesList != null)
            {
                Messages.Clear();
                foreach (var m in messagesList)
                    Messages.Add(new Message
                    {
                        Sender = Nickname,
                        Content = m.msg_text,
                        IsOwnMessage = true
                    });
            }
        }

        public bool IsOwnMessage { get; set; }

        /// <summary>
        /// Обновление списка чатов пользователя
        /// </summary>
        private void RefreshUserChats()
        {
            var chatsList = APIrequest.GetChatList(); 
            if (chatsList != null)
            {
                Chats.Clear();
                foreach (var chat in chatsList)
                    Chats.Add(chat.chat_name);
            }
        }

        /// <summary>
        /// Обновление данных по таймеру
        /// </summary>
        private void StartPeriodicUpdates()
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += (s, e) =>
            {
                    LoadInitialData();
            };
            timer.Start();
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