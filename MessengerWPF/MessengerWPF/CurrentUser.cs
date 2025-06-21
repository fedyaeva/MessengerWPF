namespace MessengerWPF;

public static class CurrentUser
{
    public static int id_user { get; set; }
    public static string token { get; set; }


    public static int currentChatID { get; set; } = 1;
    
    public static bool auth { get; set; }
    public static string user_name {get; set;}
    
}