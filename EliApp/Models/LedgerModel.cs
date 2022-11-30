using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EliApp.Models
{
    public class LedgerModel
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        [AllowNull]
        [ValidateNever]
        public string description { get; set; }
        [Display(Name = "Debits")]
        public float debitAmount { get; set; }
        [Display(Name = "Credits")]
        public float creditAmount { get; set; }
        [Display(Name = "Total Balance")]
        public float balance { get; set; }
        [Display(Name = "Account")]
        public string associatedAccountName { get; set; }
        public int accountID { get; set; }
        public int journalEntryID { get; set; }
    }
}
