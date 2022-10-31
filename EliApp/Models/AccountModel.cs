namespace EliApp.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int AccountNumber { get; set; } //cannot equal another account's number
        public string AccountDescription { get; set; }
        public string AccountType { get; set; } //debit or credit for now
        public string AccountCategory { get; set; }
        public string AccountSubcategory { get; set; }
        public float AccountInitialBalance { get; set; }
        public float AccountCurrentBalance { get; set; }
        public string DisplayInitialBalance { get; set; } //to format the balance into acceptable form use this .ToString("#,##0.00");
        public string DisplayCurrentBalance { get; set; } //to format the balance into acceptable form
        public DateTime AccountCreationTime { get; set; } //the time the account was created
        public string AccountUserID { get; set; } //the userId of the person who made the account
        public string AccountOrder { get; set; } //essentially the order the entries are made i.e. Cash (01), Credit (02)
        public string AccountStatement { get; set; } //Options are BS(Balance Sheet), IS(Income Statement), or RE (Retained Earnings statement)
        public string AccountComment { get; set; }
    }
}
