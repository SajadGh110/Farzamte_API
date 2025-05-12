using FarzamTEWebsite.Data;
using FarzamTEWebsite.DTOs;
using FarzamTEWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FarzamTEWebsite.Services
{
    public class InComingCall_StatService : IInComingCall_StatService
    {
        private FarzamDbContext _dbContext;
        public InComingCall_StatService(FarzamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetLastDate(string broker)
        {
            var LastDate = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where(inc => inc.Broker == broker)
                .MaxAsync(inc => inc.Month);

            return LastDate;
        }

        public async Task<List<string>> GetAllDate(string broker, string type)
        {
            var AllDate = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where (inc => inc.Broker == broker && inc.type == type)
                .Select(inc => inc.Month)
                .ToListAsync();
            return AllDate;
        }

        public async Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type)
        {
            var IncStat = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where(inc => inc.Broker == broker && inc.type == type)
                .Select(inc => new InComingCall_Stat_DTO
                {
                    Type = inc.type,
                    Broker = broker,
                    Month = inc.Month,
                    Answered = inc.Answered,
                    Avg_Wait = inc.Avg_Wait,
                    Avg_Talk = inc.Avg_Talk
                })
                .ToListAsync();
            return IncStat;
        }

        public async Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type, string month)
        {
            var IncStat = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where(inc => inc.Broker == broker && inc.Month == month && inc.type == type)
                .Select(inc => new InComingCall_Stat_DTO
                {
                    Type = inc.type,
                    Broker = inc.Broker,
                    Month = inc.Month,
                    Answered = inc.Answered,
                    Avg_Wait = inc.Avg_Wait,
                    Avg_Talk = inc.Avg_Talk
                })
                .ToListAsync();
            return IncStat;
        }
    }
}
