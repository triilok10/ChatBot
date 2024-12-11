namespace ChatBot.Models
{
    public class AIChat
    {
        //UserName
        public string? UserName { get; set; }
        //FirstName
        public string? FirstName { get; set; }
        //LastName
        public string? LastName { get; set; }
        //UserId
        public int? UserID { get; set; }
        //Message
        public string? Message { get; set; }
        //Response
        public bool? Response { get; set; } = false;

    }
}
