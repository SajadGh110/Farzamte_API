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
    public class NoticeController : ControllerBase
    {
        private FarzamDbContext _dbContext;

        public NoticeController(FarzamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate_SMS()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = _dbContext.Notice_SMS
                .AsNoTracking()
                .Where(inc => inc.Broker == Broker)
                .Max(inc => inc.modifiedon)
                .ToString("yyyy-MM-dd");
            return Ok(new { EndDate = LastDate });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate_Call()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = _dbContext.Notice_Call
                .AsNoTracking()
                .Where(inc => inc.Broker == Broker)
                .Max(inc => inc.createdon)
                .ToString("yyyy-MM-dd");
            return Ok(new { EndDate = LastDate });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Count_SMS_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var happyCallsByDay = _dbContext.Notice_SMS
                .AsNoTracking()
                .Where(hc => hc.modifiedon >= startDate && hc.modifiedon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.modifiedon.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(happyCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Count_TotalCalls_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var happyCallsByDay = _dbContext.Notice_Call
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
        public async Task<IActionResult> Count_SuccessfulCalls_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var SuccessfulCallsByDay = _dbContext.Notice_Call
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
        public async Task<IActionResult> Count_UnsuccessfulCalls_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var SuccessfulCallsByDay = _dbContext.Notice_Call
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
            return Ok(SuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Noticetype_SMS(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Noticetype = await _dbContext.Notice_SMS
                .AsNoTracking()
                .Where(n => n.modifiedon >= startDate && n.modifiedon <= endDate && n.Broker == Broker)
                .Select(n => new { noticetype = (n.noticetype == null || n.noticetype == "NULL") ? "نامشخص" : n.noticetype })
                .GroupBy(n => n.noticetype)
                .Select(group => new { Noticetype = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(Noticetype);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Noticetype_Call(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Noticetype = await _dbContext.Notice_Call
                .AsNoTracking()
                .Where(n => n.createdon >= startDate && n.createdon <= endDate && n.Broker == Broker)
                .Select(n => new { noticetype = (n.noticetype == null || n.noticetype == "NULL") ? "نامشخص" : n.noticetype })
                .GroupBy(n => n.noticetype)
                .Select(group => new { Noticetype = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(Noticetype);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> capitalincrease_type_SMS(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Noticetype = await _dbContext.Notice_SMS
                .AsNoTracking()
                .Where(n => n.modifiedon >= startDate && n.modifiedon <= endDate && n.Broker == Broker && n.typeofcapitalincrease != null && n.typeofcapitalincrease != "NULL")
                .Select(n => n.typeofcapitalincrease)
                .GroupBy(n => n)
                .Select(group => new { Name = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(Noticetype);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> capitalincrease_type_Call(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Noticetype = await _dbContext.Notice_Call
                .AsNoTracking()
                .Where(n => n.createdon >= startDate && n.createdon <= endDate && n.Broker == Broker && n.typeofcapitalincrease != null && n.typeofcapitalincrease != "NULL")
                .Select(n => n.typeofcapitalincrease)
                .GroupBy(n => n)
                .Select(group => new { Name = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(Noticetype);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> symbol_SMS(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Noticetype = await _dbContext.Notice_SMS
                .AsNoTracking()
                .Where(n => n.modifiedon >= startDate && n.modifiedon <= endDate && n.Broker == Broker)
                .Select(n => n.symbol)
                .GroupBy(n => n)
                .Select(group => new { symbol = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(Noticetype);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> symbol_Call(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Noticetype = await _dbContext.Notice_Call
                .AsNoTracking()
                .Where(n => n.createdon >= startDate && n.createdon <= endDate && n.Broker == Broker)
                .Select(n => n.symbol)
                .GroupBy(n => n)
                .Select(group => new { symbol = group.Key, Count = group.Count() })
                .ToListAsync();

            return Ok(Noticetype);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GenerateFakeRecords_Call(DateTime date)
        {
            var fakeRecords = new List<Notice_Call>();
            var random = new Random();
            var noticetypes = new[] { "مجمع افزایش سرمایه", "مجمع سود نقدی" };
            var typeofcapitalincreases = new[] { "سود انباشته", "آورده نقدی" };
            var symbols = new[] { "فولاد", "حکشتی", "دانا", "غگل", "ومعادن" };
            var experts = new[] { "fatemeh rezaei", "akram lotfi", "Fateme Akbari" };
            var statusReasons = new[] { "Made", "Unsuccessful" };

            for (int i = 0; i < random.Next(9, 35); i++)
            {
                var symbol = symbols[random.Next(symbols.Length)];
                var noticetype = noticetypes[random.Next(noticetypes.Length)];

                switch (noticetype)
                {
                    case "مجمع افزایش سرمایه":
                        typeofcapitalincreases = new[] { "سود انباشته", "آورده نقدی" };
                        break;
                    case "مجمع سود نقدی":
                        typeofcapitalincreases = new[] {"NULL"};
                        break;
                    default:
                        break;
                }

                var fakeRecord = new Notice_Call
                {
                    noticeid = "id-" + random.Next(1, 9999),
                    fullName = "Customer " + random.Next(1, 25),
                    noticetype = noticetype,
                    statusReason = statusReasons[random.Next(statusReasons.Length)],
                    typeofcapitalincrease = typeofcapitalincreases[random.Next(typeofcapitalincreases.Length)],
                    symbol = symbol,
                    expert = experts[random.Next(experts.Length)],
                    Broker = "demo",
                    createdon = date,
                    company = "Company " + symbol,
                    description = "test text " + random.Next(1, 9999),
                };
                fakeRecords.Add(fakeRecord);
            }
            _dbContext.Notice_Call.AddRange(fakeRecords);
            _dbContext.SaveChanges();
            return Ok($"{fakeRecords.Count} fake records have been generated and saved.");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GenerateFakeRecords_SMS(DateTime date)
        {
            var fakeRecords = new List<Notice_SMS>();
            var random = new Random();
            var noticetypes = new[] { "مجمع افزایش سرمایه", "مجمع سود نقدی", "آگهی دعوت ( افزایش سرمایه)", "تصمیمات مجمع عمومی عادی سالیانه" };
            var typeofcapitalincreases = new[] { "سود انباشته", "آورده نقدی" };
            var symbols = new[] { "فولاد", "حکشتی", "دانا", "امین", "فروس", "واعتبار", "شپدیس", "سامان", "فملی", "هرمز", "شپنا", "شتران", "وتوکا", "فسازان", "والبر", "فمراد", "فاراک" };
            var experts = new[] { "fatemeh rezaei", "akram lotfi", "Fateme Akbari" };

            for (int i = 0; i < random.Next(100, 400); i++)
            {
                var symbol = symbols[random.Next(symbols.Length)];
                var noticetype = noticetypes[random.Next(noticetypes.Length)];

                switch (noticetype)
                {
                    case "مجمع افزایش سرمایه":
                        typeofcapitalincreases = new[] { "سود انباشته", "آورده نقدی" };
                        break;
                    case "مجمع سود نقدی":
                    case "آگهی دعوت ( افزایش سرمایه)":
                    case "تصمیمات مجمع عمومی عادی سالیانه":
                        typeofcapitalincreases = new[] { "NULL" };
                        break;
                    default:
                        break;
                }

                var fakeRecord = new Notice_SMS
                {
                    noticeid = "id-" + random.Next(1, 9999),
                    fullName = "Customer " + random.Next(1, 25),
                    noticetype = noticetype,
                    typeofcapitalincrease = typeofcapitalincreases[random.Next(typeofcapitalincreases.Length)],
                    symbol = symbol,
                    expert = experts[random.Next(experts.Length)],
                    Broker = "demo",
                    modifiedon = date,
                    company = "Company " + symbol,
                    description = "test text " + random.Next(1, 9999),
                    response_id = "res" + random.Next(1,9999)
                };
                fakeRecords.Add(fakeRecord);
            }
            _dbContext.Notice_SMS.AddRange(fakeRecords);
            _dbContext.SaveChanges();
            return Ok($"{fakeRecords.Count} fake records have been generated and saved.");
        }
    }
}
