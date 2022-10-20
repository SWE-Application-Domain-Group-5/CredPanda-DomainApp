namespace EliApp.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int AccountNumber { get; set; }
        public string AccountDescription { get; set; }
        public string AccountType { get; set; } //debit or credit
        public string AccountCategory { get; set; }
        public string AccountSubcategory { get; set; }
        public float AccountInitialBalance { get; set; }
        public float AccountCurrentBalance { get; set; }
        public DateTime AccountCreationTime { get; set; }
        public string AccountUserID { get; set; }
        public string AccountOrder { get; set; } //"essentially the order the entries are made i.e. Cash (01), Credit (02). "
        public string AccountStatement { get; set; }
        public string AccountComment { get; set; }
    }
}
