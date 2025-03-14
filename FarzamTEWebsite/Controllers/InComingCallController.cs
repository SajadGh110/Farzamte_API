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
    [Authorize]
    public class InComingCallController : ControllerBase
    {
        private FarzamDbContext _dbContext;
        public InComingCallController(FarzamDbContext dbContext)
        {
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
        public async Task<IActionResult> Top_Reasons_Customers(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate 
                            && inc.createdon <= endDate 
                            && inc.Broker == Broker 
                            && inc.from != null 
                            && inc.from != "Unknown" 
                            && !inc.from.Contains("مشتری"))
                .ToListAsync();

            var topReasons = phoneCallReasons
                .SelectMany(inc => new[] { inc.phonecallreason, inc.phonecallreason2, inc.phonecallreason3 })
                .Where(reason => !string.IsNullOrEmpty(reason) && reason != "NULL")
                .GroupBy(reason => reason)
                .Select(group => new { Reason = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .Take(3)
                .Select(group => group.Reason)
                .ToList();

            var reasonsByDay = phoneCallReasons
                .GroupBy(inc => inc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Reasons = topReasons.ToDictionary(reason => reason, reason => g.Count(inc => inc.phonecallreason == reason || inc.phonecallreason2 == reason || inc.phonecallreason3 == reason))
                })
                .OrderBy(g => g.Date)
                .ToList();

            var chartData = reasonsByDay.Select(day =>
            {
                var data = new Dictionary<string, object> { ["reasons"] = day.Date };
                foreach (var reason in day.Reasons)
                {
                    data[reason.Key] = reason.Value;
                }
                return data;
            }).ToList();

            return Ok(chartData);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Top_Reasons_Others(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate 
                            && inc.createdon <= endDate 
                            && inc.Broker == Broker 
                            && inc.from != null 
                            && inc.from != "Unknown" 
                            && inc.from.Contains("مشتری"))
                .ToListAsync();

            var topReasons = phoneCallReasons
                .SelectMany(inc => new[] { inc.phonecallreason, inc.phonecallreason2, inc.phonecallreason3 })
                .Where(reason => !string.IsNullOrEmpty(reason) && reason != "NULL")
                .GroupBy(reason => reason)
                .Select(group => new { Reason = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .Take(3)
                .Select(group => group.Reason)
                .ToList();

            var reasonsByDay = phoneCallReasons
                .GroupBy(inc => inc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Reasons = topReasons.ToDictionary(reason => reason, reason => g.Count(inc => inc.phonecallreason == reason || inc.phonecallreason2 == reason || inc.phonecallreason3 == reason))
                })
                .OrderBy(g => g.Date)
                .ToList();

            var chartData = reasonsByDay.Select(day =>
            {
                var data = new Dictionary<string, object> { ["reasons"] = day.Date };
                foreach (var reason in day.Reasons)
                {
                    data[reason.Key] = reason.Value;
                }
                return data;
            }).ToList();

            return Ok(chartData);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Top_Reasons_Totals(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate
                            && inc.createdon <= endDate
                            && inc.Broker == Broker
                            && inc.from != null
                            && inc.from != "Unknown")
                .ToListAsync();

            var topReasons = phoneCallReasons
                .SelectMany(inc => new[] { inc.phonecallreason, inc.phonecallreason2, inc.phonecallreason3 })
                .Where(reason => !string.IsNullOrEmpty(reason) && reason != "NULL")
                .GroupBy(reason => reason)
                .Select(group => new { Reason = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .Take(3)
                .Select(group => group.Reason)
                .ToList();

            var reasonsByDay = phoneCallReasons
                .GroupBy(inc => inc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Reasons = topReasons.ToDictionary(reason => reason, reason => g.Count(inc => inc.phonecallreason == reason || inc.phonecallreason2 == reason || inc.phonecallreason3 == reason))
                })
                .OrderBy(g => g.Date)
                .ToList();

            var chartData = reasonsByDay.Select(day =>
            {
                var data = new Dictionary<string, object> { ["reasons"] = day.Date };
                foreach (var reason in day.Reasons)
                {
                    data[reason.Key] = reason.Value;
                }
                return data;
            }).ToList();

            return Ok(chartData);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Phonecall_Reasons_Customers(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate 
                            && inc.createdon <= endDate 
                            && inc.Broker == Broker 
                            && inc.from != null 
                            && inc.from != "Unknown" 
                            && !inc.from.Contains("مشتری"))
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
        public async Task<IActionResult> Phonecall_Reasons_Others(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate 
                            && inc.createdon <= endDate 
                            && inc.Broker == Broker 
                            && inc.from != null 
                            && inc.from != "Unknown" 
                            && inc.from.Contains("مشتری"))
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
        public async Task<IActionResult> Phonecall_Reasons_Totals(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var phoneCallReasons = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate
                            && inc.createdon <= endDate
                            && inc.Broker == Broker
                            && inc.from != null
                            && inc.from != "Unknown")
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
        public async Task<IActionResult> Reason_Detail_Customers(DateTime startDate, DateTime endDate, string Reason)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReasonDetail1 = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate 
                            && inc.createdon <= endDate 
                            && inc.Broker == Broker 
                            && inc.phonecallreason == Reason 
                            && inc.from != null 
                            && inc.from != "Unknown" 
                            && !inc.from.Contains("مشتری"))
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
        public async Task<IActionResult> Reason_Detail_Others(DateTime startDate, DateTime endDate, string Reason)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReasonDetail1 = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate 
                            && inc.createdon <= endDate 
                            && inc.Broker == Broker 
                            && inc.phonecallreason == Reason 
                            && inc.from != null 
                            && inc.from != "Unknown" 
                            && inc.from.Contains("مشتری"))
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
        public async Task<IActionResult> Reason_Detail_Totals(DateTime startDate, DateTime endDate, string Reason)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReasonDetail1 = await _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate
                            && inc.createdon <= endDate
                            && inc.Broker == Broker
                            && inc.phonecallreason == Reason
                            && inc.from != null
                            && inc.from != "Unknown")
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
        public async Task<IActionResult> description_Customers(DateTime startDate, DateTime endDate, string Detail)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var baseQuery = _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate
                            && inc.createdon <= endDate
                            && inc.Broker == Broker
                            && inc.description != "NULL"
                            && inc.description != null
                            && inc.from != null
                            && inc.from != "Unknown"
                            && !inc.from.Contains("مشتری"));

            var selected_fields = await baseQuery
                .Where(inc => inc.phonecallreasondetail == Detail
                            || inc.phonecallreasondetail2 == Detail
                            || inc.phonecallreasondetail3 == Detail)
                .Select(inc => new { inc.from, inc.phonenumber, inc.description })
                .ToListAsync();

            return Ok(selected_fields);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> description_Others(DateTime startDate, DateTime endDate, string Detail)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var baseQuery = _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate
                            && inc.createdon <= endDate
                            && inc.Broker == Broker
                            && inc.from != null
                            && inc.from != "Unknown"
                            && inc.from.Contains("مشتری"));

            var selected_fields = await baseQuery
                .Where(inc => inc.phonecallreasondetail == Detail
                            || inc.phonecallreasondetail2 == Detail
                            || inc.phonecallreasondetail3 == Detail)
                .Select(inc => new { inc.from, inc.phonenumber, inc.description })
                .ToListAsync();

            return Ok(selected_fields);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> description_Totals(DateTime startDate, DateTime endDate, string Detail)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var baseQuery = _dbContext.InComingCalls
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate
                            && inc.createdon <= endDate
                            && inc.Broker == Broker
                            && inc.from != null
                            && inc.from != "Unknown");

            var selected_fields = await baseQuery
                .Where(inc => inc.phonecallreasondetail == Detail
                            || inc.phonecallreasondetail2 == Detail
                            || inc.phonecallreasondetail3 == Detail)
                .Select(inc => new { inc.from, inc.phonenumber, inc.description })
                .ToListAsync();

            return Ok(selected_fields);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GenerateFakeRecords(DateTime date)
        {
            var fakeRecords = new List<InComingCall>();
            var random = new Random();
            var phonecallreasons = new[] { "مالی", "کاربری", "ثبت نام", "خدمات" };
            var customer = new[] { "Customer " + random.Next(1, 9999),"مشتری خاص" };
            string[] phonecallreasondetails = { "" };

            for (int i = 0; i < random.Next(70, 150); i++)
            {
                
                var phonecallreason = phonecallreasons[random.Next(phonecallreasons.Length)];
                
                switch (phonecallreason)
                {
                    case "مالی":
                        phonecallreasondetails = new[] { "دریافت وجه", "واریز وجه" };
                        break;
                    case "کاربری":
                        phonecallreasondetails = new[] { "درخواست مجدد کاربری", "مشکل تعیین و ثبت رمز عبور", "عدم ارسال پیامک", "عدم دریافت نام کاربری" };
                        break;
                    case "ثبت نام":
                        phonecallreasondetails = new[] { "ثبت نام غیرحضوری", "ویرایش اطلاعات", "سایر", "فراخوانی اطلاعات" };
                        break;
                    case "خدمات":
                        phonecallreasondetails = new[] { "اختلال در پرتفو", "مسدودی", "عرضه اولیه", "خرید و فروش" };
                        break;
                    default:
                        break;
                }
                var fakeRecord = new InComingCall
                {
                    from = customer[random.Next(customer.Length)],
                    to = "Operator " + random.Next(1, 25),
                    Broker = "demo",
                    phonenumber = "0912" + random.Next(1000000, 9999999).ToString(),
                    createdon = date,
                    description = "متن تست برای دمو " + random.Next(1, 10),
                    phonecallreason = phonecallreason,
                    phonecallreason2 = "",
                    phonecallreason3 = "",
                    phonecallreasondetail = phonecallreasondetails[random.Next(phonecallreasondetails.Length)],
                    phonecallreasondetail2 = "",
                    phonecallreasondetail3 = "",
                };
                fakeRecords.Add(fakeRecord);
            }
            _dbContext.InComingCalls.AddRange(fakeRecords);
            _dbContext.SaveChanges();
            return Ok($"{fakeRecords.Count} fake records have been generated and saved.");
        }
    }
}
