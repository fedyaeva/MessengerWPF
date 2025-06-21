using System.Net;
using System.Windows;
using ChatApp;
using ChatApp.ViewModels;
using Newtonsoft.Json;

namespace MessengerWPF;

public partial class AuthWindow : Window
{
    public string Email { get; set; }
    public string Password { get; set; }
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
        Email = TextEmail.Text.Trim();
        Password = TextPassword.Password;

        if (string.IsNullOrEmpty(Email))
        {
            TextError.Text = "Необходимо указать Email";
        }

        if (string.IsNullOrEmpty(Password))
        {
            TextError.Text = "Необходимо указать пароль";
        }
        
        APIRequests APIrequest = new APIRequests();
        AuthResponse authResponse = APIrequest.POSTAuth(Email, Password);
        if (authResponse != null)
        {
            CurrentUser.token = authResponse.token;
            CurrentUser.auth = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        else
        {
            TextError.Text = ErrorResponse.errorMessage;   
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