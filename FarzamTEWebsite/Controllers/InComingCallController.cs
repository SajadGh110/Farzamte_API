using FarzamTEWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class InComingCallController : ControllerBase
    {
        private IConfiguration _configuration;
        private FarzamDbContext _dbContext;
        public InComingCallController(FarzamDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.Broker == Broker)
                .Max(inc => inc.createdon)
                .ToString("yyyy-MM-dd");
            return Ok(new { EndDate = LastDate });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Total_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var happyCallsByDay = _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker)
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
        public async Task<IActionResult> Phonecall_Reasons(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker)
                .ToListAsync();

            var reasons = phoneCallReasons
                .SelectMany(inc => new[] { inc.phonecallreason, inc.phonecallreason2, inc.phonecallreason3 })
                .Where(reason => !string.IsNullOrEmpty(reason) && reason != "NULL")
                .GroupBy(reason => reason)
                .Select(group => new { Reason = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .ToList();

            return Ok(reasons);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Reason_Detail(DateTime startDate, DateTime endDate, string Reason)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReasonDetail1 = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker && inc.phonecallreason == Reason)
                .GroupBy(inc => inc.phonecallreasondetail)
                .Select(group => new
                {
                    ReasonDetail = string.IsNullOrEmpty(group.Key) || group.Key.Equals("NULL", StringComparison.OrdinalIgnoreCase) ? "نامشخص" : group.Key,
                    Count = group.Count()
                }).ToListAsync();

            var ReasonDetail2 = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker && inc.phonecallreason2 == Reason)
                .GroupBy(inc => inc.phonecallreasondetail2)
                .Select(group => new
                {
                    ReasonDetail = string.IsNullOrEmpty(group.Key) || group.Key.Equals("NULL", StringComparison.OrdinalIgnoreCase) ? "نامشخص" : group.Key,
                    Count = group.Count()
                }).ToListAsync();

            var ReasonDetail3 = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker && inc.phonecallreason3 == Reason)
                .GroupBy(inc => inc.phonecallreasondetail3)
                .Select(group => new
                {
                    ReasonDetail = string.IsNullOrEmpty(group.Key) || group.Key.Equals("NULL", StringComparison.OrdinalIgnoreCase) ? "نامشخص" : group.Key,
                    Count = group.Count()
                }).ToListAsync();

            var CombinedResult = ReasonDetail1
            .Concat(ReasonDetail2)
            .Concat(ReasonDetail3)
            .GroupBy(x => x.ReasonDetail)
            .Select(g => new
            {
                ReasonDetail = g.Key,
                Count = g.Sum(x => x.Count)
            })
            .OrderByDescending(inc => inc.Count)
            .ToList();

            return Ok(CombinedResult);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> description(DateTime startDate, DateTime endDate, string Detail)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var selected_fields = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker && inc.description != "NULL" && inc.description != null && ( inc.phonecallreasondetail == Detail || inc.phonecallreasondetail2 == Detail || inc.phonecallreasondetail3 == Detail))
                .Select(inc => new { inc.fullName, inc.phonenumber, inc.description})
                .ToListAsync();
            return Ok(selected_fields);
        }
    }
}
