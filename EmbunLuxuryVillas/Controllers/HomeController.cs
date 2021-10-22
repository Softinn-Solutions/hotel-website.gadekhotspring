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
            if (String.IsNullOrWhiteSpace(sendMailViewModel.Name) ||
                String.IsNullOrWhiteSpace(sendMailViewModel.Email) ||
                String.IsNullOrWhiteSpace(sendMailViewModel.Phone) ||
                String.IsNullOrWhiteSpace(sendMailViewModel.Subject) ||
                String.IsNullOrWhiteSpace(sendMailViewModel.Message))
                return BadRequest("Please enter all required fields.");

            var isValidEmailAddress = IsValidEmailAddress(sendMailViewModel.Email);
            if (!isValidEmailAddress)
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
        
         
        [HttpPost]
        public async Task<IActionResult> SendMeetingInquiryEmail([FromBody]SendMeetingInquiryMailViewModel sendMeetingInquiryMailViewModel)
        {
            if (String.IsNullOrWhiteSpace(sendMeetingInquiryMailViewModel.Package) ||
                String.IsNullOrWhiteSpace(sendMeetingInquiryMailViewModel.Name) ||
                String.IsNullOrWhiteSpace(sendMeetingInquiryMailViewModel.Email) ||
                String.IsNullOrWhiteSpace(sendMeetingInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var isValidEmailAddress = IsValidEmailAddress(sendMeetingInquiryMailViewModel.Email);
            if (!isValidEmailAddress)
                return BadRequest("Please enter valid email address.");
            
            try
            {
                await BvHelper.SendMeetingInquiryMail(sendMeetingInquiryMailViewModel);
                return Ok("Email had been sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SendEventInquiryEmail([FromBody]SendEventInquiryMailViewModel sendEventInquiryMailView)
        {
            if (String.IsNullOrWhiteSpace(sendEventInquiryMailView.Package) ||
                String.IsNullOrWhiteSpace(sendEventInquiryMailView.Name) ||
                String.IsNullOrWhiteSpace(sendEventInquiryMailView.Email) ||
                String.IsNullOrWhiteSpace(sendEventInquiryMailView.Phone))
                return BadRequest("Please enter all required fields.");

            var isValidEmailAddress = IsValidEmailAddress(sendEventInquiryMailView.Email);
            if (!isValidEmailAddress)
                return BadRequest("Please enter valid email address.");
            
            try
            {
                await BvHelper.SendEventInquiryMail(sendEventInquiryMailView);
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
            if (String.IsNullOrWhiteSpace(sendTourInquiryMailViewModel.Package) ||
                String.IsNullOrWhiteSpace(sendTourInquiryMailViewModel.Name) ||
                String.IsNullOrWhiteSpace(sendTourInquiryMailViewModel.Email) ||
                String.IsNullOrWhiteSpace(sendTourInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var isValidEmailAddress = IsValidEmailAddress(sendTourInquiryMailViewModel.Email);
            if (!isValidEmailAddress)
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
            if (String.IsNullOrWhiteSpace(sendMiceInquiryMailViewModel.Package) ||
                String.IsNullOrWhiteSpace(sendMiceInquiryMailViewModel.Name) ||
                String.IsNullOrWhiteSpace(sendMiceInquiryMailViewModel.Email) ||
                String.IsNullOrWhiteSpace(sendMiceInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var isValidEmailAddress = IsValidEmailAddress(sendMiceInquiryMailViewModel.Email);
            if (!isValidEmailAddress)
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
            if (String.IsNullOrWhiteSpace(sendDiningInquiryMailViewModel.Name) ||
                String.IsNullOrWhiteSpace(sendDiningInquiryMailViewModel.Email) ||
                String.IsNullOrWhiteSpace(sendDiningInquiryMailViewModel.Phone))
                return BadRequest("Please enter all required fields.");

            var isValidEmailAddress = IsValidEmailAddress(sendDiningInquiryMailViewModel.Email);
            if (!isValidEmailAddress)
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