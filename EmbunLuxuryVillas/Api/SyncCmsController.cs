using EmbunLuxuryVillas.Helpers;
using EmbunLuxuryVillas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            {
                return BadRequest("Please check the public key provided!");
            }   
#endif
            //Download all necessary files
            HttpClient client = _factory.CreateClient();

            //CMS Production
            var baseUrl = "https://cms.mysoftinn.com";

            //CMS Staging
            //var baseUrl = "https://cms-dev.mysoftinn.com";

            var hotelId = _appConfigurations.Value.HotelId;

            var cssLibrary = $"{baseUrl}/CustomFile/softinnCsslibrary";
            var scriptLibrary = $"{baseUrl}/CustomFile/softinnScriptlibrary";
            var softinnFunctions = $"{baseUrl}/CustomFile/SoftinnFunctions?hotelId={hotelId}";

            var headScript = $"{baseUrl}/CustomFile/CustomHeadScript?hotelId={hotelId}";
            var startingOfBodyScript = $"{baseUrl}/CustomFile/CustomStartingBodyScript?hotelId={hotelId}";
            var endingOfBodyScript = $"{baseUrl}/CustomFile/CustomEndingBodyScript?hotelId={hotelId}";

            try
            {
                await GetContentAndWriteToFile(cssLibrary, "wwwroot/css/SoftinnLibrary.css", client);
                await GetContentAndWriteToFile(scriptLibrary, "wwwroot/Scripts/SoftinnLibrary.js", client);
                await GetContentAndWriteToFile(softinnFunctions, "wwwroot/Scripts/SoftinnFunctions.js", client);
                await GetContentAndWriteToFile(headScript, "wwwroot/Scripts/headScript.js", client);
                await GetContentAndWriteToFile(startingOfBodyScript, "wwwroot/Scripts/startingOfBodyScript.js", client);
                await GetContentAndWriteToFile(endingOfBodyScript, "wwwroot/Scripts/endingOfBodyScript.js", client);

            }
            catch
            {
                return BadRequest("Unable to download files from CMS.");
            }

            var azureStorageHelper = new AzureStorageHelper(_appConfigurations);
            var databaseIndex = await azureStorageHelper.GetDatabaseIndexByHotelId(hotelId);

            if (databaseIndex == null)
            {
                return BadRequest();
            }

            var isSuccess = await azureStorageHelper.DownloadDatabaseFile(databaseIndex.DatabaseFileName);

            if (isSuccess)
            {
                return Ok();
            }

            return BadRequest("Unable to replace LiteDb database!");
        }

        private string GetDigestedMessage(string publicKey, string dateTimeString)
        {
            var privateKey = _appConfigurations.Value.PrivateKey.ToString(CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(publicKey))
            {
                using (SHA256 hashvalue = SHA256Managed.Create())
                {
                    return String.Join("",
                        hashvalue.ComputeHash(Encoding.UTF8.GetBytes($"{publicKey}{privateKey}{dateTimeString.ToString()}"))
                            .Select(item => item.ToString("x2").ToUpper()));
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task GetContentAndWriteToFile(string url, string filePath, HttpClient client)
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                byte[] info = new UTF8Encoding(true).GetBytes(content);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}