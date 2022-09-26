using Microsoft.AspNetCore.Identity;

namespace EliApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the EliAppUser class
public class EliAppUser : IdentityUser
{
    //Get user's first name
    [PersonalData]
    public string? FirstName { get; set; }
    //Get user's last name
    [PersonalData]
    public string? LastName { get; set; }
    //Get user's address
    [PersonalData]
    public string? Address { get; set; }
    //Get user's Date of birth
    [PersonalData]
    public DateTime? DOB { get; set; }
    [PersonalData]
    public  string? ProfilePicture { get; set; }
    [PersonalData]
    //Get user's date of registration
    public DateTime RegisterDate { get; set; }
    //Get user's generated username
    [PersonalData]
    public string? GeneratedUserName { get; set; }
    
    //Get user's activation state
    [PersonalData]
    public bool isActive { get; set; }
    //Get user's List of old passwords
    //[PersonalData]
    //public List<string>? oldPasswords { get; set; } 
    
}

//This is basically a way to add extra attributes to IdentityUser - Eli