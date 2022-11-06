namespace EliApp.Models
{
    public class LedgerModel
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public string description { get; set; }
        public List<float> debits { get; set; }
        public List<float> credits { get; set; }
        public float balance { get; set; }
        public string journalEntry { get; set; }
    }
}
