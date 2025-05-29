using FarzamTEWebsite.DTOs;

namespace FarzamTEWebsite.Services.Interfaces
{
    public interface IBrokerageReports
    {
        Task<BrokerageMoshtaghe_DTO> GetBrokerageMoshtaghe(int userId, string dateMonthly);
        Task<BrokerageOnline_DTO> GetBrokerageOnline(int userId, string dateMonthly);
    }
}
