namespace FarzamTEWebsite.Models
{
    public class Notice_Call
    {
        public int id { get; set; }
        public required string noticeid { get; set; }
        public required string fullName { get; set; }
        public required DateTime createdon { get; set; }
        public required string Broker { get; set; }
        public string? typeofcapitalincrease { get; set; }
        public string? noticetype { get; set; }
        public string? symbol { get; set; }
        public string? company { get; set; }
        public string? description { get; set; }
        public required string expert { get; set; }
        public string? statusReason { get; set; }
    }
}
