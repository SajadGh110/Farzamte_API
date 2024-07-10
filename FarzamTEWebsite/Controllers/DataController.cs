using Microsoft.AspNetCore.Mvc;
using FarzamTEWebsite.Data;
using System.Text;
using Newtonsoft.Json;
using FarzamTEWebsite.Models;
using AuthenticationPlugin;



namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private IConfiguration _configuration;
        private FarzamDbContext _dbContext;

        public DataController(FarzamDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> HappyCall_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.16.22.7:8081/api/HappyCallReport";
            var data = new
            {
                StartDate = stDate,
                EndDate = enDate
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", "739C109D-F6FC-EC11-BAD5-005056B5FE72");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8 , "application/json");

                HttpResponseMessage response = await client.PostAsync(url,content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> HappyCall_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.16.22.7:8081/api/HappyCallReport";
            var data = new
            {
                StartDate = stDate,
                EndDate = enDate
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Authorization", "739C109D-F6FC-EC11-BAD5-005056B5FE72");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<HappyCall> HappiCallObjects = JsonConvert.DeserializeObject<List<HappyCall>>(responseContent);
                    _dbContext.HappyCalls.AddRange(HappiCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All HappyCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
    }
}
