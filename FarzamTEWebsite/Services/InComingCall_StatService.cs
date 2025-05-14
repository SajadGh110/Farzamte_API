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

        public async Task<List<string>> GetAllTypes(string broker)
        {
            var AllTypes = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where(inc => inc.Broker == broker)
                .Select(inc => inc.type)
                .Distinct()
                .ToListAsync();
            return AllTypes;
        }

        public async Task<List<string>> GetAllDates(string broker, string type)
        {
            var AllDates = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where (inc => inc.Broker == broker && inc.type == type)
                .Select(inc => inc.Month)
                .ToListAsync();
            return AllDates;
        }

        public async Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type)
        {
            var IncStats = await _dbContext.inComingCall_Stats
                .AsNoTracking()
                .Where(inc => inc.Broker == broker && inc.type == type)
                .OrderByDescending(inc => inc.Month)
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
            return IncStats;
        }

        public async Task<List<InComingCall_Stat_DTO>> GetIncStat(string broker, string type, string month)
        {
            var IncStats = await _dbContext.inComingCall_Stats
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
            return IncStats;
        }
    }
}
