using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbunLuxuryVillas.Models;
using EmbunLuxuryVillas.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.IO;

namespace EmbunLuxuryVillas
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncCmsController : Controller
    {
        private readonly IOptions<AppConfigurations> _appConfigurations;
        private IHttpClientFactory _factory;

        public SyncCmsController(IOptions<AppConfigurations> config, IHttpClientFactory factory)
        {
            _appConfigurations = config;
            _factory = factory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
#if !DEBUG
            var headers = Request.Headers;

            string dateTimeString = Request.Headers["datetime"];
            string publicKey = Request.Headers["publickey"];
            string digestedMessage = Request.Headers["digestedmessage"];

            if (string.IsNullOrEmpty(dateTimeString) || string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(digestedMessage))
            {
                return BadRequest();
            }

            if (digestedMessage != GetDigestedMessage(publicKey, dateTimeString))
                return BadRequest("Please check the public key provided!");
#endif

            //Download all necessary files
            HttpClient client = _factory.CreateClient();

            //CMS Production
            var baseUrl = "https://cms.mysoftinn.com";

            //CMS Staging
            //var baseUrl = "https://cms-dev.mysoftinn.com";

            //CMS Local
            //baseUrl = "http://cms.mysoftinn.local:58893";

            var cssLibrary = $"{baseUrl}/CustomFile/softinnCsslibrary";
            var scriptLibrary = $"{baseUrl}/CustomFile/softinnScriptlibrary";
            var softinnFunctions = $"{baseUrl}/CustomFile/SoftinnFunctions?hotelId={_appConfigurations.Value.HotelId}";

            var headScript = $"{baseUrl}/CustomFile/CustomHeadScript?hotelId={_appConfigurations.Value.HotelId}";
            var startingOfBodyScript = $"{baseUrl}/CustomFile/CustomStartingBodyScript?hotelId={_appConfigurations.Value.HotelId}";
            var endingOfBodyScript = $"{baseUrl}/CustomFile/CustomEndingBodyScript?hotelId={_appConfigurations.Value.HotelId}";

            var response = client.GetAsync(cssLibrary).Result;
            byte[] info = new UTF8Encoding(true).GetBytes(response.Content.ReadAsStringAsync()
                              .Result);

            using (FileStream fs = System.IO.File.Create("wwwroot/css/SoftinnLibrary.css"))
            {
                fs.Write(info, 0, info.Length);
            }

            response = client.GetAsync(scriptLibrary).Result;
            info = new UTF8Encoding(true).GetBytes(response.Content.ReadAsStringAsync()
                              .Result);

            using (FileStream fs = System.IO.File.Create("wwwroot/Scripts/SoftinnLibrary.js"))
            {
                fs.Write(info, 0, info.Length);
            }

            response = client.GetAsync(softinnFunctions).Result;
            info = new UTF8Encoding(true).GetBytes(response.Content.ReadAsStringAsync()
                              .Result);

            using (FileStream fs = System.IO.File.Create("wwwroot/Scripts/SoftinnFunctions.js"))
            {
                fs.Write(info, 0, info.Length);
            }

            response = client.GetAsync(headScript).Result;
            info = new UTF8Encoding(true).GetBytes(response.Content.ReadAsStringAsync()
                              .Result);

            using (FileStream fs = System.IO.File.Create("wwwroot/Scripts/headScript.js"))
            {
                fs.Write(info, 0, info.Length);
            }

            response = client.GetAsync(startingOfBodyScript).Result;
            info = new UTF8Encoding(true).GetBytes(response.Content.ReadAsStringAsync()
                              .Result);

            using (FileStream fs = System.IO.File.Create("wwwroot/Scripts/startingOfBodyScript.js"))
            {
                fs.Write(info, 0, info.Length);
            }

            response = client.GetAsync(endingOfBodyScript).Result;
            info = new UTF8Encoding(true).GetBytes(response.Content.ReadAsStringAsync()
                              .Result);

            using (FileStream fs = System.IO.File.Create("wwwroot/Scripts/endingOfBodyScript.js"))
            {
                fs.Write(info, 0, info.Length);
            }

            var azureStorageHelper = new AzureStorageHelper(_appConfigurations);
            var databaseIndex = await azureStorageHelper.GetDatabaseIndexByHotelId(_appConfigurations.Value.HotelId);

            if (databaseIndex == null)
            {
                return BadRequest();
            }

            var isSuccess = await azureStorageHelper.DownloadDatabaseFile(databaseIndex.DatabaseFileName);

            if (isSuccess)
                return Ok();

            return BadRequest("Unable to replace LiteDb database!");
        }

        private string GetDigestedMessage(string publicKey, string dateTimeString)
        {
            var privateKey = _appConfigurations.Value.PrivateKey.ToString(CultureInfo.InvariantCulture);

            using (SHA256 hashvalue = SHA256Managed.Create())
            {
                return String.Join("",
                    hashvalue.ComputeHash(Encoding.UTF8.GetBytes($"{publicKey}{privateKey}{dateTimeString.ToString()}"))
                        .Select(item => item.ToString("x2").ToUpper()));
            }
        }
    }
}