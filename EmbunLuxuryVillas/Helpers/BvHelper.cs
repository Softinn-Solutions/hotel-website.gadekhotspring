using System;
using System.Text;
using System.Threading.Tasks;
using EmbunLuxuryVillas.ViewModels;
using HtmlAgilityPack;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmbunLuxuryVillas.Helpers
{
    public static class BvHelper
    {
        public static async Task SendMail(SendMailViewModel mailViewModel)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            var emailHtml = "Name : " + mailViewModel.Salutation + ". " + mailViewModel.Name +
                            "<br />Phone: " + mailViewModel.Phone + "<br />" +
                            "<br />Subject: " + mailViewModel.Subject + "<br />" +
                            "<br />Email: " + mailViewModel.Email + "<br />" +
                            "<br /><br />" + mailViewModel.Message;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marketing@mysoftinn.com", $"{hotelViewModel.DisplayName} Website Contact Form"),
                Subject = $"{hotelViewModel.DisplayName}, You have a new message from the website.",
                HtmlContent = emailHtml,
                ReplyTo = new EmailAddress(mailViewModel.Email)
            };
            msg.AddTo(new EmailAddress(hotelViewModel.DisplayEmail));
            msg.AddBcc(new EmailAddress("marketing@mysoftinn.com"));
            var response = await client.SendEmailAsync(msg);
        }

        public static async Task SendMeetingInquiryMail(SendMeetingInquiryMailViewModel sendMeetingInquiryMailViewModel)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            var emailHtml = "Package : " + sendMeetingInquiryMailViewModel.Package +
                            "<br />Event Date : " +
                            (IsValidDateTime(sendMeetingInquiryMailViewModel.EventDate)
                                ? sendMeetingInquiryMailViewModel.EventDate
                                : "Not Specified") +
                            "<br />No. of pax : " +
                            (sendMeetingInquiryMailViewModel.NoPax != 0
                                ? sendMeetingInquiryMailViewModel.NoPax.ToString()
                                : "Not Specified") +
                            "<br />Total budget : " +
                            (sendMeetingInquiryMailViewModel.Budget != 0
                                ? sendMeetingInquiryMailViewModel.Budget.ToString()
                                : "Not Specified") +
                            "<br />Name : " + sendMeetingInquiryMailViewModel.Name +
                            "<br />Company Name : " +
                            (!String.IsNullOrEmpty(sendMeetingInquiryMailViewModel.CompanyName)
                                ? sendMeetingInquiryMailViewModel.CompanyName
                                : "Not Specified") +
                            "<br />Phone: " + sendMeetingInquiryMailViewModel.Phone +
                            "<br />Email: " + sendMeetingInquiryMailViewModel.Email +
                            "<br /><br />" + sendMeetingInquiryMailViewModel.Message;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marketing@mysoftinn.com", $"{hotelViewModel.DisplayName} Hotel Website Inquiry"),
                Subject = $"{hotelViewModel.DisplayName}, you have a new meeting inquiry",
                HtmlContent = emailHtml,
                ReplyTo = new EmailAddress(sendMeetingInquiryMailViewModel.Email)
            };
            msg.AddTo(new EmailAddress(hotelViewModel.DisplayEmail));
            msg.AddBcc(new EmailAddress("marketing@mysoftinn.com"));
            await client.SendEmailAsync(msg);
        }

        public static async Task SendEventInquiryMail(SendEventInquiryMailViewModel sendEventInquiryMailViewModel)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            var emailHtml = "Package : " + sendEventInquiryMailViewModel.Package +
                            "<br />Name : " + sendEventInquiryMailViewModel.Name +
                            "<br />Company Name : " +
                            (!String.IsNullOrEmpty(sendEventInquiryMailViewModel.CompanyName)
                                ? sendEventInquiryMailViewModel.CompanyName
                                : "Not Specified") +
                            "<br />Phone: " + sendEventInquiryMailViewModel.Phone +
                            "<br />Email: " + sendEventInquiryMailViewModel.Email +
                            "<br /><br />" + sendEventInquiryMailViewModel.Message;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marketing@mysoftinn.com", $"{hotelViewModel.DisplayName} Hotel Website Inquiry"),
                Subject = $"{hotelViewModel.DisplayName}, you have a new event inquiry",
                HtmlContent = emailHtml,
                ReplyTo = new EmailAddress(sendEventInquiryMailViewModel.Email)
            };
            msg.AddTo(new EmailAddress(hotelViewModel.DisplayEmail));
            msg.AddBcc(new EmailAddress("marketing@mysoftinn.com"));
            await client.SendEmailAsync(msg);
        }

        public static string GetImageLinkFromStorage(string filePath)
        {
            return "//softinnstorage.blob.core.windows.net" + filePath;
        }

        public static bool IsValidDateTime(string txtDate)
        {
            return !txtDate.Contains("Invalid");
        }

        public static string ToSeoFriendly(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
                else if (Char.IsWhiteSpace(c) || c == '-')
                {
                    sb.Append('-');
                }
            }
            return sb.ToString();
        }

        public static CustomRoomViewModel GetAdditionalRoomInfo(string roomName)
        {
            var apartment3Bedroom = new CustomRoomViewModel()
            {
                BackgroundImage = "bg-8",
            };

            var apartment2Bedroom = new CustomRoomViewModel()
            {
                BackgroundImage = "bg-9",
            };

            var deluxeApartment2Bedroom = new CustomRoomViewModel()
            {
                BackgroundImage = "bg-10",
            };

            var apartment4Bedroom = new CustomRoomViewModel()
            {
                BackgroundImage = "bg-11",
            };

            switch (roomName)
            {
                case "3 Bedroom Apartment":
                    return apartment3Bedroom;
                case "2 Bedroom Apartment":
                    return apartment2Bedroom;
                case "Deluxe 2 Bedroom Apartment":
                    return deluxeApartment2Bedroom;
                case "4 Bedroom Apartment":
                    return apartment4Bedroom;
                default:
                    return new CustomRoomViewModel();
            }
        }

        public static async Task SendTourPackageInquiryMail(SendTourPackageInquiryMailViewModel tourPackageInquiryMailViewModel)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            var emailHtml = "Package : " + tourPackageInquiryMailViewModel.Package +
                            "<br />Travel Date : " + tourPackageInquiryMailViewModel.TravelDate +
                            "<br />Name : " + tourPackageInquiryMailViewModel.Salutation + ". " + tourPackageInquiryMailViewModel.Name +
                            "<br />Company Name : " + tourPackageInquiryMailViewModel.CompanyName +
                            "<br />Phone: " + tourPackageInquiryMailViewModel.Phone +
                            "<br />Email: " + tourPackageInquiryMailViewModel.Email +
                            "<br /><br />" + tourPackageInquiryMailViewModel.Message;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marketing@mysoftinn.com", $"{hotelViewModel.DisplayName} Tour Package Inquiry"),
                Subject = $"{hotelViewModel.DisplayName}, you have a new tour package inquiry",
                HtmlContent = emailHtml,
                ReplyTo = new EmailAddress(tourPackageInquiryMailViewModel.Email)
            };
            msg.AddTo(new EmailAddress(hotelViewModel.DisplayEmail));
            msg.AddBcc(new EmailAddress("marketing@mysoftinn.com"));

            var response = await client.SendEmailAsync(msg);
        }

        public static async Task SendMiceInquiryMail(SendMiceInquiryMailViewModel miceInquiryMailViewModel)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            var emailHtml = "Package : " + miceInquiryMailViewModel.Package +
                            "<br />Event Date : " + miceInquiryMailViewModel.EventDate +
                            "<br />No. of pax : " + miceInquiryMailViewModel.NoPax +
                            "<br />Total budget : " + miceInquiryMailViewModel.Budget +
                            "<br />Name : " + miceInquiryMailViewModel.Salutation + ". " + miceInquiryMailViewModel.Name +
                            "<br />Company Name : " + miceInquiryMailViewModel.CompanyName +
                            "<br />Phone: " + miceInquiryMailViewModel.Phone +
                            "<br />Email: " + miceInquiryMailViewModel.Email +
                            "<br /><br />" + miceInquiryMailViewModel.Message;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marketing@mysoftinn.com", $"{hotelViewModel.DisplayName} Meetings & Events Inquiry"),
                Subject = $"{hotelViewModel.DisplayName}, you have a new meetings & events inquiry",
                HtmlContent = emailHtml,
                ReplyTo = new EmailAddress(miceInquiryMailViewModel.Email)
            };
            msg.AddTo(new EmailAddress(hotelViewModel.DisplayEmail));
            msg.AddBcc(new EmailAddress("marketing@mysoftinn.com"));

            var response = await client.SendEmailAsync(msg);
        }

        public static async Task SendDiningInquiryMail(SendDiningInquiryMailViewModel sendDiningInquiryMailViewModel)
        {
            var liteDbHelper = new LiteDbHelper();
            var hotelViewModel = liteDbHelper.GetHotel();

            var emailHtml = "Name : " + sendDiningInquiryMailViewModel.Salutation + ". " + sendDiningInquiryMailViewModel.Name +
                            "<br />Phone: " + sendDiningInquiryMailViewModel.Phone +
                            "<br />Email: " + sendDiningInquiryMailViewModel.Email +
                            "<br />Date: " + sendDiningInquiryMailViewModel.DateTime +
                            "<br />Dining option: " + sendDiningInquiryMailViewModel.DiningOption +
                            "<br />Adults: " + sendDiningInquiryMailViewModel.Adults +
                            "<br />Children: " + sendDiningInquiryMailViewModel.Children +
                            "<br /><br />" + sendDiningInquiryMailViewModel.Message;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("marketing@mysoftinn.com", $"{hotelViewModel.DisplayName} Dining Reservations Inquiry"),
                Subject = $"{hotelViewModel.DisplayName}, you have a new dining reservations inquiry",
                HtmlContent = emailHtml,
                ReplyTo = new EmailAddress(sendDiningInquiryMailViewModel.Email)
            };
            msg.AddTo(new EmailAddress(hotelViewModel.DisplayEmail));
            msg.AddBcc(new EmailAddress("marketing@mysoftinn.com"));

            var response = await client.SendEmailAsync(msg);
        }

        public static string GetReadableText(string input)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);

            return doc.DocumentNode.InnerText;
        }
    }
}
