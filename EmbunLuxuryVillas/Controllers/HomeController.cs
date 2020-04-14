using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmbunLuxuryVillas.Models;
using System.Threading.Tasks;
using EmbunLuxuryVillas.ViewModels;
using System;
using EmbunLuxuryVillas.Helpers;

namespace EmbunLuxuryVillas.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OurStory()
        {
            return View();
        }

        public IActionResult TheVillasSuite()
        {
            return View();
        }

        public IActionResult Activities()
        {
            return View();
        }

        public IActionResult Policy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody]SendMailViewModel sendMailViewModel)
        {
            if (String.IsNullOrEmpty(sendMailViewModel.Name) || String.IsNullOrEmpty(sendMailViewModel.Email) || String.IsNullOrEmpty(sendMailViewModel.Phone) || String.IsNullOrEmpty(sendMailViewModel.Subject) ||
                String.IsNullOrEmpty(sendMailViewModel.Message))
                return BadRequest("Please enter all required fields.");

            var validation = IsValidEmailAddress(sendMailViewModel.Email);
            if (!validation)
                return BadRequest("Please enter valid email address.");
            try
            {
                await BvHelper.SendMail(sendMailViewModel);
                return Ok("Email had been sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        private static bool IsValidEmailAddress(string emailAddress)
        {
            return new System.ComponentModel.DataAnnotations
                    .EmailAddressAttribute()
                .IsValid(emailAddress);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SendTourPackageInquiryEmail([FromBody]SendTourPackageInquiryMailViewModel sendTourInquiryMailViewModel)
        {
            if (String.IsNullOrEmpty(sendTourInquiryMailViewModel.Package) || String.IsNullOrEmpty(sendTourInquiryMailViewModel.Name) || String.IsNullOrEmpty(sendTourInquiryMailViewModel.Email) || String.IsNullOrEmpty(sendTourInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var validation = IsValidEmailAddress(sendTourInquiryMailViewModel.Email);
            if (!validation)
                return BadRequest("Please enter valid email address.");
            try
            {
                await BvHelper.SendTourPackageInquiryMail(sendTourInquiryMailViewModel);
                return Ok("Email had been sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> SendMiceInquiryEmail([FromBody]SendMiceInquiryMailViewModel sendMiceInquiryMailViewModel)
        {
            if (String.IsNullOrEmpty(sendMiceInquiryMailViewModel.Package) || String.IsNullOrEmpty(sendMiceInquiryMailViewModel.Name) || String.IsNullOrEmpty(sendMiceInquiryMailViewModel.Email) || String.IsNullOrEmpty(sendMiceInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var validation = IsValidEmailAddress(sendMiceInquiryMailViewModel.Email);
            if (!validation)
                return BadRequest("Please enter valid email address.");
            try
            {
                await BvHelper.SendMiceInquiryMail(sendMiceInquiryMailViewModel);
                return Ok("Email had been sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> SendDiningInquiryEmail([FromBody]SendDiningInquiryMailViewModel sendDiningInquiryMailViewModel)
        {
            if (String.IsNullOrEmpty(sendDiningInquiryMailViewModel.Name) ||
                String.IsNullOrEmpty(sendDiningInquiryMailViewModel.Email) ||
                String.IsNullOrEmpty(sendDiningInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var validation = IsValidEmailAddress(sendDiningInquiryMailViewModel.Email);
            if (!validation)
                return BadRequest("Please enter valid email address.");

            try
            {
                await BvHelper.SendDiningInquiryMail(sendDiningInquiryMailViewModel);
                return Ok("Email had been sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}