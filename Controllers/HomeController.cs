using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace Core.GoogleCatpcha.Controllers
{
    public class reCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateForm(FormCollection form)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LdLCDcUAAAAAHf-wGiw4vSNUD6Ag1-j7Qme9cqZ"; // change this
            string gRecaptchaResponse = Request.Form["g-recaptcha-response"];
            //gRecaptchaResponse = ViewData["CResp"].ToString();
            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;
            reCaptchaResponse captChaesponse = new reCaptchaResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlToPost);
                var resp = await client.GetAsync(urlToPost + "?" + postData);
                resp.EnsureSuccessStatusCode();
                var result = await resp.Content.ReadAsStringAsync();
                captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            }
            if (captChaesponse.Success)
            {
                ViewData["Result"] = "Success";
            }
            else
            {
                ViewData["Result"] = "Fail";
            }

            // send post data
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            //request.Method = "POST";
            //request.ContentLength = postData.Length;
            //request.ContentType = "application/x-www-form-urlencoded";

            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    streamWriter.Write(postData);
            //}

            //// receive the response now
            //string result = string.Empty;
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    using (var reader = new StreamReader(response.GetResponseStream()))
            //    {
            //        result = reader.ReadToEnd();
            //    }
            //}

            //// validate the response from Google reCaptcha
            //var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            //if (!captChaesponse.Success)
            //{
            //    ViewBag.CaptchaErrorMessage = "Sorry, please validate the reCAPTCHA";
            //    return View();
            //}

            // go ahead and write code to validate username password against database


            return View("Contact");
        }
    }
}
