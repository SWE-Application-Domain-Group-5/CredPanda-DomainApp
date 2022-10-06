using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EliApp.Models
{
	public class EditUserViewModel
	{
        //Add attributes here to add them to the configuration list
		public EditUserViewModel()
		{
            Roles = new List<string>();
        }
        
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime RegisterDate { get; set; }

        public bool isActive{ get; set; } 

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        

        public IList<string> Roles { get; set; }
    }
}
