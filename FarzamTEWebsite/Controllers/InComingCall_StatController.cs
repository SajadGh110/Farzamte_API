using FarzamTEWebsite.DTOs;
using FarzamTEWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class InComingCall_StatController : ControllerBase
    {
        private readonly IInComingCall_StatService _Inc_StatService;
        public InComingCall_StatController(IInComingCall_StatService dbContext)
        {
            _Inc_StatService = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LastDate()
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var LastDate = await _Inc_StatService.GetLastDate(broker);
            return Ok(LastDate);
        }

        [Authorize]
        [HttpGet]
        public async Task<List<string>> AllDate(string type)
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var AllDate = await _Inc_StatService.GetAllDate(broker, type);
            return AllDate;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<InComingCall_Stat_DTO>> IncStat(string type)
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var IncStat = await _Inc_StatService.GetIncStat(broker, type);
            return IncStat;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<InComingCall_Stat_DTO>> IncStat_M(string type, string month)
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var IncStat = await _Inc_StatService.GetIncStat(broker, type, month);
            return IncStat;
        }
    }
}
