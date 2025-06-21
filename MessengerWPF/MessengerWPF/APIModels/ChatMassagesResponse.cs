namespace MessengerWPF;

public class ChatMassagesResponse
{
    public int id{get;set;}

    public string msg_text { get;set; }
    public string create_date { get; set; }
    public int id_user {get;set;}
    public int id_chat{get;set;}
    public string user_name {get;set;}
}