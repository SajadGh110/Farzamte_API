using FarzamTEWebsite.Data;
using FarzamTEWebsite.Models;
using FarzamTEWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class BrokerageController : ControllerBase
    {
        private readonly IBrokerageReports brokerageReports;
        private FarzamDbContext _dbContext;
        public BrokerageController(FarzamDbContext dbContext, IBrokerageReports brokerageReports)
        {
            _dbContext = dbContext;
            this.brokerageReports = brokerageReports;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AllDate()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var AllDate = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Brokerage_ID == u.Brokerage_ID)
                .Select(t => t.Date_Monthly)
                .ToListAsync();
            return Ok(AllDate);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get_Brokerage_Name()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var Name = _dbContext.Brokerages
                .AsNoTracking()
                .Where(t => t.Id == u.Brokerage_ID)
                .Select(t => new { t.Name, t.Logo });
            return Ok(Name);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Totals(string Date_Monthly)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var totals = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly)
                .GroupBy(t => 1)
                .Select(g => new
                {
                    BOBT_Sum = (long)(g.Sum(t => (long?)t.BOBT_Total_Value) ?? 0),
                    FI_Sum = (long)(g.Sum(t => (long?)t.FI_Total_Value) ?? 0),
                    BKI_Sum = (long)(g.Sum(t => (long?)t.BKI_Total_Value) ?? 0),
                    BEI_Sum = (long)(g.Sum(t => (long?)t.BEI_Total_Value) ?? 0),
                    All_Sum = (long)(g.Sum(t => (long?)t.All_Total_Value) ?? 0)
                })
                .FirstOrDefaultAsync();

            var brokerage = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly && t.Brokerage_ID == u.Brokerage_ID)
                .Select(t => new
                {
                    BOBT = t.BOBT_Total_Value,
                    FI = t.FI_Total_Value,
                    BKI = t.BKI_Total_Value,
                    BEI = t.BEI_Total_Value,
                    All = t.All_Total_Value
                })
                .FirstOrDefaultAsync();

            float bobt_share = (float)(brokerage?.BOBT) / (float)(totals?.BOBT_Sum) * 100;
            float fi_share = (float)(brokerage?.FI) / (float)(totals?.FI_Sum) * 100;
            float bki_share = (float)(brokerage?.BKI) / (float)(totals?.BKI_Sum) * 100;
            float bei_share = (float)(brokerage?.BEI) / (float)(totals?.BEI_Sum) * 100;
            float all_share = (float)(brokerage?.All) / (float)(totals?.All_Sum) * 100;

            return Ok(new
            {
                date = Date_Monthly,
                bobt_share = (float)Math.Round(bobt_share, 2),
                fi_share = (float)Math.Round(fi_share, 2),
                bobt_fi_share = (float)Math.Round(bobt_share + fi_share, 2),
                bki_share = (float)Math.Round(bki_share, 2),
                bei_share = (float)Math.Round(bei_share, 2),
                all_share = (float)Math.Round(all_share, 2)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Chart1(string Date_Monthly)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var AllTransactions = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly)
                .Select(t => new
                {
                    Brokerage_ID = t.Brokerage_ID,
                    BOBT_Total_Value = t.BOBT_Total_Value,
                    FI_Total_Value = t.FI_Total_Value,
                    BOBT_AND_FI_Total_Value = t.BOBT_AND_FI_Total_Value,
                    BKI_Total_Value = t.BKI_Total_Value,
                    BEI_Total_Value = t.BEI_Total_Value,
                    All_Total_Value = t.All_Total_Value
                })
                .ToListAsync();

            // BOBT_Brokerage_Value
            var BOBT_Brokerage_Value = AllTransactions
                .OrderByDescending(t => t.BOBT_Total_Value)
                .ToList();
            var BOBT_Ranked_Brokerage_Value = BOBT_Brokerage_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);
            // FI_Brokerage_Value
            var FI_Brokerage_Value = AllTransactions
                .OrderByDescending(t => t.FI_Total_Value)
                .ToList();
            var FI_Ranked_Brokerage_Value = FI_Brokerage_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);
            // BOBT_AND_FI_Brokerage_Value
            var BOBT_AND_FI_Brokerage_Value = AllTransactions
                .OrderByDescending(t => t.BOBT_AND_FI_Total_Value)
                .ToList();
            var BOBT_AND_FI_Ranked_Brokerage_Value = BOBT_AND_FI_Brokerage_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);
            // BKI_Brokerage_Value
            var BKI_Brokerage_Value = AllTransactions
                .OrderByDescending(t => t.BKI_Total_Value)
                .ToList();
            var BKI_Ranked_Brokerage_Value = BKI_Brokerage_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);
            // BEI_Brokerage_Value
            var BEI_Brokerage_Value = AllTransactions
                .OrderByDescending(t => t.BEI_Total_Value)
                .ToList();
            var BEI_Ranked_Brokerage_Value = BEI_Brokerage_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);
            // All_Brokerage_Value
            var All_Brokerage_Value = AllTransactions
                .OrderByDescending(t => t.All_Total_Value)
                .ToList();
            var All_Ranked_Brokerage_Value = All_Brokerage_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            return Ok(new
            {
                BOBT_Brokerage_Value = BOBT_Ranked_Brokerage_Value?.Transaction.BOBT_Total_Value ?? 0,
                BOBT_Total_Value = AllTransactions.Sum(t => (long?)t.BOBT_Total_Value),
                BOBT_Brokerage_Rank = BOBT_Ranked_Brokerage_Value?.Rank ?? 0,
                FI_Brokerage_Value = FI_Ranked_Brokerage_Value?.Transaction.FI_Total_Value ?? 0,
                FI_Total_Value = AllTransactions.Sum(t => (long?)t.FI_Total_Value),
                FI_Brokerage_Rank = FI_Ranked_Brokerage_Value?.Rank ?? 0,
                BOBT_AND_FI_Brokerage_Value = BOBT_AND_FI_Ranked_Brokerage_Value?.Transaction.BOBT_AND_FI_Total_Value ?? 0,
                BOBT_AND_FI_Total_Value = AllTransactions.Sum(t => (long?)t.BOBT_AND_FI_Total_Value),
                BOBT_AND_FI_Brokerage_Rank = BOBT_AND_FI_Ranked_Brokerage_Value?.Rank ?? 0,
                BKI_Brokerage_Value = BKI_Ranked_Brokerage_Value?.Transaction.BKI_Total_Value ?? 0,
                BKI_Total_Value = AllTransactions.Sum(t => (long?)t.BKI_Total_Value),
                BKI_Brokerage_Rank = BKI_Ranked_Brokerage_Value?.Rank ?? 0,
                BEI_Brokerage_Value = BEI_Ranked_Brokerage_Value?.Transaction.BEI_Total_Value ?? 0,
                BEI_Total_Value = AllTransactions.Sum(t => (long?)t.BEI_Total_Value),
                BEI_Brokerage_Rank = BEI_Ranked_Brokerage_Value?.Rank ?? 0,
                All_Brokerage_Value = All_Ranked_Brokerage_Value?.Transaction.All_Total_Value ?? 0,
                All_Total_Value = AllTransactions.Sum(t => (long?)t.All_Total_Value),
                All_Brokerage_Rank = All_Ranked_Brokerage_Value?.Rank ?? 0
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Chart2(string Date_Monthly)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var AllTransactions = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly)
                .Select(t => new
                {
                    Brokerage_ID = t.Brokerage_ID,
                    BOBT_Oragh_Bedehi = t.BOBT_Oragh_Bedehi_Online + t.BOBT_Oragh_Bedehi_Normal,
                    BOBT_Moshtaghe = t.BOBT_Moshtaghe_Online + t.BOBT_Moshtaghe_Normal,
                    BOBT_Sarmaye_Herfei = t.BOBT_Sarmaye_Herfei_Online + t.BOBT_Sarmaye_Herfei_Normal + t.BOBT_Sarmaye_Herfei_Algorithm,
                    BOBT_saham = t.BOBT_saham_Online + t.BOBT_saham_Normal + t.BOBT_saham_Algorithm,
                    BOBT_Total_Value = t.BOBT_Total_Value
                })
                .ToListAsync();

            // BOBT_Oragh_Bedehi
            var Ranked_BOBT_Oragh_Bedehi = AllTransactions
                .OrderByDescending(t => t.BOBT_Oragh_Bedehi)
                .ToList();
            var Brokerage_Rank_BOBT_Oragh_Bedehi = Ranked_BOBT_Oragh_Bedehi
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BOBT_Moshtaghe
            var Ranked_BOBT_Moshtaghe = AllTransactions
                .OrderByDescending(t => t.BOBT_Moshtaghe)
                .ToList();
            var Brokerage_Rank_BOBT_Moshtaghe = Ranked_BOBT_Moshtaghe
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BOBT_Sarmaye_Herfei
            var Ranked_BOBT_Sarmaye_Herfei = AllTransactions
                .OrderByDescending(t => t.BOBT_Sarmaye_Herfei)
                .ToList();
            var Brokerage_Rank_BOBT_Sarmaye_Herfei = Ranked_BOBT_Sarmaye_Herfei
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BOBT_saham
            var Ranked_BOBT_saham = AllTransactions
                .OrderByDescending(t => t.BOBT_saham)
                .ToList();
            var Brokerage_Rank_BOBT_saham = Ranked_BOBT_saham
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BOBT_Total_Value
            var Ranked_BOBT_Total_Value = AllTransactions
                .OrderByDescending(t => t.BOBT_Total_Value)
                .ToList();
            var Brokerage_Rank_BOBT_Total_Value = Ranked_BOBT_Total_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            return Ok(new
            {
                BOBT_Oragh_Bedehi = Brokerage_Rank_BOBT_Oragh_Bedehi?.Transaction.BOBT_Oragh_Bedehi ?? 0,
                BOBT_Oragh_Bedehi_Total = AllTransactions.Sum(t => (long?)t.BOBT_Oragh_Bedehi),
                BOBT_Oragh_Bedehi_Rank = Brokerage_Rank_BOBT_Oragh_Bedehi?.Rank ?? 0,
                BOBT_Moshtaghe = Brokerage_Rank_BOBT_Moshtaghe?.Transaction.BOBT_Moshtaghe ?? 0,
                BOBT_Moshtaghe_Total = AllTransactions.Sum(t => (long?)t.BOBT_Moshtaghe),
                BOBT_Moshtaghe_Rank = Brokerage_Rank_BOBT_Moshtaghe?.Rank ?? 0,
                BOBT_Sarmaye_Herfei = Brokerage_Rank_BOBT_Sarmaye_Herfei?.Transaction.BOBT_Sarmaye_Herfei ?? 0,
                BOBT_Sarmaye_Herfei_Total = AllTransactions.Sum(t => (long?)t.BOBT_Sarmaye_Herfei),
                BOBT_Sarmaye_Herfei_Rank = Brokerage_Rank_BOBT_Sarmaye_Herfei?.Rank ?? 0,
                BOBT_saham = Brokerage_Rank_BOBT_saham?.Transaction.BOBT_saham ?? 0,
                BOBT_saham_Total = AllTransactions.Sum(t => (long?)t.BOBT_saham),
                BOBT_saham_Rank = Brokerage_Rank_BOBT_saham?.Rank ?? 0,
                BOBT_Brokerage_Value = Brokerage_Rank_BOBT_Total_Value?.Transaction.BOBT_Total_Value ?? 0,
                BOBT_Total_Value = AllTransactions.Sum(t => (long?)t.BOBT_Total_Value),
                BOBT_Brokerage_Rank = Brokerage_Rank_BOBT_Total_Value?.Rank ?? 0
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Chart3(string Date_Monthly)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var AllTransactions = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly)
                .Select(t => new
                {
                    Brokerage_ID = t.Brokerage_ID,
                    FI_Brokerage_Station = t.FI_Brokerage_Station,
                    FI_Online_Normal = t.FI_Online_Normal,
                    FI_Online_Group = t.FI_Online_Group,
                    FI_Online_Other = t.FI_Online_Other,
                    FI_Total_Value = t.FI_Total_Value
                })
                .ToListAsync();

            // FI_Brokerage_Station
            var Ranked_FI_Brokerage_Station = AllTransactions
                .OrderByDescending(t => t.FI_Brokerage_Station)
                .ToList();
            var Brokerage_Rank_FI_Brokerage_Station = Ranked_FI_Brokerage_Station
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // FI_Online_Normal
            var Ranked_FI_Online_Normal = AllTransactions
                .OrderByDescending(t => t.FI_Online_Normal)
                .ToList();
            var Brokerage_Rank_FI_Online_Normal = Ranked_FI_Online_Normal
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // FI_Online_Group
            var Ranked_FI_Online_Group = AllTransactions
                .OrderByDescending(t => t.FI_Online_Group)
                .ToList();
            var Brokerage_Rank_FI_Online_Group = Ranked_FI_Online_Group
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // FI_Online_Other
            var Ranked_FI_Online_Other = AllTransactions
                .OrderByDescending(t => t.FI_Online_Other)
                .ToList();
            var Brokerage_Rank_FI_Online_Other = Ranked_FI_Online_Other
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // FI_Total_Value
            var Ranked_FI_Total_Value = AllTransactions
                .OrderByDescending(t => t.FI_Total_Value)
                .ToList();
            var Brokerage_Rank_FI_Total_Value = Ranked_FI_Total_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            return Ok(new
            {
                FI_Brokerage_Station = Brokerage_Rank_FI_Brokerage_Station?.Transaction.FI_Brokerage_Station ?? 0,
                FI_Brokerage_Station_Total = AllTransactions.Sum(t => (long?)t.FI_Brokerage_Station),
                FI_Brokerage_Station_Rank = Brokerage_Rank_FI_Brokerage_Station?.Rank ?? 0,
                FI_Online_Normal = Brokerage_Rank_FI_Online_Normal?.Transaction.FI_Online_Normal ?? 0,
                FI_Online_Normal_Total = AllTransactions.Sum(t => (long?)t.FI_Online_Normal),
                FI_Online_Normal_Rank = Brokerage_Rank_FI_Online_Normal?.Rank ?? 0,
                FI_Online_Group = Brokerage_Rank_FI_Online_Group?.Transaction.FI_Online_Group ?? 0,
                FI_Online_Group_Total = AllTransactions.Sum(t => (long?)t.FI_Online_Group),
                FI_Online_Group_Rank = Brokerage_Rank_FI_Online_Group?.Rank ?? 0,
                FI_Online_Other = Brokerage_Rank_FI_Online_Other?.Transaction.FI_Online_Other ?? 0,
                FI_Online_Other_Total = AllTransactions.Sum(t => (long?)t.FI_Online_Other),
                FI_Online_Other_Rank = Brokerage_Rank_FI_Online_Other?.Rank ?? 0,
                FI_Brokerage_Value = Brokerage_Rank_FI_Total_Value?.Transaction.FI_Total_Value ?? 0,
                FI_Total_Value = AllTransactions.Sum(t => (long?)t.FI_Total_Value),
                FI_Brokerage_Value_Rank = Brokerage_Rank_FI_Total_Value?.Rank ?? 0
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Chart4(string Date_Monthly)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var AllTransactions = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly)
                .Select(t => new
                {
                    Brokerage_ID = t.Brokerage_ID,
                    BKI_Physical = t.BKI_Physical,
                    BKI_Self = t.BKI_Self,
                    BKI_Ati = t.BKI_Ati,
                    BKI_Ekhtiar = t.BKI_Ekhtiar,
                    BKI_Total_Value = t.BKI_Total_Value
                })
                .ToListAsync();

            // BKI_Physical
            var Ranked_BKI_Physical = AllTransactions
                .OrderByDescending(t => t.BKI_Physical)
                .ToList();
            var Brokerage_Rank_BKI_Physical = Ranked_BKI_Physical
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BKI_Self
            var Ranked_BKI_Self = AllTransactions
                .OrderByDescending(t => t.BKI_Self)
                .ToList();
            var Brokerage_Rank_BKI_Self = Ranked_BKI_Self
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BKI_Ati
            var Ranked_BKI_Ati = AllTransactions
                .OrderByDescending(t => t.BKI_Ati)
                .ToList();
            var Brokerage_Rank_BKI_Ati = Ranked_BKI_Ati
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BKI_Ekhtiar
            var Ranked_BKI_Ekhtiar = AllTransactions
                .OrderByDescending(t => t.BKI_Ekhtiar)
                .ToList();
            var Brokerage_Rank_BKI_Ekhtiar = Ranked_BKI_Ekhtiar
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BKI_Total_Value
            var Ranked_BKI_Total_Value = AllTransactions
                .OrderByDescending(t => t.BKI_Total_Value)
                .ToList();
            var Brokerage_Rank_BKI_Total_Value = Ranked_BKI_Total_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            return Ok(new
            {
                BKI_Physical = Brokerage_Rank_BKI_Physical?.Transaction.BKI_Physical ?? 0,
                BKI_Physical_Total = AllTransactions.Sum(t => (long?)t.BKI_Physical),
                BKI_Physical_Rank = Brokerage_Rank_BKI_Physical?.Rank ?? 0,
                BKI_Self = Brokerage_Rank_BKI_Self?.Transaction.BKI_Self ?? 0,
                BKI_Self_Total = AllTransactions.Sum(t => (long?)t.BKI_Self),
                BKI_Self_Rank = Brokerage_Rank_BKI_Self?.Rank ?? 0,
                BKI_Ati = Brokerage_Rank_BKI_Ati?.Transaction.BKI_Ati ?? 0,
                BKI_Ati_Total = AllTransactions.Sum(t => (long?)t.BKI_Ati),
                BKI_Ati_Rank = Brokerage_Rank_BKI_Ati?.Rank ?? 0,
                BKI_Ekhtiar = Brokerage_Rank_BKI_Ekhtiar?.Transaction.BKI_Ekhtiar ?? 0,
                BKI_Ekhtiar_Total = AllTransactions.Sum(t => (long?)t.BKI_Ekhtiar),
                BKI_Ekhtiar_Rank = Brokerage_Rank_BKI_Ekhtiar?.Rank ?? 0,
                BKI_Brokerage_Value = Brokerage_Rank_BKI_Total_Value?.Transaction.BKI_Total_Value ?? 0,
                BKI_Total_Value = AllTransactions.Sum(t => (long?)t.BKI_Total_Value),
                BKI_Brokerage_Value_Rank = Brokerage_Rank_BKI_Total_Value?.Rank ?? 0
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Chart5(string Date_Monthly)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            var AllTransactions = await _dbContext.Transaction_Statistics_M
                .AsNoTracking()
                .Where(t => t.Date_Monthly == Date_Monthly)
                .Select(t => new
                {
                    Brokerage_ID = t.Brokerage_ID,
                    BEI_Physical = t.BEI_Physical,
                    BEI_Moshtaghe = t.BEI_Moshtaghe,
                    BEI_Other = t.BEI_Other,
                    BEI_Total_Value = t.BEI_Total_Value,
                })
                .ToListAsync();

            // BEI_Physical
            var Ranked_BEI_Physical = AllTransactions
                .OrderByDescending(t => t.BEI_Physical)
                .ToList();
            var Brokerage_Rank_BEI_Physical = Ranked_BEI_Physical
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BEI_Moshtaghe
            var Ranked_BEI_Moshtaghe = AllTransactions
                .OrderByDescending(t => t.BEI_Moshtaghe)
                .ToList();
            var Brokerage_Rank_BEI_Moshtaghe = Ranked_BEI_Moshtaghe
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BEI_Other
            var Ranked_BEI_Other = AllTransactions
                .OrderByDescending(t => t.BEI_Other)
                .ToList();
            var Brokerage_Rank_BEI_Other = Ranked_BEI_Other
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            // BEI_Total_Value
            var Ranked_BEI_Total_Value = AllTransactions
                .OrderByDescending(t => t.BEI_Total_Value)
                .ToList();
            var Brokerage_Rank_BEI_Total_Value = Ranked_BEI_Total_Value
                .Select((t, index) => new { Transaction = t, Rank = index + 1 })
                .FirstOrDefault(x => x.Transaction.Brokerage_ID == u.Brokerage_ID);

            return Ok(new
            {
                BEI_Physical = Brokerage_Rank_BEI_Physical?.Transaction.BEI_Physical ?? 0,
                BEI_Physical_Total = AllTransactions.Sum(t => (long?)t.BEI_Physical),
                BEI_Physical_Rank = Brokerage_Rank_BEI_Physical?.Rank ?? 0,
                BEI_Moshtaghe = Brokerage_Rank_BEI_Moshtaghe?.Transaction.BEI_Moshtaghe ?? 0,
                BEI_Moshtaghe_Total = AllTransactions.Sum(t => (long?)t.BEI_Moshtaghe),
                BEI_Moshtaghe_Rank = Brokerage_Rank_BEI_Moshtaghe?.Rank ?? 0,
                BEI_Other = Brokerage_Rank_BEI_Other?.Transaction.BEI_Other ?? 0,
                BEI_Other_Total = AllTransactions.Sum(t => (long?)t.BEI_Other),
                BEI_Other_Rank = Brokerage_Rank_BEI_Other?.Rank ?? 0,
                BEI_Brokerage_Value = Brokerage_Rank_BEI_Total_Value?.Transaction.BEI_Total_Value ?? 0,
                BEI_Total_Value = AllTransactions.Sum(t => (long?)t.BEI_Total_Value),
                BEI_Brokerage_Value_Rank = Brokerage_Rank_BEI_Total_Value?.Rank ?? 0
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBrokerageMoshtaghe(string Date_Monthly)
        {
            if (string.IsNullOrEmpty(Date_Monthly))
                return BadRequest("Date_Monthly is required.");
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await brokerageReports.GetBrokerageMoshtaghe(userId, Date_Monthly);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBrokerageOnline(string Date_Monthly)
        {
            if (string.IsNullOrEmpty(Date_Monthly))
                return BadRequest("Date_Monthly is required.");
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await brokerageReports.GetBrokerageOnline(userId, Date_Monthly);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GenerateFakeRecords(int brokerage_id, String date)
        {
            var fakeRecords = new List<Transaction_Statistics_M>();
            var random = new Random();

            var BOBT_Oragh_Bedehi_Online = random.Next(0, 10000000);
            var BOBT_Oragh_Bedehi_Normal = random.Next(0, 10000000);
            var BOBT_Moshtaghe_Online = random.Next(0, 10000000);
            var BOBT_Moshtaghe_Normal = random.Next(0, 10000000);
            var BOBT_Sarmaye_Herfei_Online = random.Next(0, 200000);
            var BOBT_Sarmaye_Herfei_Normal = random.Next(0, 200000);
            var BOBT_Sarmaye_Herfei_Algorithm = random.Next(0, 200000);
            var BOBT_saham_Online = random.Next(500000, 200000000);
            var BOBT_saham_Normal = random.Next(0, 1000000);
            var BOBT_saham_Algorithm = random.Next(0, 200000);
            var BOBT_Total_Value = BOBT_Oragh_Bedehi_Online + BOBT_Oragh_Bedehi_Normal + BOBT_Moshtaghe_Online + BOBT_Moshtaghe_Normal + BOBT_Sarmaye_Herfei_Online + BOBT_Sarmaye_Herfei_Normal + BOBT_Sarmaye_Herfei_Algorithm + BOBT_saham_Online + BOBT_saham_Normal + BOBT_saham_Algorithm;
            var FI_Brokerage_Station = random.Next(200000, 200000000);
            var FI_Online_Normal = random.Next(200000, 200000000);
            var FI_Online_Group = random.Next(5000, 20000000);
            var FI_Online_Other = random.Next(5000, 20000000);
            var FI_Total_Value = FI_Brokerage_Station + FI_Online_Normal + FI_Online_Group + FI_Online_Other;
            var BOBT_AND_FI_Total_Value = BOBT_Total_Value + FI_Total_Value;
            var BKI_Physical = random.Next(100000, 200000000);
            var BKI_Self = random.Next(100000, 400000000);
            var BKI_Ati = random.Next(10000, 5000000);
            var BKI_Ekhtiar = random.Next(0, 1000000);
            var BKI_Total_Value = BKI_Physical + BKI_Self + BKI_Ati + BKI_Ekhtiar;
            var BEI_Physical = random.Next(0, 50000000);
            var BEI_Moshtaghe = random.Next(0, 7000000);
            var BEI_Other = random.Next(0, 50000000);
            var BEI_Total_Value = BEI_Physical + BEI_Moshtaghe + BEI_Other;
            var All_Total_Value = BOBT_AND_FI_Total_Value + BKI_Total_Value + BEI_Total_Value;

            var fakeRecord = new Transaction_Statistics_M
            {
                Brokerage_ID = brokerage_id,
                Date_Monthly = date,
                BOBT_Oragh_Bedehi_Online = BOBT_Oragh_Bedehi_Online,
                BOBT_Oragh_Bedehi_Normal = BOBT_Oragh_Bedehi_Normal,
                BOBT_Moshtaghe_Online = BOBT_Moshtaghe_Online,
                BOBT_Moshtaghe_Normal = BOBT_Moshtaghe_Normal,
                BOBT_Sarmaye_Herfei_Online = BOBT_Sarmaye_Herfei_Online,
                BOBT_Sarmaye_Herfei_Normal = BOBT_Sarmaye_Herfei_Normal,
                BOBT_Sarmaye_Herfei_Algorithm = BOBT_Sarmaye_Herfei_Algorithm,
                BOBT_saham_Online = BOBT_saham_Online,
                BOBT_saham_Normal = BOBT_saham_Normal,
                BOBT_saham_Algorithm = BOBT_saham_Algorithm,
                BOBT_Total_Value = BOBT_Total_Value,
                FI_Brokerage_Station = FI_Brokerage_Station,
                FI_Online_Normal = FI_Online_Normal,
                FI_Online_Group = FI_Online_Group,
                FI_Online_Other = FI_Online_Other,
                FI_Total_Value = FI_Total_Value,
                BOBT_AND_FI_Total_Value = BOBT_AND_FI_Total_Value,
                BKI_Physical = BKI_Physical,
                BKI_Self = BKI_Self,
                BKI_Ati = BKI_Ati,
                BKI_Ekhtiar = BKI_Ekhtiar,
                BKI_Total_Value = BKI_Total_Value,
                BEI_Physical = BEI_Physical,
                BEI_Moshtaghe = BEI_Moshtaghe,
                BEI_Other = BEI_Other,
                BEI_Total_Value = BEI_Total_Value,
                All_Total_Value = All_Total_Value
            };
            fakeRecords.Add(fakeRecord);
            
            _dbContext.Transaction_Statistics_M.AddRange(fakeRecords);
            _dbContext.SaveChanges();
            return Ok($"{fakeRecords.Count} fake records have been generated and saved.");
        }
    }
}