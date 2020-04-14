using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbunLuxuryVillas.ViewModels
{
    public class SendTourPackageInquiryMailViewModel
    {
        public string Package { get; set; }
        public string TravelDate { get; set; }
        public string Salutation { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
