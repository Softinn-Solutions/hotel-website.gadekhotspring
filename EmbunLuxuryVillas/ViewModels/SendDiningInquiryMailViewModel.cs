using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbunLuxuryVillas.ViewModels
{
    public class SendDiningInquiryMailViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateTime { get; set; }
        public string DiningOption { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public string Message { get; set; }
    }
}
