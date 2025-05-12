namespace FarzamTEWebsite.DTOs
{
    public class InComingCall_Stat_DTO
    {
        public string Type { get; set; }
        public string Broker { get; set; }
        public string Month { get; set; }
        public int Answered { get; set; }
        public int Avg_Wait { get; set; }
        public int Avg_Talk { get; set; }
    }
}
