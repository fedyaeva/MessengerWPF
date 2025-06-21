using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<string> Participants { get; set; } = new();
        public ObservableCollection<Message> Messages { get; set; } = new();
        public ObservableCollection<string> Chats { get; set; } = new ObservableCollection<string>();
        
        
        
        /// <summary>
        /// Мое
        /// </summary>
        public MainViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
            CreatePersonalChatCommand = new RelayCommand(CreatePersonalChat);
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
        public ICommand CreatePersonalChatCommand { get; }
        

        private void SendMessage()
        {
            Messages.Add(new Message
            {
                Sender = Nickname,
                Content = MessageText,
                IsOwnMessage = true
            });
            MessageText = "";
            APIrequest.POSTSendMessage(CurrentUser.currentChatID, MessageText);
            APIrequest.GetChatMessages(CurrentUser.currentChatID);
            LoadInitialData();
        }
        
        /// <summary>
        /// Создание личного чата
        /// </summary>
        /// <param name="participant"></param>
        private void CreatePersonalChat()
        {
            var result = APIrequest.PostCreatePersonalChat(CurrentUser.id_user, "Личный чат");
            if (result.id !=null)
            {
                CurrentUser.currentChatID = result.id;
                LoadInitialData();
            }
        }
        
        /// <summary>
        /// Загрузка данных в чате
        /// </summary>
        private void LoadInitialData()
        {
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
                    Participants.Add(p.userName);                
            }
            /*
            if ((CurrentUser.currentChatID = 0) != null)
            { 
                var participantsList = APIrequest.GETUsers();  
                if (participantsList != null)
                {
                    Participants.Clear();
                    foreach (var p in participantsList)
                        Participants.Add(p.userName);                
                }
            } else{
            var participantsList = APIrequest.GETChatUsers(CurrentUser.id_user);
            if (participantsList != null)
            {
                Participants.Clear();
                foreach (var p in participantsList)
                    Participants.Add(p.username);                
            }
            }*/
        }
        /// <summary>
        /// Обновление сообщений в чате
        /// </summary>
        private void RefreshMessages()
        {
            var messagesList = APIrequest.GetChatMessages(CurrentUser.id_user);
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
            var chatsList = APIrequest.GetChatList(CurrentUser.currentChatID); // Предполагается, что возвращается List<string>
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
                if (!string.IsNullOrEmpty(Nickname))
                {
                    LoadInitialData();
                }
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
