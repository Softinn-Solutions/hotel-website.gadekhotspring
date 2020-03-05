
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbunLuxuryVillas.ViewModels
{
    public class SendMiceInquiryMailViewModel
    {
        public string Package { get; set; }
        public string EventDate { get; set; }
        public int? NoPax { get; set; }
        public double? Budget { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
