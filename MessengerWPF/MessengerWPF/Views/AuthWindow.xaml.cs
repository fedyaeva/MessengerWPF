using System.Net;
using System.Windows;
using ChatApp.ViewModels;
using Newtonsoft.Json;

namespace MessengerWPF;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Обработчик нажатия на кнопку "Войти"
    /// </summary>
    [Obsolete("Obsolete")]
    private void ButtonLoginClick(object sender, RoutedEventArgs e)
    {
        string email = TextEmail.Text.Trim();
        string password = TextPassword.Password;

        if (string.IsNullOrEmpty(email))
        {
            TextError.Text = "Необходимо указать Email";
        }

        if (string.IsNullOrEmpty(password))
        {
            TextError.Text = "Необходимо указать пароль";
        }
        
        APIRequests APIrequest = new APIRequests();
        AuthResponse authResponse = APIrequest.POSTAuth(email, password);
        if (authResponse != null)
        {
            CurrentUser.id_user = authResponse.id;
            CurrentUser.token = authResponse.token;
            CurrentUser.auth = true;
            MainViewModel mainViewModel = new MainViewModel();
            mainViewModel.Nickname = $"Пользователь {CurrentUser.id_user}"; //Тут изменить на имя пользователя, если добавят возврат
        }
        else
        {
            TextError.Text = "Произошла ошибка:" + ErrorResponse.errorMessage;   
        }
    }

    /// <summary>
    /// Обработчик нажатия на кнопку "Регистрация"
    /// </summary>
    private void ButtonRegisterClick(object sender, RoutedEventArgs e)
    {
        RegistrWindow registrWindow = new RegistrWindow();
        registrWindow.Show();
        this.Close();
    }
}