namespace FarzamTEWebsite.Models
{
    public class Notice_SMS
    {
        public int id { get; set; }
        public string? noticeid { get; set; }
        public required string fullName { get; set; }
        public required DateTime modifiedon { get; set; }
        public required string Broker { get; set; }
        public string? typeofcapitalincrease { get; set; }
        public string? noticetype { get; set; }
        public string? symbol { get; set; }
        public string? company { get; set; }
        public string? response_id { get; set; }
        public string? description { get; set; }
        public string? expert { get; set; }
    }
}
