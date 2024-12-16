using FarzamTEWebsite.Data;
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
    }
}
