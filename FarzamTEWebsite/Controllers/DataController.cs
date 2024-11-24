using FarzamTEWebsite.Data;
using FarzamTEWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace FarzamTEWebsite.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private FarzamDbContext _dbContext;

        public DataController(FarzamDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Pishro -----------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Pishro_HappyCall_ShowData(DateTime stDate, DateTime enDate)
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pishro_HappyCall_SaveData(DateTime stDate, DateTime enDate)
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
                    foreach (var HappyCall in HappiCallObjects)
                    {
                        switch (HappyCall.statusReason)
                        {
                            case "ناموفق" or "ناموفق " or "عدم پاسخگویی" or "عدم پاسخگویی " or "خاموش" or "خاموش " or "رد تماس" or "رد تماس " or "خارج از دسترس" or "خارج از دسترس " or "اشغال" or "اشغال " or "مشغول" or "مشغول " or "UnResponsive" or "Off" or "Reject" or "Unavailable" or "Busy":
                                HappyCall.statusReason = "Unsuccessful";
                                break;
                            case "ساخته شده" or "ساخته شده " or "عدم تمایل به مکالمه" or "عدم تمایل به مکالمه " or "عدم اطلاع" or "عدم اطلاع " or "فاقد اطلاعات" or "فاقد اطلاعات " or "تماس تکراری" or "تماس تکراری " or "Received" or "Lack of information" or "Repeat Call" or "disinclination":
                                HappyCall.statusReason = "Made";
                                break;
                            case "تماس مجدد" or "تماس مجدد ":
                                HappyCall.statusReason = "ReCall";
                                break;
                        }
                    }
                    foreach (var HappyCall in HappiCallObjects) HappyCall.Broker = "Pishro";
                    _dbContext.HappyCalls.AddRange(HappiCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All HappyCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pishro_InComingCall_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.16.22.7:8081/api/InComingCallReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pishro_InComingCall_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.16.22.7:8081/api/InComingCallReport";
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
                    List<InComingCall> InComingCallObjects = JsonConvert.DeserializeObject<List<InComingCall>>(responseContent);
                    foreach (var InComingCall in InComingCallObjects)
                    {
                        InComingCall.Broker = "Pishro";
                        InComingCall.phonecallreason = CleanPhoneCallReason(InComingCall.phonecallreason);
                        InComingCall.phonecallreason2 = CleanPhoneCallReason(InComingCall.phonecallreason2);
                        InComingCall.phonecallreason3 = CleanPhoneCallReason(InComingCall.phonecallreason3);
                    }
                    _dbContext.InComingCalls.AddRange(InComingCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All InComingCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        // Mobin ------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Mobin_HappyCall_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/HappyCallReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_HappyCall_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/HappyCallReport";
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
                    foreach (var HappyCall in HappiCallObjects)
                    {
                        switch (HappyCall.statusReason)
                        {
                            case "ناموفق" or "ناموفق " or "عدم پاسخگویی" or "عدم پاسخگویی " or "خاموش" or "خاموش " or "رد تماس" or "رد تماس " or "خارج از دسترس" or "خارج از دسترس " or "اشغال" or "اشغال " or "مشغول" or "مشغول " or "UnResponsive" or "Off" or "Reject" or "Unavailable" or "Busy":
                                HappyCall.statusReason = "Unsuccessful";
                                break;
                            case "ساخته شده" or "ساخته شده " or "عدم تمایل به مکالمه" or "عدم تمایل به مکالمه " or "عدم اطلاع" or "عدم اطلاع " or "فاقد اطلاعات" or "فاقد اطلاعات " or "تماس تکراری" or "تماس تکراری " or "Received" or "Lack of information" or "Repeat Call" or "disinclination":
                                HappyCall.statusReason = "Made";
                                break;
                            case "تماس مجدد" or "تماس مجدد ":
                                HappyCall.statusReason = "ReCall";
                                break;
                        }
                    }
                    foreach (var HappyCall in HappiCallObjects) HappyCall.Broker = "Mobin";
                    _dbContext.HappyCalls.AddRange(HappiCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All HappyCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_InComingCall_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/InComingCallReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_InComingCall_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/InComingCallReport";
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
                    List<InComingCall> InComingCallObjects = JsonConvert.DeserializeObject<List<InComingCall>>(responseContent);
                    foreach (var InComingCall in InComingCallObjects)
                    {
                        InComingCall.Broker = "Mobin";
                        InComingCall.phonecallreason = CleanPhoneCallReason(InComingCall.phonecallreason);
                        InComingCall.phonecallreason2 = CleanPhoneCallReason(InComingCall.phonecallreason2);
                        InComingCall.phonecallreason3 = CleanPhoneCallReason(InComingCall.phonecallreason3);
                    }
                    _dbContext.InComingCalls.AddRange(InComingCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All InComingCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_CaseReport_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/CaseReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_CaseReport_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/CaseReport";
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
                    List<CaseReport> CaseReportObjects = JsonConvert.DeserializeObject<List<CaseReport>>(responseContent);
                    foreach (var CaseReport in CaseReportObjects)
                    {
                        var tempStatus = CaseReport.status;
                        CaseReport.status = CaseReport.statuscode.ToString();
                        CaseReport.statuscode = tempStatus;
                        CaseReport.Broker = "Mobin";
                        CaseReport.phonecallReason = CleanPhoneCallReason(CaseReport.phonecallReason);
                    }
                    _dbContext.CaseReports.AddRange(CaseReportObjects);
                    _dbContext.SaveChanges();
                    return Ok("All CaseReports From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_TransportToSmart_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/TransportToSmart";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_TransportToSmart_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/TransportToSmart";
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
                    List<TransportToSmart> TransportToSmartObjects = JsonConvert.DeserializeObject<List<TransportToSmart>>(responseContent);
                    foreach (var TransportToSmart in TransportToSmartObjects)
                    {
                        switch (TransportToSmart.status)
                        {
                            case "ناموفق" or "ناموفق " or "عدم پاسخگویی" or "عدم پاسخگویی " or "خاموش" or "خاموش " or "رد تماس" or "رد تماس " or "خارج از دسترس" or "خارج از دسترس " or "اشغال" or "اشغال " or "مشغول" or "مشغول " or "UnResponsive" or "Off" or "Reject" or "Unavailable" or "Busy":
                                TransportToSmart.status = "Unsuccessful";
                                break;
                            case "ساخته شده" or "ساخته شده " or "عدم تمایل به مکالمه" or "عدم تمایل به مکالمه " or "عدم اطلاع" or "عدم اطلاع " or "فاقد اطلاعات" or "فاقد اطلاعات " or "تماس تکراری" or "تماس تکراری " or "Received" or "Lack of information" or "Repeat Call" or "disinclination":
                                TransportToSmart.status = "Made";
                                break;
                            case "تماس مجدد" or "تماس مجدد ":
                                TransportToSmart.status = "ReCall";
                                break;
                        }

                        TransportToSmart.Broker = "Mobin";

                        if (TransportToSmart.reasonOfContinueSmart != null)
                            foreach (var reason in TransportToSmart.reasonOfContinueSmart)
                                _dbContext.TTS_Reasons.Add(reason);

                        if (TransportToSmart.reasonOfReturnTadbir != null)
                            foreach (var reason in TransportToSmart.reasonOfReturnTadbir)
                                _dbContext.TTS_Reasons.Add(reason);
                    }
                    _dbContext.TransportsToSmart.AddRange(TransportToSmartObjects);
                    _dbContext.SaveChanges();
                    return Ok("All TransportsToSmart From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_Notice_Call_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = $"http://192.168.38.2:8081/api/NoticeReport?StartDate={stDate.ToString("yyyy-MM-dd")}&EndDate={enDate.ToString("yyyy-MM-dd")}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "739C109D-F6FC-EC11-BAD5-005056B5FE72");
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_Notice_Call_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = $"http://192.168.38.2:8081/api/NoticeReport?StartDate={stDate.ToString("yyyy-MM-dd")}&EndDate={enDate.ToString("yyyy-MM-dd")}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "739C109D-F6FC-EC11-BAD5-005056B5FE72");
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<Notice_Call> Notice_CallObjects = JsonConvert.DeserializeObject<List<Notice_Call>>(responseContent);
                    foreach (var Notice_Call in Notice_CallObjects)
                    {
                        switch (Notice_Call.statusReason)
                        {
                            case "ناموفق" or "ناموفق " or "عدم پاسخگویی" or "عدم پاسخگویی " or "خاموش" or "خاموش " or "رد تماس" or "رد تماس " or "خارج از دسترس" or "خارج از دسترس " or "اشغال" or "اشغال " or "مشغول" or "مشغول " or "UnResponsive" or "Off" or "Reject" or "Unavailable" or "Busy":
                                Notice_Call.statusReason = "Unsuccessful";
                                break;
                            case "ساخته شده" or "ساخته شده " or "عدم تمایل به مکالمه" or "عدم تمایل به مکالمه " or "عدم اطلاع" or "عدم اطلاع " or "فاقد اطلاعات" or "فاقد اطلاعات " or "تماس تکراری" or "تماس تکراری " or "Received" or "Lack of information" or "Repeat Call" or "disinclination":
                                Notice_Call.statusReason = "Made";
                                break;
                            case "تماس مجدد" or "تماس مجدد ":
                                Notice_Call.statusReason = "ReCall";
                                break;
                        }

                        Notice_Call.Broker = "Mobin";
                    }
                    _dbContext.Notice_Call.AddRange(Notice_CallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All Notices Calls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_Notice_SMS_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/NoticeReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Mobin_Notice_SMS_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://192.168.38.2:8081/api/NoticeReport";
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
                    List<Notice_SMS> Notice_SMSObjects = JsonConvert.DeserializeObject<List<Notice_SMS>>(responseContent);
                    foreach (var Notice_SMS in Notice_SMSObjects)
                    {
                        Notice_SMS.Broker = "Mobin";
                    }
                    _dbContext.Notice_SMS.AddRange(Notice_SMSObjects);
                    _dbContext.SaveChanges();
                    return Ok("All Notices SMS From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        // Pouyan -----------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Pouyan_HappyCall_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.18.10.40:8080/api/HappyCallReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pouyan_HappyCall_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.18.10.40:8080/api/HappyCallReport";
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
                    foreach (var HappyCall in HappiCallObjects)
                    {
                        if (HappyCall.TradeStatus != "Active")
                            HappyCall.TradeStatus = "Inactive";
                        switch (HappyCall.statusReason)
                        {
                            case "ناموفق" or "ناموفق " or "عدم پاسخگویی" or "عدم پاسخگویی " or "خاموش" or "خاموش " or "رد تماس" or "رد تماس " or "خارج از دسترس" or "خارج از دسترس " or "اشغال" or "اشغال " or "مشغول" or "مشغول " or "UnResponsive" or "Off" or "Reject" or "Unavailable" or "Busy":
                                HappyCall.statusReason = "Unsuccessful";
                                break;
                            case "ساخته شده" or "ساخته شده " or "عدم تمایل به مکالمه" or "عدم تمایل به مکالمه " or "عدم اطلاع" or "عدم اطلاع " or "فاقد اطلاعات" or "فاقد اطلاعات " or "تماس تکراری" or "تماس تکراری " or "Received" or "Lack of information" or "Repeat Call" or "disinclination":
                                HappyCall.statusReason = "Made";
                                break;
                            case "تماس مجدد" or "تماس مجدد ":
                                HappyCall.statusReason = "ReCall";
                                break;
                        }
                    }
                    foreach (var HappyCall in HappiCallObjects) HappyCall.Broker = "Pouyan";
                    _dbContext.HappyCalls.AddRange(HappiCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All HappyCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pouyan_InComingCall_ShowData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.18.10.40:8080/api/InComingCallReport";
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
                    return Ok(responseContent);
                }
                else
                {
                    return BadRequest(response);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pouyan_InComingCall_SaveData(DateTime stDate, DateTime enDate)
        {
            string url = "http://172.18.10.40:8080/api/InComingCallReport";
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
                    List<InComingCall> InComingCallObjects = JsonConvert.DeserializeObject<List<InComingCall>>(responseContent);
                    foreach (var InComingCall in InComingCallObjects)
                    {
                        InComingCall.Broker = "Pouyan";
                        InComingCall.phonecallreason = CleanPhoneCallReason(InComingCall.phonecallreason);
                        InComingCall.phonecallreason2 = CleanPhoneCallReason(InComingCall.phonecallreason2);
                        InComingCall.phonecallreason3 = CleanPhoneCallReason(InComingCall.phonecallreason3);
                    }
                    _dbContext.InComingCalls.AddRange(InComingCallObjects);
                    _dbContext.SaveChanges();
                    return Ok("All InComingCalls From " + stDate + " to " + enDate + " Saved!");
                }
                else
                    return BadRequest(response);
            }
        }
        // ------------------------------------------------------------------------------------------------------

        private string CleanPhoneCallReason(string? reason)
        {
            if (string.IsNullOrEmpty(reason))
                return reason;

            return Regex.Replace(reason, @"\d+\.?", "").Trim();
        }

        [Authorize(Policy = "OwnerPolicy")]
        [HttpPost]
        public async Task<IActionResult> Add_Brokerage(Brokerage brokerage)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            if (u.Role != "Owner")
                return BadRequest("Access Denied!");
            if (String.IsNullOrEmpty(brokerage.Name))
                return BadRequest("Name Requierd!");
            else
            {
                var New_Brokerage = new Brokerage
                {
                    Name = brokerage.Name,
                    Logo = brokerage.Logo,
                };

                _dbContext.Brokerages.Add(New_Brokerage);
                await _dbContext.SaveChangesAsync();
                return Ok("Brokerage Added!");
            }
        }

        [Authorize(Policy = "OwnerPolicy")]
        [HttpPost]
        public async Task<IActionResult> Add_Transaction(Transaction_Statistics_M transaction)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _dbContext.Users.Find(id);
            if (u.Role != "Owner")
                return BadRequest("Access Denied!");
            else
            {
                _dbContext.Transaction_Statistics_M.Add(transaction);
                await _dbContext.SaveChangesAsync();
                return Ok("Transaction Added!");
            }
        }
    }
}
