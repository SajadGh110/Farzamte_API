using FarzamTEWebsite.Data;
using FarzamTEWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class HappyCallController : ControllerBase
    {
        private FarzamDbContext _dbContext;
        public HappyCallController(FarzamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Date()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);

            var uniqueDates = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.Broker == Broker)
                .Select(hc => hc.createdon.Date)
                .Distinct()
                .OrderByDescending(date => date)
                .Take(30)
                .ToListAsync();

            if (uniqueDates.Count == 0)
                return NotFound("No records found.");

            return Ok(new { StartDate = uniqueDates.Last().ToString("yyyy-MM-dd"), LastDate = uniqueDates.First().ToString("yyyy-MM-dd") });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Customers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var CustomersCount = _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.CallTo)
                .Count();
            return Ok(CustomersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Customers_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var CustomersList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(CustomersList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Active_Customers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveCustomersCount = _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .Select(hc => hc.CallTo)
                .Distinct()
                .Count();
            return Ok(ActiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Active_Customers_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var ActiveCustomersList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(ActiveCustomersList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Inactive_Customers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var InactiveCustomersCount = _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .Select(hc => hc.CallTo)
                .Distinct()
                .Count();
            return Ok(InactiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Inactive_Customers_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var InactiveCustomersList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(InactiveCustomersList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AllCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var AllCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker);
            return Ok(AllCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var SuccessfulCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Made");
            return Ok(SuccessfulCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SuccessfulCalls_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Made")
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var SuccessfulCallsList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(SuccessfulCallsList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveAfterCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveAfterCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive" && hc.TradeStatusAffter == "Active");
            return Ok(ActiveAfterCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveAfterCalls_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive" && hc.TradeStatusAffter == "Active")
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var ActiveAfterCallsList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(ActiveAfterCallsList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveInOtherBrockers_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveInOtherBrockersCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && (hc.CustomerRequirement == "فعال در کارگزاری دیگر" || hc.CustomerRequirement1 == "فعال در کارگزاری دیگر" || hc.CustomerRequirement2 == "فعال در کارگزاری دیگر" || hc.CustomerRequirement3 == "فعال در کارگزاری دیگر"));
            return Ok(ActiveInOtherBrockersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveInOtherBrockers_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && (hc.CustomerRequirement == "فعال در کارگزاری دیگر" || hc.CustomerRequirement1 == "فعال در کارگزاری دیگر" || hc.CustomerRequirement2 == "فعال در کارگزاری دیگر" || hc.CustomerRequirement3 == "فعال در کارگزاری دیگر"))
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var ActiveInOtherBrockersList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(ActiveInOtherBrockersList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ExplanationClub_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ExplanationClubCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.ExplanationClub == true);
            return Ok(ExplanationClubCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ExplanationClub_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var filteredCustomers = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.ExplanationClub == true)
                .Select(hc => new
                {
                    hc.CallTo,
                    hc.RegDate
                })
                .ToListAsync();
            var ExplanationClubList = filteredCustomers
                .GroupBy(hc => new { hc.CallTo, hc.RegDate })
                .Select(g => new
                {
                    Customer = g.Key.CallTo,
                    Reg_Date = g.Key.RegDate,
                    CallCount = g.Count()
                })
                .ToList();
            return Ok(ExplanationClubList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveSuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveSuccessfulCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active" && hc.statusReason == "Made");
            return Ok(ActiveSuccessfulCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> InactiveSuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var InactiveSuccessfulCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive" && hc.statusReason == "Made");
            return Ok(InactiveSuccessfulCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UnsuccessfulCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "Unsuccessful");
            return Ok(ReCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ReCalls_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReCallsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.statusReason == "ReCall");
            return Ok(ReCallsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserRequests_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var UserRequestsCount = _dbContext.HappyCalls
                .AsNoTracking().Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.UserRequest != null);
            return Ok(UserRequestsCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Total_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var happyCallsByDay = _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
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
        public async Task<IActionResult> SuccessfulCalls_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var SuccessfulCallsByDay = _dbContext.HappyCalls
                .AsNoTracking()
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
            return Ok(SuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UnsuccessfulCalls_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var UnsuccessfulCallsByDay = _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(h => h.statusReason == "Unsuccessful") == 0 ? 0 : g.Count(h => h.statusReason == "Unsuccessful")
                })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(UnsuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveIntroduction(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var Customers_Calls = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Calls = group.ToList() })
                .ToListAsync();
            if (!Customers_Calls.Any())
                return Ok(Customers_Calls);
            var ActiveIntroduction_Count = Customers_Calls
                .Select(group => new { Introduction = group.Calls.Any(hc => hc.introduction != null) ? group.Calls.First(hc => hc.introduction != null).introduction : "نامشخص" })
                .GroupBy(hc => hc.Introduction)
                .Select(group => new { Introduction = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            return Ok(ActiveIntroduction_Count);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> InactiveIntroduction(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var Customers_Calls = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Calls = group.ToList() })
                .ToListAsync();
            if (!Customers_Calls.Any())
                return Ok(Customers_Calls);
            var InactiveIntroduction_Count = Customers_Calls
                .Select(group => new { Introduction = group.Calls.Any(hc => hc.introduction != null) ? group.Calls.First(hc => hc.introduction != null).introduction : "نامشخص" })
                .GroupBy(hc => hc.Introduction)
                .Select(group => new { Introduction = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            return Ok(InactiveIntroduction_Count);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ActiveChoosingBrokerage(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var Customers_Calls = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Active")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Calls = group.ToList() })
                .ToListAsync();
            if (!Customers_Calls.Any())
                return Ok(Customers_Calls);
            var ActiveChoosingBrokerage_Count = Customers_Calls
                .Select(group => new { ChoosingBrokerage = group.Calls.Any(hc => hc.ChoosingBrokerage != null) ? group.Calls.First(hc => hc.ChoosingBrokerage != null).ChoosingBrokerage : "نامشخص" })
                .GroupBy(hc => hc.ChoosingBrokerage)
                .Select(group => new { ChoosingBrokerage = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            return Ok(ActiveChoosingBrokerage_Count);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> InactiveChoosingBrokerage(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var Customers_Calls = await _dbContext.HappyCalls
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.TradeStatus == "Inactive")
                .GroupBy(hc => hc.CallTo)
                .Select(group => new { CallTo = group.Key, Calls = group.ToList() })
                .ToListAsync();
            if (!Customers_Calls.Any())
                return Ok(Customers_Calls);
            var ActiveChoosingBrokerage_Count = Customers_Calls
                .Select(group => new { ChoosingBrokerage = group.Calls.Any(hc => hc.ChoosingBrokerage != null) ? group.Calls.First(hc => hc.ChoosingBrokerage != null).ChoosingBrokerage : "نامشخص" })
                .GroupBy(hc => hc.ChoosingBrokerage)
                .Select(group => new { ChoosingBrokerage = group.Key, Count = group.Count() })
                .OrderByDescending(hc => hc.Count)
                .ToList();
            return Ok(ActiveChoosingBrokerage_Count);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GenerateFakeRecords(DateTime date)
        {
            var fakeRecords = new List<HappyCall>();
            var random = new Random();
            var tradeStatuses = new[] { "Inactive", "Active" };
            var statusReasons = new[] { "Made", "Unsuccessful", "ReCall" };
            var introductions = new[] { "شبکه های اجتماعی", "معرفی آشنایان", "تبلیغات و بازاریابی", "نمایشگاه", "اتفاقی" };
            var ChoosingBrokerages = new[] { "سرمایه گذاری", "شرایط اعتبار", "بورس کالا", "اتفاقی", "سامانه های معاملاتی", "عرضه اولیه"};
            

            for (int i = 0; i < random.Next(5,25); i++)
            {
                var fakeRecord = new HappyCall
                {
                    CallTo = "Customer " + random.Next(1,9999),
                    CallFrom = "Operator " + random.Next(1,25),
                    TradeStatus = tradeStatuses[random.Next(tradeStatuses.Length)],
                    statusReason = statusReasons[random.Next(statusReasons.Length)],
                    nationalCode = "Code" + random.Next(100000000, 999999999).ToString(),
                    Broker = "demo",
                    phonenumber = "0912" + random.Next(1000000, 9999999).ToString(),
                    createdon = date,
                    RegDate = DateTime.Now.AddDays(-random.Next(0, 30)),
                    introduction = introductions[random.Next(introductions.Length)],
                    ChoosingBrokerage = ChoosingBrokerages[random.Next(ChoosingBrokerages.Length)],
                    ExplanationClub = random.Next(0, 2) == 1,
                    UserRequest = "",
                    checkingPanel = "Panel " + random.Next(1, 10),
                    CustomerRequirement = "Requirement " + random.Next(1, 10),
                    CustomerRequirementDesc = "Description " + random.Next(1, 10),
                    CustomerRequirement1 = "Requirement1 " + random.Next(1, 10),
                    CustomerRequirementDesc1 = "Description1 " + random.Next(1, 10),
                    CustomerRequirement2 = "Requirement2 " + random.Next(1, 10),
                    CustomerRequirementDesc2 = "Description2 " + random.Next(1, 10),
                    CustomerRequirement3 = "Requirement3 " + random.Next(1, 10),
                    CustomerRequirementDesc3 = "Description3 " + random.Next(1, 10),
                    TradeStatusAffter = tradeStatuses[random.Next(tradeStatuses.Length)],
                    TotalTradeAmount = random.Next(10000, 1000000).ToString(),
                    totalBrokerCommission = random.Next(100, 10000).ToString()
                };
                fakeRecords.Add(fakeRecord);
            }
            _dbContext.HappyCalls.AddRange(fakeRecords);
            _dbContext.SaveChanges();
            return Ok($"{fakeRecords.Count} fake records have been generated and saved.");
        }
    }
}
