using System.ComponentModel.DataAnnotations;

namespace EliApp.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
