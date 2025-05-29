using FarzamTEWebsite.Data;
using FarzamTEWebsite.DTOs;
using FarzamTEWebsite.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FarzamTEWebsite.Services
{
    public class BrokerageReportsService : IBrokerageReports
    {
        private readonly FarzamDbContext _context;

        public BrokerageReportsService(FarzamDbContext context)
        {
            _context = context;
        }

        public async Task<BrokerageMoshtaghe_DTO> GetBrokerageMoshtaghe(int userId, string dateMonthly)
        {
            var u = await _context.Users.FindAsync(userId);

            var data = await _context.Transaction_Statistics_M
            .AsNoTracking()
            .Where(t => t.Date_Monthly == dateMonthly)
            .Select(t => new
            {
                t.Brokerage_ID,
                t.BOBT_Moshtaghe_Normal,
                t.BOBT_Moshtaghe_Online,
                t.FI_Moshtaghe_Station,
                t.FI_Moshtaghe_Normal,
                t.FI_Moshtaghe_Group,
                t.FI_Moshtaghe_Other,
                t.TotalMoshtaghe
            })
            .ToListAsync();

            var dto = new BrokerageMoshtaghe_DTO { Date = dateMonthly };
            var dynamicData = data.Cast<dynamic>().ToList();

            FillMetric(dto, dynamicData, "TotalMoshtaghe", t => t.TotalMoshtaghe, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Moshtaghe_Normal", t => t.BOBT_Moshtaghe_Normal, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Moshtaghe_Online", t => t.BOBT_Moshtaghe_Online, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Moshtaghe_Station", t => t.FI_Moshtaghe_Station, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Moshtaghe_Normal", t => t.FI_Moshtaghe_Normal, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Moshtaghe_Group", t => t.FI_Moshtaghe_Group, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Moshtaghe_Other", t => t.FI_Moshtaghe_Other, u.Brokerage_ID);

            var total = (long)(dto.TotalMoshtaghe == 0 ? 1 : dto.TotalMoshtaghe);
            dto.Percent_Moshtaghe = dto.TotalMoshtaghe == 0 ? 0 :
                (int)Math.Round((double)dto.Brokerage_TotalMoshtaghe / total * 100);

            return dto;
        }

        public async Task<BrokerageOnline_DTO> GetBrokerageOnline(int userId, string dateMonthly)
        {
            var u = await _context.Users.FindAsync(userId);

            var data = await _context.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == dateMonthly)
                .Select(t => new
                {
                    t.Brokerage_ID,
                    t.BOBT_Oragh_Bedehi_Online,
                    t.BOBT_Moshtaghe_Online,
                    t.BOBT_Sarmaye_Herfei_Online,
                    t.BOBT_Sarmaye_Herfei_Algorithm,
                    t.BOBT_saham_Online,
                    t.BOBT_saham_Algorithm,
                    t.BOBT_Sandogh_Online,
                    t.BOBT_Sandogh_Algorithm,
                    t.FI_Online_Normal,
                    t.FI_Online_Group,
                    t.FI_Online_Other,
                    t.TotalOnline
                })
                .ToListAsync();

            var dto = new BrokerageOnline_DTO { Date = dateMonthly };
            var dynamicData = data.Cast<dynamic>().ToList();

            FillMetric(dto, dynamicData, "TotalOnline", t => t.TotalOnline, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Oragh_Bedehi_Online", t => t.BOBT_Oragh_Bedehi_Online, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Moshtaghe_Online", t => t.BOBT_Moshtaghe_Online, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Sarmaye_Herfei_Online", t => t.BOBT_Sarmaye_Herfei_Online, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Sarmaye_Herfei_Algorithm", t => t.BOBT_Sarmaye_Herfei_Algorithm, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_saham_Online", t => t.BOBT_saham_Online, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_saham_Algorithm", t => t.BOBT_saham_Algorithm, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Sandogh_Online", t => t.BOBT_Sandogh_Online, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "BOBT_Sandogh_Algorithm", t => t.BOBT_Sandogh_Algorithm, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Online_Normal", t => t.FI_Online_Normal, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Online_Group", t => t.FI_Online_Group, u.Brokerage_ID);
            FillMetric(dto, dynamicData, "FI_Online_Other", t => t.FI_Online_Other, u.Brokerage_ID);

            var total = (long)(dto.TotalOnline == 0 ? 1 : dto.TotalOnline);
            dto.Percent_Online = dto.TotalOnline == 0 ? 0 :
                (int)Math.Round((double)dto.Brokerage_TotalOnline / total * 100);

            return dto;
        }

        void FillMetric<TDto>(TDto dto, List<dynamic> data, string propName, Func<dynamic, long> selector, int brokerageId)
        {
            var sorted = data.OrderByDescending(selector).ToList();
            var selected = sorted.Select((t, i) => new { t, Rank = i + 1 })
                .FirstOrDefault(x => x.t.Brokerage_ID == brokerageId);
            long total = data.Sum(t => (long)(selector(t) ?? 0));
            int value = selected != null ? (int)(selector(selected.t) ?? 0) : 0;
            int rank = total == 0 ? 0 : selected?.Rank ?? 0;

            var type = dto.GetType();
            type.GetProperty($"{propName}")?.SetValue(dto, total);
            type.GetProperty($"Brokerage_{propName}")?.SetValue(dto, value);
            type.GetProperty($"Brokerage_Rank_{propName}")?.SetValue(dto, rank);
        }
    }
}
