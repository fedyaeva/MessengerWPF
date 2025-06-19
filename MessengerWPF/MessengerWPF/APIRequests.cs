

using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MessengerWPF;

public class APIRequests
{
    string host = "http://192.168.10.60:896";
    HttpClient httpClient = new HttpClient();
    
    private T Post<T>(string endpoint, object data, HttpStatusCode successStatusCode)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync($"{host}/{endpoint}", content).Result;

            if (response.StatusCode != successStatusCode)
            {
                throw new Exception($"Ошибка: статус {response.StatusCode}");
            }

            var responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseJson);
        }
        catch (HttpRequestException ex)
        {
            ErrorResponse.errorMessage = ex.Message;
            return default(T);
        }
        catch (TaskCanceledException ex)
        {
            ErrorResponse.errorMessage = ex.Message;
            return default(T);
        }
        catch (Exception ex)
        {
            ErrorResponse.errorMessage = ex.Message;
            return default(T);
        }
    }
    

    private T Get<T>(string endpoint, HttpStatusCode successStatusCode)
    {
        try
        {
            var response = httpClient.GetAsync($"{host}/{endpoint}").Result;

            if (response.StatusCode != successStatusCode)
            {
                throw new Exception($"Ошибка: статус {response.StatusCode}");
            }

            var responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseJson);
        }
        catch (HttpRequestException ex)
        {
            ErrorResponse.errorMessage = ex.Message;
            return default(T);
        }
        catch (TaskCanceledException ex)
        {
            ErrorResponse.errorMessage = ex.Message;
            return default(T);
        }
        catch (Exception ex)
        {
            ErrorResponse.errorMessage = ex.Message;
            return default(T);
        }
    }

    public AuthResponse POSTAuth(string email, string password)
    {
            var requestData = new AuthRequest
            {
                email = email,
                password = password
            };
            var result = Post<AuthResponse>("api/auth", requestData, HttpStatusCode.Accepted);
            //поправить эндпоинт
            return result;
    }
    
    public RegistrResponse POSTRegistr(string user_name, string email, string password)
    {
        var requestData = new RegistrRequest
        {
            user_name = user_name,
            email = email,
            password = password
        };
        var result = Post<RegistrResponse>("api/registr", requestData, HttpStatusCode.OK);
        //поправить эндпоинт
        return result;

    }
    
    
    public CreateChatResponse POSTCreateChat(int user_id, string  chat_name)
    {
        var requestData = new CreateChatRequest
        {
            user_id = user_id,
            chat_name = chat_name
        };
        var result = Post<CreateChatResponse>("api/Chat/create", requestData, HttpStatusCode.OK);
        //поправить эндпоинт
        return result;
    }
    
    
    public CreateGroupChatResponse POSTCreateGroupChat(string  chat_name, int id_type_chat)
    {
        var requestData = new CreateGroupChatRequest
        {
            chat_name = chat_name,
            id_type_chat = id_type_chat
        };
        var result = Post<CreateGroupChatResponse>("api/create_group_chat", requestData, HttpStatusCode.OK);
        //поправить эндпоинт
        return result;
    }
    
    public SendMessageResponse POSTSendMessage(int id_chat, string msg_text)
    {
        var requestData = new SendMessageRequest
        {
            id_chat = id_chat,
            msg_text = msg_text
        };
        var result = Post<SendMessageResponse>("api/send_message", requestData, HttpStatusCode.OK);
        //поправить эндпоинт
        return result;
    }
    
    public SendMessageUnrigisterUserResponse PostSendMessageUnrigisterUser (string user_name, int id_chat, string msg_text)
    {
        var requestData = new SendMessageUnrigisterUserRequest
        {
            user_name = user_name,
            id_chat = id_chat,
            msg_text = msg_text
        };
        var result = Post<SendMessageUnrigisterUserResponse>("api/send_message_unrigisterUser", requestData, HttpStatusCode.OK);
        //поправить эндпоинт
        return result;
    }

    public bool POSTAddUserToChat(int id_chat, int id_user)
    {
        
        var requestData = new AddUserToChatRequest
        {
            id_chat = id_chat,
            id_user = id_user
        };

        var response = Post<object>("api/chat/add_user", requestData, HttpStatusCode.Created);
        //поправить эндпоинт
        return response != null;
    }
    
    public List<ChatListResponse> GetChatList(int user_id)
    {
        var result = Get<List<ChatListResponse>>($"api/chats?user_id={user_id}", HttpStatusCode.Accepted);
        //поправить эндпоинт
        return result;
    }
    
    public List<ChatMassagesResponse> GetChatMessages(int id_chat)
    {
        var result = Get<List<ChatMassagesResponse>>($"api/chat_messages?id={id_chat}", HttpStatusCode.Accepted);
        //поправить эндпоинт
        return result;
    }
    
    public List<ChatUsersResponse> GETChatUsers(int id_chat)
    {
        var result = Get<List<ChatUsersResponse>>($"api/chat/chat_users?chat_id={id_chat}", HttpStatusCode.Created);
        //поправить эндпоинт
        return result;
    }
    
    public string GenerateInviteLink(int id_chat)
    {
        var response = Get<GenerateInviteLinkResponse>($"api/chat/invite_link?chat_id={id_chat}", HttpStatusCode.Accepted);
        //поправить эндпоинт
        return response?.invite_link;
    }
    
}
