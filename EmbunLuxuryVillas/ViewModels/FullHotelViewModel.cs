using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softinn.EntityModels.ViewModel;

namespace EmbunLuxuryVillas.ViewModels
{
    public class FullHotelViewModel
    {
        public HotelViewModel Hotel { get; set; }
        public CountryViewModel Country { get; set; }
        public StateViewModel State { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
        public List<PromotionViewModel> Promotions { get; set; }
        public List<PromotionalEventViewModel> PromotionalEvents { get; set; }
        public List<TourPackageViewModel> TourPackages { get; set; }
        public List<MiceViewModel> Mices { get; set; }
        public List<RoomTypeViewModel> RoomTypes { get; set; }
        public List<PolicyViewModel> Policies { get; set; }
        public List<AttractionViewModel> Attractions { get; set; }
        public List<GoogleReviewViewModel> GoogleReviews { get; set; }
        public List<CustomReviewViewModel> CustomReviews { get; set; }
        public List<BlogViewModel> Blogs { get; set; }
    }
}