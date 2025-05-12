using FarzamTEWebsite.DTOs;

namespace FarzamTEWebsite.Services.Interfaces
{
    public interface IInComingCall_StatService
    {
        Task<string> GetLastDate(string broker);
        Task<List<string>> GetAllDate(string broker, string type);
        Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type);
        Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type, string month);
    }
}
