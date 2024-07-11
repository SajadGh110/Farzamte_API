using FarzamTEWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HappyCallController : ControllerBase
    {
        private IConfiguration _configuration;
        private FarzamDbContext _dbContext;

        public HappyCallController(FarzamDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Total_Count(DateTime startDate, DateTime endDate)
        {
            var happyCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(happyCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public IActionResult SuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            var SuccessfulCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(h => h.statusReason == "Made") == 0 ? 0 : g.Count(h => h.statusReason == "Made")
                })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(SuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UnsuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            var UnsuccessfulCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(h => h.statusReason != "Made") == 0 ? 0 : g.Count(h => h.statusReason != "Made")
                })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(UnsuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public IActionResult All(DateTime startDate, DateTime endDate)
        {
            var AllhappyCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate)
                .OrderBy(hc => hc.createdon)
                .Select(hc => new
                {
                    hc.Id,
                    hc.CallTo,
                    hc.CallFrom,
                    hc.statusReason,
                    hc.nationalCode,
                    hc.phonenumber,
                    hc.introduction,
                    hc.ChoosingBrokerage,
                    hc.ExplanationClub,
                    hc.UserRequest,
                    hc.checkingPanel,
                    hc.CustomerRequirement,
                    hc.CustomerRequirementDesc,
                    hc.CustomerRequirement1,
                    hc.CustomerRequirementDesc1,
                    hc.CustomerRequirement2,
                    hc.CustomerRequirementDesc2,
                    hc.TradeStatus,
                    hc.TradeStatusAffter,
                    hc.TotalTradeAmount,
                    hc.totalBrokerCommission,
                    RegisterDate = hc.RegDate.ToString("yyyy-MM-dd"),
                    CreateDate = hc.createdon.ToString("yyyy-MM-dd")
                })
                .ToList();
            return Ok(AllhappyCallsByDay);
        }
    }
}
