namespace FarzamTEWebsite.Models
{
    public class Transaction_Statistics_M
    {
        public int Id { get; set; }
        public int Brokerage_ID { get; set; }
        public int? BOBT_Oragh_Bedehi_Online { get; set; }
        public int? BOBT_Oragh_Bedehi_Normal { get; set; }
        public int? BOBT_Moshtaghe_Online { get; set; }
        public int? BOBT_Moshtaghe_Normal { get; set; }
        public int? BOBT_Sarmaye_Herfei_Online { get; set; }
        public int? BOBT_Sarmaye_Herfei_Normal { get; set; }
        public int? BOBT_Sarmaye_Herfei_Algorithm { get; set; }
        public int? BOBT_saham_Online { get; set; }
        public int? BOBT_saham_Normal { get; set; }
        public int? BOBT_saham_Algorithm { get; set; }
        public int? BOBT_Sandogh_Online { get; set; }
        public int? BOBT_Sandogh_Algorithm { get; set; }
        public long? BOBT_Total_Value { get; set; }
        public int? FI_Brokerage_Station { get; set; }
        public int? FI_Moshtaghe_Station { get; set; }
        public int? FI_Online_Normal { get; set; }
        public int? FI_Moshtaghe_Normal { get; set; }
        public int? FI_Online_Group { get; set; }
        public int? FI_Moshtaghe_Group { get; set; }
        public int? FI_Online_Other { get; set; }
        public int? FI_Moshtaghe_Other { get; set; }
        public long? FI_Total_Value { get; set; }
        public long? BOBT_AND_FI_Total_Value { get; set; }
        public int? BKI_Physical { get; set; }
        public int? BKI_Self { get; set; }
        public int? BKI_Ati { get; set; }
        public int? BKI_Ekhtiar { get; set; }
        public long? BKI_Total_Value { get; set; }
        public int? BEI_Physical { get; set; }
        public int? BEI_Moshtaghe { get; set; }
        public int? BEI_Other { get; set; }
        public long? BEI_Total_Value { get; set; }
        public long? All_Total_Value { get; set; }
        public string? Date_Monthly { get; set; }

        public int? TotalMoshtaghe
        {
            get
            {
                return (BOBT_Moshtaghe_Normal ?? 0) + (BOBT_Moshtaghe_Online ?? 0) + (FI_Moshtaghe_Station ?? 0)
                    + (FI_Moshtaghe_Normal ?? 0) + (FI_Moshtaghe_Group ?? 0) + (FI_Moshtaghe_Other ?? 0);
            }
        }

        public int? TotalOnline
        {
            get
            {
                return (BOBT_Oragh_Bedehi_Online ?? 0) + (BOBT_Moshtaghe_Online ?? 0) + (BOBT_Sarmaye_Herfei_Online ?? 0)
                    + (BOBT_Sarmaye_Herfei_Algorithm ?? 0) + (BOBT_saham_Online ?? 0) + (BOBT_saham_Algorithm ?? 0)
                    + (BOBT_Sandogh_Online ?? 0) + (BOBT_Sandogh_Algorithm ?? 0) + (FI_Online_Normal ?? 0)
                    + (FI_Online_Group ?? 0) + (FI_Online_Other ?? 0);
            }
        }
    }
}
