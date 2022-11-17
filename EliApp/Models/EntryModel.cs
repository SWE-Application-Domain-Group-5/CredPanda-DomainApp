using EliApp.Areas.Identity.Data;
using EliApp.Migrations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace EliApp.Models
{
    public enum EntryState
    {
        PENDING,
        APPROVED, 
        DECLINED
    }

    public enum AccountType
    {
        Debit,
        Credit
    }

    public class EntryModel
    {
        public int Id { get; set; }
        //public InputModel Input { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile EntryUpload { get; set; } // added
        [Display(Name = "Created by:")]
        [Required]
        public string userId { get; set; }
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        [Display(Name = "Account Involved")]
        public string accountInvolved { get; set; }
        [Display(Name = "Supporting Files")]
        [AllowNull]
        public string supportingFile { get; set; }
        [Display(Name = "Type of Account")]
        public AccountType accountType { get; set; }
        [Display(Name = "Status")]
        public EntryState state { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public float amount { get; set; }
        [AllowNull]
        public string comment { get; set; }

        /* Testing Stuff out for right now
         * protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EntryModel>()
                .HasKey(e => new { e.accountInvolved});
        }
        public class InputModel
        {
        }

        private EntryModel CreateEntry()
        {
            try
            {
                return Activator.CreateInstance<EntryModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(EntryModel)}'. " +
                    $"Ensure that '{nameof(EntryModel)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }*/
    }
}
