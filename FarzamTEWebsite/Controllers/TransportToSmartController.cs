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
    public class TransportToSmartController : ControllerBase
    {
        private IConfiguration _configuration;
        private FarzamDbContext _dbContext;

        public TransportToSmartController(FarzamDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate()
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = _dbContext.TransportsToSmart
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
            var TotalCallsByDay = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(TotalCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ContinueSmart_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var SuccessfulCallsByDay = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(h => h.resultOfCall == "ادامه با اسمارت") == 0 ? 0 : g.Count(h => h.resultOfCall == "ادامه با اسمارت")
                })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(SuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ReturnTadbir_Count_Day(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var UnsuccessfulCallsByDay = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .GroupBy(hc => hc.createdon.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(h => h.resultOfCall == "برگشت به تدبیر") == 0 ? 0 : g.Count(h => h.resultOfCall == "برگشت به تدبیر")
                })
                .AsEnumerable()
                .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
                .OrderBy(hc => hc.Date)
                .ToList();
            return Ok(UnsuccessfulCallsByDay);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Total_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveCustomersCount = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker);
            return Ok(ActiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ContinueSmart_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveCustomersCount = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "ادامه با اسمارت");
            return Ok(ActiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ReturnTadbir_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var ActiveCustomersCount = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "برگشت به تدبیر");
            return Ok(ActiveCustomersCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Successful_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var UnsuccessfulCount = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made");
            return Ok(UnsuccessfulCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Unsuccessful_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            var UnsuccessfulCount = _dbContext.TransportsToSmart
                .AsNoTracking()
                .Count(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status != "Made");
            return Ok(UnsuccessfulCount);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Total_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Total_List = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker)
                .Select(hc => new
                {
                    hc.Id,
                    hc.from,
                    hc.to,
                    description = string.IsNullOrEmpty(hc.description) || hc.description == "NULL" ? "نامشخص" : hc.description.Length > 20 ? hc.description.Substring(0, 20) + "..." : hc.description.Replace("\"", ""),
                    hc.createdon,
                    hc.resultOfCall,
                    customerSatisfaction = string.IsNullOrEmpty(hc.customerSatisfaction) || hc.customerSatisfaction == "NULL" ? "نامشخص" : hc.customerSatisfaction
                })
                .ToListAsync();

            return Ok(Total_List);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Successful_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Total_List = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made")
                .Select(hc => new
                {
                    hc.Id,
                    hc.from,
                    hc.to,
                    description = string.IsNullOrEmpty(hc.description) || hc.description == "NULL" ? "نامشخص" : hc.description.Length > 20 ? hc.description.Substring(0, 20) + "..." : hc.description.Replace("\"", ""),
                    hc.createdon,
                    hc.resultOfCall,
                    customerSatisfaction = string.IsNullOrEmpty(hc.customerSatisfaction) || hc.customerSatisfaction == "NULL" ? "نامشخص" : hc.customerSatisfaction
                })
                .ToListAsync();

            return Ok(Total_List);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Unsuccessful_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var Total_List = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status != "Made")
                .Select(hc => new
                {
                    hc.Id,
                    hc.from,
                    hc.to,
                    description = string.IsNullOrEmpty(hc.description) || hc.description == "NULL" ? "نامشخص" : hc.description.Length > 20 ? hc.description.Substring(0, 20) + "..." : hc.description.Replace("\"", ""),
                    hc.createdon,
                    hc.resultOfCall,
                    customerSatisfaction = string.IsNullOrEmpty(hc.customerSatisfaction) || hc.customerSatisfaction == "NULL" ? "نامشخص" : hc.customerSatisfaction
                })
                .ToListAsync();

            return Ok(Total_List);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ContinueSmart_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var ContinueSmart_List = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "ادامه با اسمارت")
                .Select(hc => new
                {
                    hc.Id,
                    hc.from,
                    hc.to,
                    description = string.IsNullOrEmpty(hc.description) || hc.description == "NULL" ? "نامشخص" : hc.description.Length > 20 ? hc.description.Substring(0, 20) + "..." : hc.description.Replace("\"", ""),
                    hc.createdon,
                    hc.resultOfCall,
                    customerSatisfaction = string.IsNullOrEmpty(hc.customerSatisfaction) || hc.customerSatisfaction == "NULL" ? "نامشخص" : hc.customerSatisfaction
                })
                .ToListAsync();

            return Ok(ContinueSmart_List);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ReturnTadbir_List(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var ReturnTadbir_List = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "برگشت به تدبیر")
                .Select(hc => new
                {
                    hc.Id,
                    hc.from,
                    hc.to,
                    description = string.IsNullOrEmpty(hc.description) || hc.description == "NULL" ? "نامشخص" : hc.description.Length > 20 ? hc.description.Substring(0, 20) + "..." : hc.description.Replace("\"", ""),
                    hc.createdon,
                    hc.resultOfCall,
                    customerSatisfaction = string.IsNullOrEmpty(hc.customerSatisfaction) || hc.customerSatisfaction == "NULL" ? "نامشخص" : hc.customerSatisfaction
                })
                .ToListAsync();

            return Ok(ReturnTadbir_List);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Reasons_ContinueSmart_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var reasonsList = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "ادامه با اسمارت")
                .SelectMany(hc => hc.reasonOfContinueSmart.Select(r => r.name.Replace("\r\n", "")))
                .ToListAsync();

            var groupedReasons = reasonsList
                .GroupBy(r => r)
                .Select(grp => new
                {
                    Reason = grp.Key,
                    Count = grp.Count()
                })
                .ToList();

            return Ok(groupedReasons);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Reasons_ReturnTadbir_Count(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var reasonsList = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "برگشت به تدبیر")
                .SelectMany(hc => hc.reasonOfReturnTadbir.Select(r => r.name.Replace("\r\n", "")))
                .ToListAsync();

            var groupedReasons = reasonsList
                .GroupBy(r => r)
                .Select(grp => new
                {
                    Reason = grp.Key,
                    Count = grp.Count()
                })
                .ToList();

            return Ok(groupedReasons);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Average_CustomerSatisfaction(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var dataList = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made")
                .ToListAsync();

            var validSatisfactions = dataList
                .Where(hc => hc.customerSatisfaction != null && hc.customerSatisfaction != "NULL")
                .Select(hc => int.Parse(hc.customerSatisfaction))
                .ToList();

            if (validSatisfactions.Count == 0)
                return Ok(0);

            var averageSatisfaction = validSatisfactions.Average();
            var round_averageSatisfaction = Math.Round(averageSatisfaction, 0);

            return Ok(round_averageSatisfaction);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Average_CustomerSatisfaction_Smart(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var dataList = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "ادامه با اسمارت")
                .ToListAsync();

            var validSatisfactions = dataList
                .Where(hc => hc.customerSatisfaction != null && hc.customerSatisfaction != "NULL")
                .Select(hc => int.Parse(hc.customerSatisfaction))
                .ToList();

            if (validSatisfactions.Count == 0)
                return Ok(0);

            var averageSatisfaction = validSatisfactions.Average();
            var round_averageSatisfaction = Math.Round(averageSatisfaction, 0);

            return Ok(round_averageSatisfaction);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Average_CustomerSatisfaction_Tadbir(DateTime startDate, DateTime endDate)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            endDate = endDate.Date.AddDays(1).AddTicks(-1);

            var dataList = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(hc => hc.createdon >= startDate && hc.createdon <= endDate && hc.Broker == Broker && hc.status == "Made" && hc.resultOfCall == "برگشت به تدبیر")
                .ToListAsync();

            var validSatisfactions = dataList
                .Where(hc => hc.customerSatisfaction != null && hc.customerSatisfaction != "NULL")
                .Select(hc => int.Parse(hc.customerSatisfaction))
                .ToList();

            if (validSatisfactions.Count == 0)
                return Ok(0);

            var averageSatisfaction = validSatisfactions.Average();
            var round_averageSatisfaction = Math.Round(averageSatisfaction, 0);

            return Ok(round_averageSatisfaction);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> detail_table(int id)
        {
            string Broker = User.FindFirstValue(ClaimTypes.PrimarySid);

            var selected_record = await _dbContext.TransportsToSmart
                .AsNoTracking()
                .Where(inc => inc.Id == id)
                .Select(inc => new
                {
                    inc.from,
                    inc.to,
                    description = string.IsNullOrEmpty(inc.description) || inc.description == "NULL" ? "نامشخص" : inc.description.Replace("\"", ""),
                    inc.createdon,
                    inc.phonenumber,
                    inc.nationalCode,
                    customerSatisfaction = string.IsNullOrEmpty(inc.customerSatisfaction) || inc.customerSatisfaction == "NULL" ? "نامشخص" : inc.customerSatisfaction,
                    inc.resultOfCall,
                    Reasons = inc.resultOfCall == "ادامه با اسمارت" ? inc.reasonOfContinueSmart.Select(r => r.name.Replace("\r\n", "").Replace("\"", "")).ToList() :
                              inc.resultOfCall == "برگشت به تدبیر" ? inc.reasonOfReturnTadbir.Select(r => r.name.Replace("\r\n", "").Replace("\"", "")).ToList() : null
                })
                .FirstOrDefaultAsync();

            if (selected_record == null)
            {
                return NotFound("Wrong ID!");
            }

            return Ok(selected_record);
        }
    }
}
