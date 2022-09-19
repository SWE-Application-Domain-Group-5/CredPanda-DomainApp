namespace CredPanda_Financial_Application.Models
{
    public class User
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string role { get; set; }
        public string dateOfBirth { get; set; }
        public string dateJoined { get; set; }
        public string username { get; set; }
        public string picture { get; set; }
        public string password { get; set; }

        public List<string> oldPasswords;

        public User()
        {

        }
    }
}
