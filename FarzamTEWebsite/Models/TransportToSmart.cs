namespace FarzamTEWebsite.Models
{
    public class TransportToSmart
    {
        public int Id { get; set; }
        public required string from { get; set; }
        public required string to { get; set; }
        public required string Broker { get; set; }
        public string? description { get; set; }
        public required DateTime createdon { get; set; }
        public string? phonenumber { get; set; }
        public string? customerSatisfaction { get; set; }
        public List<TTS_Reason> reasonOfContinueSmart { get; set; }
        public List<TTS_Reason> reasonOfReturnTadbir { get; set; }
        public string? resultOfCall { get; set; }
        public required string status { get; set; }
        public string? nationalCode { get; set; }
    }

    public class TTS_Reason
    {
        public int Id { get; set; }
        public string name { get; set; }
    }
}
