using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using ChatApp.ViewModels;
using Newtonsoft.Json;

namespace MessengerWPF;

public partial class RegistrWindow : Window
{
    public RegistrWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Обработчик нажатия на кнопку "Зарегистрироваться"
    /// </summary>
    private void ButtonReistrClick(object sender, RoutedEventArgs e)
    {
        string name = TextName.Text.Trim();
        string password1 = TextPassword1.Password;
        string password2 = TextPassword2.Password;
        string email = TextEmail.Text.Trim();

        if (password1 != password2)
        {
            TextError.Text = "Введенные пароли не совпадают, повторите ввод";
            return;
        }

        if (!IsValidEmail(email))
        {
            TextError.Text = "Email указан некорректно, повторите ввод";
            return;
        }
        else
        {
            APIRequests APIrequest = new APIRequests();
            RegistrResponse registrResponse = APIrequest.POSTRegistr(name, email, password1);
            if (registrResponse != null)
            {
                CurrentUser.id_user = registrResponse.id;
                CurrentUser.token = registrResponse.token;
                CurrentUser.auth = true;
                MainViewModel mainViewModel = new MainViewModel();
                mainViewModel.Nickname = CurrentUser.user_name;
            }
            else
            {
                TextError.Text = "Произошла ошибка:" + ErrorResponse.errorMessage;   
            }
        }
    }

    /// <summary>
        /// Валидайия email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
}