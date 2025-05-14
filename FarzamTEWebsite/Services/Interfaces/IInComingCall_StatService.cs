using FarzamTEWebsite.DTOs;

namespace FarzamTEWebsite.Services.Interfaces
{
    public interface IInComingCall_StatService
    {
        Task<List<string>> GetAllTypes(string broker);
        Task<List<string>> GetAllDates(string broker, string type);
        Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type);
        Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type, string month);
    }
}
