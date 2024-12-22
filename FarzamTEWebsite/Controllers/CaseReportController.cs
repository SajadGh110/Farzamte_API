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
    public class CaseReportController : ControllerBase
    {
        private FarzamDbContext _dbContext;

        public CaseReportController(FarzamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.Broker == Broker)
                .Max(item => item.createdon)
                .ToString("yyyy-MM-dd");
            return Ok(new { EndDate = LastDate });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ticket_Reasons(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Tickets = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker)
                .ToListAsync();

            var Reasons = Tickets
                .Select(item => item.phonecallReason)
                .Where(reason => !string.IsNullOrEmpty(reason) && reason != "NULL")
                .GroupBy(reason => reason)
                .Select(group => new { Reason = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .ToList();

            return Ok(Reasons);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Reason_Detail(DateTime startDate, DateTime endDate, string Reason)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ReasonDetail = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker && item.phonecallReason == Reason)
                .GroupBy(item => item.casetype)
                .Select(group => new
                {
                    ReasonDetail = string.IsNullOrEmpty(group.Key) || group.Key.Equals("NULL", StringComparison.OrdinalIgnoreCase) ? "نامشخص" : group.Key,
                    Count = group.Count()
                }).ToListAsync();

            return Ok(ReasonDetail);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ticket_Status(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var Tickets = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker)
                .ToListAsync();

            var Status = Tickets
                .Select(item => item.status)
                .Where(status => !string.IsNullOrEmpty(status) && status != "NULL")
                .GroupBy(status => status)
                .Select(group => new { status = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .ToList();

            return Ok(Status);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> casetype_detail(DateTime startDate, DateTime endDate, string casetype)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var selected_fields = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker && item.casetype == casetype)
                .Select(item => new { item.owner, item.createdon, item.CaseResolutionCreatedOn, item.CaseResolutionsolver, item.status, item.CustomerName })
                .ToListAsync();
            return Ok(selected_fields);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Total_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var all = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker)
                .Select(item => new
                { 
                    item.Id,
                    item.owner,
                    item.createdon,
                    item.CaseResolutionCreatedOn,
                    item.CaseResolutionsolver,
                    item.status
                })
                .OrderByDescending(item => item.createdon)
                .ToListAsync();
            return Ok(all);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Solved_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var all = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker && item.status == "مشکل حل شده")
                .Select(item => new
                {
                    item.Id,
                    item.owner,
                    item.createdon,
                    item.CaseResolutionCreatedOn,
                    item.CaseResolutionsolver,
                    item.status
                })
                .OrderByDescending(item => item.createdon)
                .ToListAsync();
            return Ok(all);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> InProgress_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var all = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker && item.status == "در حال پیش رفت")
                .Select(item => new
                {
                    item.Id,
                    item.owner,
                    item.createdon,
                    item.status
                })
                .OrderByDescending(item => item.createdon)
                .ToListAsync();
            return Ok(all);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cancelled_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var all = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker && item.status == "لغو شده")
                .Select(item => new
                {
                    item.Id,
                    item.owner,
                    item.createdon,
                    item.status
                })
                .OrderByDescending(item => item.createdon)
                .ToListAsync();
            return Ok(all);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> InfoProvided_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var all = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.createdon >= startDate && item.createdon <= endDate && item.Broker == Broker && item.status == "اطلاعات ارائه شده")
                .Select(item => new
                {
                    item.Id,
                    item.owner,
                    item.createdon,
                    item.status
                })
                .OrderByDescending(item => item.createdon)
                .ToListAsync();
            return Ok(all);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> detail_table(int id)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var selected_record = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new { item.owner, item.createdon, item.CaseResolutionCreatedOn, item.CaseResolutionsolver, item.status, item.CustomerName, item.phonecallReason, item.casetype, item.caseAutoNumber })
                .FirstOrDefaultAsync();
            if (selected_record == null)
            {
                return NotFound("Wrong ID!");
            }
            return Ok(selected_record);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GenerateFakeRecords(DateTime date)
        {
            var fakeRecords = new List<CaseReport>();
            var random = new Random();
            var phonecallreasons = new[] { "کاربری", "انتقاد پیشنهاد", "مالی", "خدمات", "ثبت نام" };
            string[] phonecallreasondetails = { "" };
            string[] casetypes = { "" };
            var CaseResolutionsolvers = new[] { "Azam Karimi", "Hadiseh Amirbeigi"};
            var Owners = new[] { "Faezeh Safizadeh", "Akram Lotfi", "Zohreh Khandan", "Rezvaneh Akbari" };
            var Status = "";

            for (int i = 0; i < random.Next(10, 50); i++)
            {
                var customer = "Customer " + random.Next(1, 9999);
                var phonecallreason = phonecallreasons[random.Next(phonecallreasons.Length)];
                var statuscode = random.Next(1, 4).ToString();

                switch (phonecallreason)
                {
                    case "کاربری":
                        phonecallreasondetails = new[] { "تغییر نسخه پنل", "درخواست مجدد کاربری" };
                        casetypes = new[] { "تغییر نسخه پنل", "درخواست مجدد کاربری" };
                        break;
                    case "انتقاد پیشنهاد":
                        phonecallreasondetails = new[] { "انتقاد", "پیشنهاد" };
                        casetypes = new[] { "انتقاد", "پیشنهاد" };
                        break;
                    case "مالی":
                        phonecallreasondetails = new[] { "عدم دریافت وجه درخواستی", "واریز وجه", "برداشت وجه" };
                        casetypes = new[] { "عدم دریافت وجه درخواستی", "واریز وجه", "برداشت وجه" };
                        break;
                    case "خدمات":
                        phonecallreasondetails = new[] { "امکان خروح از حساب", "مسدودی", "نیاز به پشتیبانی", "خرید و فروش" };
                        casetypes = new[] { "امکان خروح از حساب", "مسدودی", "نیاز به پشتیبانی", "خرید و فروش" };
                        break;
                    case "ثبت نام":
                        phonecallreasondetails = new[] { "ثبت نام غیرحضوری", "پیگیری ثبت نام", "تکمیل حساب کاربری" };
                        casetypes = new[] { "ثبت نام غیرحضوری", "پیگیری ثبت نام", "تکمیل حساب کاربری" };
                        break;
                    default:
                        break;
                }

                switch (statuscode)
                {
                    case "1":
                        Status = "مشکل حل شده";
                        break;
                    case "2":
                        Status = "لغو شده";
                        break;
                    case "3":
                        Status = "درحال بررسی";
                        break;
                    default:
                        break;
                }

                var phonecallReasonsDetails = phonecallreasondetails[random.Next(phonecallreasondetails.Length)];
                var casetype = casetypes[random.Next(casetypes.Length)];

                var fakeRecord = new CaseReport
                {
                    statuscode = statuscode,
                    status = Status,
                    Broker = "demo",
                    description = "متن تست برای دمو " + random.Next(1, 10),
                    CaseResolutionDescription = "detail " + random.Next(200,500),
                    caseAutoNumber = random.Next(100,999) + "-" + random.Next(100, 999) + "-" + random.Next(100, 999),
                    CustomerName = customer,
                    owner = Owners[random.Next(Owners.Length)],                    
                    createdon = date,
                    CaseResolutionCreatedOn = date.AddDays(2),
                    CaseResolutionsolver = CaseResolutionsolvers[random.Next(CaseResolutionsolvers.Length)],
                    CaseResolutionSubject = customer,
                    phonecallReason = phonecallreason,
                    phonecallReasonsDetails = phonecallReasonsDetails,
                    casetype = casetype,
                    title = phonecallreason + " - " + casetype,
                };
                fakeRecords.Add(fakeRecord);
            }
            _dbContext.CaseReports.AddRange(fakeRecords);
            _dbContext.SaveChanges();
            return Ok($"{fakeRecords.Count} fake records have been generated and saved.");
        }
    }
}
