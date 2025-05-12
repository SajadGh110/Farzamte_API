using System.Runtime.Intrinsics.X86;

namespace FarzamTEWebsite.Models
{
    public class InComingCall_Stat
    {
        public int id { get; set; }
        public required string type { get; set; }
        public required string Broker { get; set; }
        public required string Month { get; set; }
        public int Received { get; set; }
        public int Answered { get; set; }
        public int Unanswered { get; set; }
        public int Abandoned { get; set; }
        public int Transferred { get; set; }
        public int Logins { get; set; }
        public int Logoff { get; set; }
        public int Avg_Wait { get; set; }
        public int Avg_Talk { get; set; }
        public int Max_Callers { get; set; }
        public float Answ { get; set; }
        public float Unansw { get; set; }
        public float SLA { get; set; }
    }
}
