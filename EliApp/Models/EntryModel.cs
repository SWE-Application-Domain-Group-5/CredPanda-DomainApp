namespace EliApp.Models
{
    public enum EntryState
    {
        DECLINED, 
        PENDING, 
        APPROVED
    }

    public enum AccountType
    {
        Debit, 
        Credit
    }

    public class EntryModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string accountInvolved { get; set; }
        public string supportingFile { get; set; }
        public AccountType accountType { get; set; }
        public EntryState state { get; set; }
        public float amount { get; set; }
    }
}
