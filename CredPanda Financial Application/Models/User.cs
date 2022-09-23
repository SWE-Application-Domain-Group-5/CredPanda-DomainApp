using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CredPanda_Financial_Application.Models
{
    public class User
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string role { get; set; }
        public string dateOfBirth { get; set; }

        public string dateJoined;

        //unpublic and unmap these later
        public string username { get; set; }

        public string picture { get; set; }
       
        public string password { get; set; }
        

        public List<string> oldPasswords;

        public User()
        {
            oldPasswords = new List<string>();
            dateJoined = DateTime.Today.ToShortDateString();
            Console.WriteLine(dateJoined);
        }

        public string CreateUsername()
        {
            //Take the first letter from the first name
            StringBuilder sb = new StringBuilder(firstName, 1);
            //Take the whole of the last name
            sb.Append(lastName);
            //Add in the date as needed
            //Very long and complex, and it's very likely that this can be simplified
            string[] date = dateJoined.Split('/');
            //Get the month portion
            sb.Append(date[1]);
            //Get the last two digits of the year
            sb.Append(date[2].ElementAt(2) + date[2].ElementAt(3));

            //Set the username to the created string and return it
            username = sb.ToString();
            return username;
        }

        //Will return true if the password being submitted has already been used, otherwise, it will return false after it changes the password.
        public bool setPassword(string pass)
        {
            foreach (var p in oldPasswords)
            {
                if(password == p)
                {
                    return true;
                }            
            }

            password = pass;
            oldPasswords.Add(password);
            return false;
        }

        public List<string> ListExpiredPasswords()
        {
            return oldPasswords;
        }

        public string getPassword()
        {
            return password;
        }

        public bool passwordExpired()
        {
            //add conditions for having this return true.
            return false;
        }
    }
}
