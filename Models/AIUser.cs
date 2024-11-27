using NuGet.Protocol.Plugins;

namespace ChatBot.Models
{
    public class AIUser
    {
        //AIUserID
        public int? AIUserID { get; set; }
        //UserName
        public string? UserName { get; set; }
        //PhoneNo
        public string? PhoneNo { get; set; }
        //EmailID
        public string? EmailID { get; set; }
        //TimeStamp
        public DateTime? TimeStamp { get; set; }
        //Active
        public bool? Active { get; set; }
        //AIUserInfoID
        public int? AIUserInfoID { get; set; }
        //First Name
        public string? FirstName { get; set; }
        //Last Name
        public string? LastName { get; set; }
        //Password
        public string? Password { get; set; }
        //Confirm Password
        public string? ConfirmPassword { get; set; }
        //Gender
        public GenderType? Gender { get; set; }
        //Latitude
        public string? Latitude { get; set; }
        //Longitude
        public string? Longitude { get; set; }
        //FCMToken
        public string? FCMToken { get; set; }
        //AIUserLoginInfoID
        public int? AIUserLoginInfoID { get; set; }
        //AIUserNotificationID
        public int? AIUserNotificationID { get; set; }
        //IsNotificationSend
        public bool? IsNotificationSend { get; set; }
        public bool? TermsCondition { get; set; }


        public enum GenderType
        {
            
            Male = 1,
            Female = 2,
            Other = 3
        }
    }
}
