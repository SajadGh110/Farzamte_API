namespace FarzamTEWebsite.Models
{
    public class InComingCall
    {
        public int Id { get; set; }
        public required string from { get; set; }
        public required string to { get; set; }
        public string? automationid { get; set; }
        public string? description { get; set; }
        public required string Broker { get; set; }
        public string? phonecallreason { get; set; }
        public string? phonecallreason2 { get; set; }
        public string? phonecallreason3 { get; set; }
        public string? phonecallreasondetail { get; set; }
        public string? phonecallreasondetail2 { get; set; }
        public string? phonecallreasondetail3 { get; set; }
        public string? phonenumber { get; set; }
        public string? fullName { get; set; }
        public required DateTime createdon { get; set; }
    }
}
