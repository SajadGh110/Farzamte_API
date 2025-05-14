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
        public async Task<List<string>> AllTypes()
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var AllTypes = await _Inc_StatService.GetAllTypes(broker);
            return AllTypes;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<string>> AllDates(string type)
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var AllDates = await _Inc_StatService.GetAllDates(broker, type);
            return AllDates;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<InComingCall_Stat_DTO>> IncStats(string type)
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var IncStats = await _Inc_StatService.GetIncStat(broker, type);
            return IncStats;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<InComingCall_Stat_DTO>> IncStats_M(string type, string month)
        {
            string broker = User.FindFirstValue(ClaimTypes.PrimarySid);
            var IncStats = await _Inc_StatService.GetIncStat(broker, type, month);
            return IncStats;
        }
    }
}
