namespace FarzamTEWebsite.Models
{
    public class CaseReport
    {
        public int Id { get; set; }
        public required string statuscode { get; set; }
        public required string status { get; set; }
        public required string Broker { get; set; }
        public string? description { get; set; }
        public required string CustomerName { get; set; }
        public required DateTime createdon { get; set; }
        public required string casetype { get; set; }
        public required string title { get; set; }
        public required string owner { get; set; }
        public required string caseAutoNumber { get; set; }
        public required string phonecallReason { get; set; }
        public required string phonecallReasonsDetails { get; set; }
        public DateTime? CaseResolutionCreatedOn { get; set; }
        public string? CaseResolutionSubject { get; set; }
        public string? CaseResolutionDescription { get; set; }
        public string? CaseResolutionsolver { get; set; }
    }
}
