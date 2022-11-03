using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliApp.Models
{
    public enum Statement
    {
        BalanceSheet,
        IncomeStatement, 
        RetainedEarnings
    }

    public class AccountModel
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int AccountNumber { get; set; } //cannot equal another account's number
        public string AccountDescription { get; set; }
        public AccountType AccountType { get; set; } //debit or credit for now
        public string AccountCategory { get; set; } //asset or whatever
        public string AccountSubcategory { get; set; } //something like: "Current assets"
        
        [Column(TypeName = "decimal(18, 2)")]
        public float AccountInitialBalance { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public float AccountCurrentBalance { get; set; }
        
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime AccountCreationTime { get; set; } //the time the account was created
        public string AccountUserID { get; set; } //the userId of the person who made the account
        public int AccountOrder { get; set; } //essentially the order the entries are made i.e. Cash (01), Credit (02)
        public Statement AccountStatement { get; set; } //Options are BS(Balance Sheet), IS(Income Statement), or RE (Retained Earnings statement)
        public string AccountComment { get; set; }
    }
}
