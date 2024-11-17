namespace ChatBot.Models
{
    public class AIUser
    {
        //AIUserID
        public int AIUserID { get; set; }
        //UserName
        public string UserName { get; set; }
        //MobileNo
        public string MobileNo { get; set; }
        //EmailID
        public string EmailID { get; set; }
        //TimeStamp
        public DateTime TimeStamp { get; set; }
        //Active
        public bool Active { get; set; }
        //AIUserInfoID
        public int AIUserInfoID { get; set; }
        //Name
        public string Name { get; set; }
        //Password
        public string Password { get; set; }
        //Gender
        public int Gender { get; set; }
        //Latitude
        public string Latitude { get; set; }
        //Longitude
        public string Longitude { get; set; }
        //FCMToken
        public string FCMToken { get; set; }
        //AIUserLoginInfoID
        public int AIUserLoginInfoID { get; set; }
        //AIUserNotificationID
        public int AIUserNotificationID { get; set; }
        //IsNotificationSend
        public bool IsNotificationSend { get; set; }
    }
}
