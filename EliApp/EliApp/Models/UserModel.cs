using Microsoft.AspNetCore.Identity;

namespace EliApp.Models
{
    public class UserModel : IdentityUser
    {
        public int Id { get; set; }

        public string? UserName { get; set; }
    }
}