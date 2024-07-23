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
        public IActionResult Customers_Count(DateTime startDate, DateTime endDate)
        {
            var CustomersCount = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate)
                .GroupBy(hc => hc.CallTo)
                .Count();
            return Ok(CustomersCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UniqueCustomers(DateTime startDate, DateTime endDate)
        {
            var UniqueCustomers = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate)
                .Select(hc => hc.CallTo)
                .Distinct()
                .ToList();
            if (UniqueCustomers == null)
                return NotFound();
            return Ok(UniqueCustomers);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Active_Customers_Count(DateTime startDate, DateTime endDate)
        {
            var ActiveCustomersCount = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Active")
                .Select(hc => hc.CallTo)
                .Distinct()
                .Count();
            return Ok(ActiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Inactive_Customers_Count(DateTime startDate, DateTime endDate)
        {
            var InactiveCustomersCount = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Inactive")
                .Select(hc => hc.CallTo)
                .Distinct()
                .Count();
            return Ok(InactiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AllCalls_Count(DateTime startDate, DateTime endDate)
        {
            var AllCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate);
            return Ok(AllCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult SuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            var SuccessfulCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Made");
            return Ok(SuccessfulCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ActiveAfterCalls_Count(DateTime startDate, DateTime endDate)
        {
            var ActiveAfterCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Inactive" && hc.TradeStatusAffter == "Active");
            return Ok(ActiveAfterCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ActiveInOtherBrockers_Count(DateTime startDate, DateTime endDate)
        {
            var ActiveInOtherBrockersCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && (hc.CustomerRequirement == "فعال در کارگزاری دیگر" || hc.CustomerRequirement1 == "فعال در کارگزاری دیگر" || hc.CustomerRequirement2 == "فعال در کارگزاری دیگر"));
            return Ok(ActiveInOtherBrockersCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ExplanationClub_Count(DateTime startDate, DateTime endDate)
        {
            var ExplanationClubCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.ExplanationClub == true);
            return Ok(ExplanationClubCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ActiveSuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            var ActiveSuccessfulCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Active" && hc.statusReason == "Made" );
            return Ok(ActiveSuccessfulCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult InactiveSuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            var InactiveSuccessfulCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Inactive" && hc.statusReason == "Made");
            return Ok(InactiveSuccessfulCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DisinclinationCalls_Count(DateTime startDate, DateTime endDate)
        {
            var DisinclinationCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "disinclination");
            return Ok(DisinclinationCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ReCalls_Count(DateTime startDate, DateTime endDate)
        {
            var ReCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "ReCall");
            return Ok(ReCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult LackInfoCalls_Count(DateTime startDate, DateTime endDate)
        {
            var LackInfoCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Lack of information");
            return Ok(LackInfoCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RepeatCalls_Count(DateTime startDate, DateTime endDate)
        {
            var RepeatCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Repeat Call");
            return Ok(RepeatCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UnResponsiveCalls_Count(DateTime startDate, DateTime endDate)
        {
            var UnResponsiveCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "UnResponsive");
            return Ok(UnResponsiveCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult OffCalls_Count(DateTime startDate, DateTime endDate)
        {
            var OffCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Off");
            return Ok(OffCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RejectCalls_Count(DateTime startDate, DateTime endDate)
        {
            var RejectCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Reject");
            return Ok(RejectCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UnavailableCalls_Count(DateTime startDate, DateTime endDate)
        {
            var UnavailableCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Unavailable");
            return Ok(UnavailableCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult BusyCalls_Count(DateTime startDate, DateTime endDate)
        {
            var BusyCallsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.statusReason == "Busy");
            return Ok(BusyCallsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult UserRequests_Count(DateTime startDate, DateTime endDate)
        {
            var UserRequestsCount = _dbContext.HappyCalls
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.UserRequest != null);
            return Ok(UserRequestsCount);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Total_Count_Day(DateTime startDate, DateTime endDate)
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
        public IActionResult SuccessfulCalls_Count_Day(DateTime startDate, DateTime endDate)
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
        public IActionResult UnsuccessfulCalls_Count_Day(DateTime startDate, DateTime endDate)
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

        [Authorize]
        [HttpGet]
        public IActionResult ActiveIntroduction(DateTime startDate, DateTime endDate)
        {
            var ActiveIntroduction = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Active")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Introduction = group.Any(hc => hc.introduction != null) ? group.First(hc => hc.introduction != null).introduction : "نامشخص" })
                .GroupBy(hc => hc.Introduction)
                .Select(group => new { Introduction = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (ActiveIntroduction == null || !ActiveIntroduction.Any())
                return NotFound();
            return Ok(ActiveIntroduction);
        }

        [Authorize]
        [HttpGet]
        public IActionResult InactiveIntroduction(DateTime startDate, DateTime endDate)
        {
            var InactiveIntroduction = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Inactive")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Introduction = group.Any(hc => hc.introduction != null) ? group.First(hc => hc.introduction != null).introduction : "نامشخص" })
                .GroupBy(hc => hc.Introduction)
                .Select(group => new { Introduction = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (InactiveIntroduction == null || !InactiveIntroduction.Any())
                return NotFound();
            return Ok(InactiveIntroduction);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ActiveChoosingBrokerage(DateTime startDate, DateTime endDate)
        {
            var ActiveChoosingBrokerage = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Active")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, ChoosingBrokerage = group.Any(hc => hc.ChoosingBrokerage != null) ? group.First(hc => hc.ChoosingBrokerage != null).ChoosingBrokerage : "نامشخص" })
                .GroupBy(hc => hc.ChoosingBrokerage)
                .Select(group => new { ChoosingBrokerage = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (ActiveChoosingBrokerage == null || !ActiveChoosingBrokerage.Any())
                return NotFound();
            return Ok(ActiveChoosingBrokerage);
        }

        [Authorize]
        [HttpGet]
        public IActionResult InactiveChoosingBrokerage(DateTime startDate, DateTime endDate)
        {
            var InactiveChoosingBrokerage = _dbContext.HappyCalls
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.TradeStatus == "Inactive")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, ChoosingBrokerage = group.Any(hc => hc.ChoosingBrokerage != null) ? group.First(hc => hc.ChoosingBrokerage != null).ChoosingBrokerage : "نامشخص" })
                .GroupBy(hc => hc.ChoosingBrokerage)
                .Select(group => new { ChoosingBrokerage = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            if (InactiveChoosingBrokerage == null || !InactiveChoosingBrokerage.Any())
                return NotFound();
            return Ok(InactiveChoosingBrokerage);
        }


    }
}
