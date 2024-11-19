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
        private IConfiguration _configuration;
        private FarzamDbContext _dbContext;

        public CaseReportController(FarzamDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = _dbContext.CaseReports
                .AsNoTracking()
                .Where(inc => inc.Broker == Broker)
                .Max(inc => inc.createdon)
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
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker)
                .ToListAsync();

            var Reasons = Tickets
                .Select(inc => inc.phonecallReason)
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
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker && inc.phonecallReason == Reason)
                .GroupBy(inc => inc.casetype)
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
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker)
                .ToListAsync();

            var Status = Tickets
                .Select(inc => inc.status)
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
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker && inc.casetype == casetype)
                .Select(inc => new { inc.owner, inc.createdon, inc.CaseResolutionCreatedOn, inc.CaseResolutionsolver, inc.status, inc.CustomerName })
                .ToListAsync();
            return Ok(selected_fields);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> all_table(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var all = await _dbContext.CaseReports
                .AsNoTracking()
                .Where(inc => inc.createdon >= startDate && inc.createdon <= endDate && inc.Broker == Broker)
                .Select(inc => new { inc.Id, inc.owner, inc.createdon, inc.CaseResolutionCreatedOn, inc.CaseResolutionsolver, inc.status })
                .OrderByDescending(inc => inc.createdon)
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
                .Where(inc => inc.Id == id)
                .Select(inc => new { inc.owner, inc.createdon, inc.CaseResolutionCreatedOn, inc.CaseResolutionsolver, inc.status, inc.CustomerName, inc.phonecallReason, inc.casetype, inc.caseAutoNumber })
                .FirstOrDefaultAsync();
            if (selected_record == null)
            {
                return NotFound("Wrong ID!");
            }
            return Ok(selected_record);
        }
    }
}
