using System.ComponentModel.DataAnnotations;

namespace FarzamTEWebsite.Models
{
    public class HappyCall
    {
        public int Id { get; set; }
        public required string CallTo { get; set; }
        public required string CallFrom { get; set; }
        public required string TradeStatus { get; set; }
        public required string statusReason { get; set; }
        public required string nationalCode { get; set; }
        public required string Broker { get; set; }
        public string? phonenumber { get; set; }
        public required DateTime createdon { get; set; }
        public required DateTime RegDate { get; set; }
        public string? introduction { get; set; }
        public string? ChoosingBrokerage { get; set; }
        public bool? ExplanationClub { get; set; }
        public string? UserRequest { get; set; }
        public string? checkingPanel { get; set; }
        public string? CustomerRequirement { get; set; }
        public string? CustomerRequirementDesc { get; set; }
        public string? CustomerRequirement1 { get; set; }
        public string? CustomerRequirementDesc1 { get; set; }
        public string? CustomerRequirement2 { get; set; }
        public string? CustomerRequirementDesc2 { get; set; }
        public string? TradeStatusAffter { get; set; }
        public string? TotalTradeAmount { get; set; }
        public string? totalBrokerCommission { get; set; }
    }
}
