using FarzamTEWebsite.Data;
using FarzamTEWebsite.Migrations;
using FarzamTEWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public Task<IActionResult> Customers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var CustomersCount = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.CallTo)
                .Count();
            return Task.FromResult<IActionResult>(Ok(CustomersCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> UniqueCustomers(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var UniqueCustomers = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .Select(hc => hc.CallTo)
                .Distinct()
                .ToList();
            if (UniqueCustomers == null)
                return Task.FromResult<IActionResult>(NotFound());
            return Task.FromResult<IActionResult>(Ok(UniqueCustomers));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> Active_Customers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ActiveCustomersCount = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .Select(hc => hc.CallTo)
                .Distinct()
                .Count();
            return Task.FromResult<IActionResult>(Ok(ActiveCustomersCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> Inactive_Customers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var InactiveCustomersCount = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .Select(hc => hc.CallTo)
                .Distinct()
                .Count();
            return Task.FromResult<IActionResult>(Ok(InactiveCustomersCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> AllCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var AllCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker);
            return Task.FromResult<IActionResult>(Ok(AllCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> SuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var SuccessfulCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Made");
            return Task.FromResult<IActionResult>(Ok(SuccessfulCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ActiveAfterCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ActiveAfterCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive" && hc.TradeStatusAffter == "Active");
            return Task.FromResult<IActionResult>(Ok(ActiveAfterCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ActiveInOtherBrockers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ActiveInOtherBrockersCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && (hc.CustomerRequirement == "فعال در کارگزاری دیگر" || hc.CustomerRequirement1 == "فعال در کارگزاری دیگر" || hc.CustomerRequirement2 == "فعال در کارگزاری دیگر"));
            return Task.FromResult<IActionResult>(Ok(ActiveInOtherBrockersCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ExplanationClub_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ExplanationClubCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.ExplanationClub == true);
            return Task.FromResult<IActionResult>(Ok(ExplanationClubCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ActiveSuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ActiveSuccessfulCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active" && hc.statusReason == "Made" );
            return Task.FromResult<IActionResult>(Ok(ActiveSuccessfulCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> InactiveSuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var InactiveSuccessfulCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive" && hc.statusReason == "Made");
            return Task.FromResult<IActionResult>(Ok(InactiveSuccessfulCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> DisinclinationCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var DisinclinationCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "disinclination");
            return Task.FromResult<IActionResult>(Ok(DisinclinationCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ReCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ReCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "ReCall");
            return Task.FromResult<IActionResult>(Ok(ReCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> LackInfoCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LackInfoCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Lack of information");
            return Task.FromResult<IActionResult>(Ok(LackInfoCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> RepeatCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var RepeatCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Repeat Call");
            return Task.FromResult<IActionResult>(Ok(RepeatCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> UnResponsiveCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var UnResponsiveCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "UnResponsive");
            return Task.FromResult<IActionResult>(Ok(UnResponsiveCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> OffCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var OffCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Off");
            return Task.FromResult<IActionResult>(Ok(OffCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> RejectCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var RejectCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Reject");
            return Task.FromResult<IActionResult>(Ok(RejectCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> UnavailableCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var UnavailableCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Unavailable");
            return Task.FromResult<IActionResult>(Ok(UnavailableCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> BusyCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var BusyCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Busy");
            return Task.FromResult<IActionResult>(Ok(BusyCallsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> UserRequests_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var UserRequestsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.UserRequest != null);
            return Task.FromResult<IActionResult>(Ok(UserRequestsCount));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> Total_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var happyCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Task.FromResult<IActionResult>(Ok(happyCallsByDay));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> SuccessfulCalls_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var SuccessfulCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
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
            return Task.FromResult<IActionResult>(Ok(SuccessfulCallsByDay));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> UnsuccessfulCalls_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var UnsuccessfulCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
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
            return Task.FromResult<IActionResult>(Ok(UnsuccessfulCallsByDay));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> All(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var AllhappyCallsByDay = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .OrderBy(hc => hc.createdon)
                .Select(hc => new
                {
                    hc.Id,
                    hc.CallTo,
                    hc.CallFrom,
                    hc.statusReason,
                    hc.nationalCode,
                    hc.Broker,
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
            return Task.FromResult<IActionResult>(Ok(AllhappyCallsByDay));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ActiveIntroduction(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ActiveIntroduction = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Introduction = group.Any(hc => hc.introduction != null) ? group.First(hc => hc.introduction != null).introduction : "نامشخص" })
                .GroupBy(hc => hc.Introduction)
                .Select(group => new { Introduction = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (ActiveIntroduction == null || !ActiveIntroduction.Any())
                return Task.FromResult<IActionResult>(NotFound());
            return Task.FromResult<IActionResult>(Ok(ActiveIntroduction));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> InactiveIntroduction(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var InactiveIntroduction = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Introduction = group.Any(hc => hc.introduction != null) ? group.First(hc => hc.introduction != null).introduction : "نامشخص" })
                .GroupBy(hc => hc.Introduction)
                .Select(group => new { Introduction = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (InactiveIntroduction == null || !InactiveIntroduction.Any())
                return Task.FromResult<IActionResult>(NotFound());
            return Task.FromResult<IActionResult>(Ok(InactiveIntroduction));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> ActiveChoosingBrokerage(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var ActiveChoosingBrokerage = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, ChoosingBrokerage = group.Any(hc => hc.ChoosingBrokerage != null) ? group.First(hc => hc.ChoosingBrokerage != null).ChoosingBrokerage : "نامشخص" })
                .GroupBy(hc => hc.ChoosingBrokerage)
                .Select(group => new { ChoosingBrokerage = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (ActiveChoosingBrokerage == null || !ActiveChoosingBrokerage.Any())
                return Task.FromResult<IActionResult>(NotFound());
            return Task.FromResult<IActionResult>(Ok(ActiveChoosingBrokerage));
        }

        [Authorize]
        [HttpGet]
        public Task<IActionResult> InactiveChoosingBrokerage(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var InactiveChoosingBrokerage = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, ChoosingBrokerage = group.Any(hc => hc.ChoosingBrokerage != null) ? group.First(hc => hc.ChoosingBrokerage != null).ChoosingBrokerage : "نامشخص" })
                .GroupBy(hc => hc.ChoosingBrokerage)
                .Select(group => new { ChoosingBrokerage = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (InactiveChoosingBrokerage == null || !InactiveChoosingBrokerage.Any())
                return Task.FromResult<IActionResult>(NotFound());
            return Task.FromResult<IActionResult>(Ok(InactiveChoosingBrokerage));
        }
    }
}
