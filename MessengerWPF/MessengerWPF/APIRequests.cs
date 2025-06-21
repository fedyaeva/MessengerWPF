

using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MessengerWPF;

public class APIRequests
{
    string host = "http://localhost:5064";
    HttpClient httpClient = new HttpClient();

    private T Post<T>(string endpoint, object data, HttpStatusCode successStatusCode, bool addAuthorizationHeader = false)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            if (addAuthorizationHeader)
            {
                int userId = CurrentUser.id_user;
                if (httpClient.DefaultRequestHeaders.Contains("X-User-Id"))
                {
                    httpClient.DefaultRequestHeaders.Remove("X-User-Id");
                }
                httpClient.DefaultRequestHeaders.Add("X-User-Id", $"Bearer {userId}");
            }

            var response = httpClient.PostAsync($"{host}/{endpoint}", content).Result;

            if (response.StatusCode != successStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception($"Error: {responseBody}");
            }

            var responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseJson);
        }
        catch (HttpRequestException ex)
        {
            ErrorResponse.errorMessage = $"Error: {ex.Message}";
            return default(T);
        }
        catch (TaskCanceledException ex)
        {
            ErrorResponse.errorMessage = $"Error: {ex.Message}";
            return default(T);
        }
        catch (Exception ex)
        {
            ErrorResponse.errorMessage = ex.Message; // тут уже сообщение из исключения
            return default(T);
        }
    }

    private T Get<T>(string endpoint, HttpStatusCode successStatusCode, bool addAuthorizationHeader)
    {
            
        if (addAuthorizationHeader)
        {
            int userId = CurrentUser.id_user;
            if (httpClient.DefaultRequestHeaders.Contains("X-User-Id"))
            {
                httpClient.DefaultRequestHeaders.Remove("X-User-Id");
            }
            httpClient.DefaultRequestHeaders.Add("X-User-Id", $"Bearer {userId}");
        }
        try
        {
            var response = httpClient.GetAsync($"{host}/{endpoint}").Result;

            if (response.StatusCode != successStatusCode)
            {
                var responseBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception($"Error: {responseBody}");
            }

            var responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseJson);
        }
        catch (HttpRequestException ex)
        {
            ErrorResponse.errorMessage = $"Error: {ex.Message}";
            return default(T);
        }
        catch (TaskCanceledException ex)
        {
            ErrorResponse.errorMessage = $"Error: {ex.Message}";
            return default(T);
        }
        catch (Exception ex)
        {
            ErrorResponse.errorMessage = ex.Message; 
            return default(T);
        }
    }

    /// <summary>
    /// Авторизация пользователя 
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    public AuthResponse POSTAuth(string email, string password)
    {
        var requestData = new AuthRequest
        {
            email = email,
            password = password
        };
        var result = Post<AuthResponse>("api/Users/login", requestData, HttpStatusCode.OK);
        return result;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="user_name">Имя пользователя</param>
    /// <param name="email">Email</param>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    public RegistrResponse POSTRegistr(string user_name, string email, string password)
    {
        var requestData = new RegistrRequest
        {
            username = user_name,
            email = email,
            password = password
        };
        var result = Post<RegistrResponse>("api/Users/register", requestData, HttpStatusCode.Created);
        return result;

    }
    
    


    /// <summary>
    /// Cоздание чата
    /// </summary>
    /// <param name="id_type_chat">1 - личный, 2 - групповой</param>
    /// <param name="chat_name"></param>
    /// <returns></returns>
    public CreateChatResponse PostCreateChat(int id_type_chat, string chat_name)
    {
        var requestData = new CreateChatRequest
        {
            id_type_chat = id_type_chat,
            chat_name = chat_name
        };
        var result = Post<CreateChatResponse>("api/Chat/create", requestData, HttpStatusCode.OK, true);
        return result;
    }


    /// <summary>
    /// Создание личного чата 
    /// </summary>
    /// <param name="user_id">Ид пользователя, с которым будет создан чат</param>
    /// <param name="chat_name">Название чата</param>
    /// <returns></returns>
    public CreatePersonalChatResponse PostCreatePersonalChat(int user_id, string chat_name)
    {
        var requestData = new CreatePersonalChatRequest
        {
            user_id = user_id,
            chat_name = chat_name
        };
        var result = Post<CreatePersonalChatResponse>("api/Chat/personal", requestData, HttpStatusCode.OK);
        return result;
    }

    
    /// <summary>
    /// Добавление пользователя в чат
    /// </summary>
    /// <param name="id_chat">ИД чата</param>
    /// <param name="id_user">ИД пользователя, которого нужно пригласить в чат</param>
    /// <returns></returns>
    public bool POSTAddUserToChat(int id_chat, int id_user)
    {

        var requestData = new AddUserToChatRequest
        {
            id_chat = id_chat,
            id_user = id_user
        };

        var response = Post<object>("api/Chat/add_user", requestData, HttpStatusCode.Created);
        return response != null;
    }

    /// <summary>
    /// Получение списка чатов пользователя
    /// </summary>
    /// <param name="user_id">ИД пользователя</param>
    /// <returns></returns>
    public List<ChatListResponse> GetChatList()
    {
        var result = Get<List<ChatListResponse>>($"api/Chat/chats", HttpStatusCode.Accepted, true);
        //поправить эндпоинт
        return result;
    }
    
    
    /// <summary>
    /// Получения списка пользователей в чате
    /// </summary>
    /// <param name="id_chat">ИД чата</param>
    /// <returns></returns>
    public List<ChatUsersResponse> GETChatUsers(int id_chat)
    {
        var result = Get<List<ChatUsersResponse>>($"api/Users/user_list?idChat={id_chat}", HttpStatusCode.Accepted, true);
        return result;
    }
    
    /// <summary>
    /// Получения списка всех пользователей
    /// </summary>
    /// <returns></returns>
    public List<UsersResponse> GETUsers()
    {
        var result = Get<List<UsersResponse>>($"api/Users/all_user", HttpStatusCode.OK, false);
        return result;
    }
    
    /// <summary>
    /// Генерация ссылки для
    /// </summary>
    /// <param name="id_chat">ИД чата</param>
    /// <returns></returns>
    public string GenerateInviteLink(int id_chat)
    {
        var response = Get<GenerateInviteLinkResponse>($"api/Chat/link={id_chat}", HttpStatusCode.Accepted, false);
        return response?.invite_link;
    }
    
    /// <summary>
    /// Получение списка сообщений в чате
    /// </summary>
    /// <param name="id_chat">ИД чата</param>
    /// <returns></returns>
    public List<ChatMassagesResponse> GetChatMessages(int id_chat)
    {
        var result = Get<List<ChatMassagesResponse>>($"api/Message/{id_chat}", HttpStatusCode.Accepted, false);
        return result;
    }

    /// <summary>
    /// Отправка сообщения в чат
    /// </summary>
    /// <param name="id_chat">ИД чата</param>
    /// <param name="msg_text">Текст сообщения</param>
    /// <returns></returns>
    public SendMessageResponse POSTSendMessage(int id_chat, string msg_text)
    {
        var requestData = new SendMessageRequest
        {
            id_chat = id_chat,
            msg_text = msg_text
        };
        var result = Post<SendMessageResponse>("api/Message", requestData, HttpStatusCode.OK);
        return result;
    }

    /// <summary>
    /// Отправка сообщения в чат от незарегистрированного пользователя
    /// </summary>
    /// <param name="user_name">Имя пользователя</param>
    /// <param name="id_chat">ИД чата</param>
    /// <param name="msg_text">Текст сообщения</param>
    /// <returns></returns>
    public SendMessageUnrigisterUserResponse PostSendMessageUnrigisterUser(string user_name, int id_chat,
        string msg_text)
    {
        var requestData = new SendMessageUnrigisterUserRequest
        {
            user_name = user_name,
            id_chat = id_chat,
            msg_text = msg_text
        };
        var result =
            Post<SendMessageUnrigisterUserResponse>("api/send_message_unrigisterUser", requestData, HttpStatusCode.OK);
        //поправить эндпоинт
        return result;
    }
}   
    

