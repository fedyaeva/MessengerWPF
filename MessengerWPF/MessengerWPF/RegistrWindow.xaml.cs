using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
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
    string login = TextName.Text.Trim();
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

    var registrData = new
    {
        email = email,
        password = password1
    };

    try
    {
        string jsonData = JsonConvert.SerializeObject(registrData);
        var request = (HttpWebRequest)WebRequest.Create("https://localhost:5064/api/registr"); //Заменить потом 
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

            if (response.StatusCode == HttpStatusCode.OK && result != null)
            {
                CurrentUser.id_user = result.id_user;
                CurrentUser.token = result.token;
                // Тут надо сделать переход к окну чата
                this.Close();
            }
            else
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
    /// Валидайия email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
private bool IsValidEmail(string email)
{
    if(string.IsNullOrWhiteSpace(email))
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