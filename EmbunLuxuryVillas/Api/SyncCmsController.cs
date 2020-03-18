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

namespace EmbunLuxuryVillas
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncCmsController : Controller
    {
        private readonly IOptions<AppConfigurations> _appConfigurations;

        public SyncCmsController(IOptions<AppConfigurations> config)
        {
            _appConfigurations = config;
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