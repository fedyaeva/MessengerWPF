using System.Net;
using System.Windows;
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

        var authData = new
        {
            email = email,
            password = password
        };

        try
        {
            string jsonData = JsonConvert.SerializeObject(authData);
            var request = (HttpWebRequest)WebRequest.Create("https://localhost:5064/api/auth"); //Заменить потом 
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
            }
            var response = (HttpWebResponse)request.GetResponse();
            
            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();

                var result = JsonConvert.DeserializeObject<AuthResponse>(responseText);

                if (response.StatusCode == HttpStatusCode.Accepted && result != null)
                {
                    CurrentUser.id_user = result.id_user;
                    CurrentUser.token = result.token;
                    // Тут надо сделать переход к окну чата
                    this.Close();
                } else
                {
                    TextError.Text = result?.message ?? "Ошибка авторизации.";
                }    
            }
        }
        catch (WebException ex)
        {
            using var stream = ex.Response?.GetResponseStream();
            if (stream != null)
            {
                using var reader = new System.IO.StreamReader(stream);
                string errorResponse = reader.ReadToEnd();
                TextError.Text = $"Ошибка: {errorResponse}";
            }
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