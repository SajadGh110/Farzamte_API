namespace FarzamTEWebsite.DTOs
{
    public class BrokerageMoshtaghe_DTO
    {
        public string Date { get; set; }

        public long TotalMoshtaghe { get; set; }
        public int Brokerage_TotalMoshtaghe { get; set; }
        public int Brokerage_Rank_TotalMoshtaghe { get; set; }
        public int Percent_Moshtaghe { get; set; }

        public long BOBT_Moshtaghe_Normal { get; set; }
        public int Brokerage_BOBT_Moshtaghe_Normal { get; set; }
        public int Brokerage_Rank_BOBT_Moshtaghe_Normal { get; set; }

        public long BOBT_Moshtaghe_Online { get; set; }
        public int Brokerage_BOBT_Moshtaghe_Online { get; set; }
        public int Brokerage_Rank_BOBT_Moshtaghe_Online { get; set; }

        public long FI_Moshtaghe_Station { get; set; }
        public int Brokerage_FI_Moshtaghe_Station { get; set; }
        public int Brokerage_Rank_FI_Moshtaghe_Station { get; set; }

        public long FI_Moshtaghe_Normal { get; set; }
        public int Brokerage_FI_Moshtaghe_Normal { get; set; }
        public int Brokerage_Rank_FI_Moshtaghe_Normal { get; set; }

        public long FI_Moshtaghe_Group { get; set; }
        public int Brokerage_FI_Moshtaghe_Group { get; set; }
        public int Brokerage_Rank_FI_Moshtaghe_Group { get; set; }

        public long FI_Moshtaghe_Other { get; set; }
        public int Brokerage_FI_Moshtaghe_Other { get; set; }
        public int Brokerage_Rank_FI_Moshtaghe_Other { get; set; }
    }
}
