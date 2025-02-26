using GadekHotspring.ViewModels;
using LiteDB;
using Softinn.EntityModels.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace GadekHotspring.Helpers
{
    public class LiteDbHelper
    {
        public string dbPath = $"database.db";

        public FullHotelViewModel GetFullHotelViewModel()
        {
            var hotel = GetHotel();
            var cmsSetting = GetCmsSetting();

            var promotions = GetPromotions();
            var promotionStatuses = GetPromotionStatuses();
            var issuerTypes = GetIssuerTypes();
            var discountTypes = GetDiscountTypes();

            var promotionalEvents = GetPromotionalEvents().OrderBy(pe => pe.Number);

            var roomTypes = GetRoomTypes().OrderBy(rt => rt.Number);
            var rackRates = GetRackRates();
            var rackRateTypes = GetRackRateTypes();
            var bedRelationships = GetBedRelationships();
            var relationshipTypes = GetRelationshipTypes();
            var beds = GetBeds();

            var policies = GetPolicies().OrderBy(p => p.Number);

            var attractions = GetAttractions();
            var attractionTypes = GetAttractionTypes();

            var photos = GetPhotos().OrderBy(p => p.Number);
            var photoTypes = GetPhotoTypes();

            var state = GetState();
            var country = GetCountry();

            var tourPackages = GetTourPackages().OrderBy(tp => tp.Number);
            var tourPackageCallToActionTypes = GetTourPackageCallToActionTypes();

            var googleReviews = GetGoogleReviews(dbPath);
            var customReviews = GetCustomReviews(dbPath);

            var blogs = GetBlogs();

            var customHtmlTags = GetCustomHtmlTags(dbPath);

            var customPrivacyPolicies = GetCustomPrivacyPolicies(dbPath);

            var meetings = GetMeetings();
            var meetingCtas = GetMeetingCallToActionTypes();
            var events = GetEvents();
            var eventsCtas = GetEventCallToActionTypes();

            var viewModel = new FullHotelViewModel()
            {
                Hotel = hotel,
                CMSSetting = cmsSetting,
                State = state,
                Country = country,

                Promotions = (from promotion in promotions
                              let issuerType = issuerTypes.FirstOrDefault(it => it.Id == promotion.IssuerTypeId)
                              let discountType = discountTypes.FirstOrDefault(dt => dt.Id == promotion.DiscountTypeId)
                              let promotionStatus = promotionStatuses.FirstOrDefault(ps => ps.Id == promotion.PromotionStatusId)
                              select new PromotionViewModel(promotion)
                              {
                                  IssuerType = issuerType ?? new IssuerTypeViewModel(),
                                  DiscountType = discountType ?? new DiscountTypeViewModel(),
                                  PromotionStatus = promotionStatus ?? new PromotionStatusViewModel()
                              }).ToList(),

                PromotionalEvents = (from promotionalEvent in promotionalEvents
                                     select new PromotionalEventViewModel(promotionalEvent)
                                     {
                                         Photos = (from photo in photos.Where(p => p.PromotionalEventId == promotionalEvent.Id)
                                                   select new PhotoViewModel(photo)).ToList()
                                     }).ToList(),

                TourPackages = (from tourPackage in tourPackages
                                let tourPackageCallToActionType =
                                    tourPackageCallToActionTypes.FirstOrDefault(at =>
                                        at.Id == tourPackage.TourPackageCallToActionTypeId)
                                select new TourPackageViewModel(tourPackage)
                                {
                                    TourPackageCallToActionType = tourPackageCallToActionType != null
                                        ? new TourPackageCallToActionTypeViewModel(tourPackageCallToActionType)
                                        : new TourPackageCallToActionTypeViewModel(),
                                    Photos = (from photo in photos.Where(p => p.TourPackageId == tourPackage.Id)
                                              select new PhotoViewModel(photo)).ToList()
                                }).ToList(),

                GoogleReviews = (from googleReview in googleReviews.Where(g => g.HotelId == hotel.Id)
                                 select new GoogleReviewViewModel(googleReview)).ToList(),
                CustomReviews = (from customReview in customReviews.Where(c => c.HotelId == hotel.Id)
                                 select new CustomReviewViewModel(customReview)).ToList(),

                RoomTypes = (from roomType in roomTypes
                             select new RoomTypeViewModel(roomType)
                             {
                                 RackRates = (from rackRate in rackRates.Where(rr => rr.RoomTypeId == roomType.Id)
                                              let rackRateType = rackRateTypes.FirstOrDefault(rrt => rrt.Id == rackRate.RackRateTypeId)
                                              select new RackRateViewModel(rackRate)
                                              {
                                                  RackRateType = rackRateType != null
                                                      ? new RackRateTypeViewModel(rackRateType)
                                                      : new RackRateTypeViewModel()
                                              }).ToList(),
                                 BedRelationship =
                                     (from bedRelationship in bedRelationships.Where(br => br.Id == roomType.BedRelationshipId)
                                      let relationshipType =
                                 relationshipTypes.FirstOrDefault(rt => rt.Id == bedRelationship.RelationshipTypeId)
                                      select new BedRelationshipViewModel(bedRelationship)
                                      {
                                          Beds = beds.Where(b => b.BedRelationshipId == bedRelationship.Id).ToList(),
                                          RelationshipType = relationshipType != null
                                     ? new RelationshipTypeViewModel(relationshipType)
                                     : new RelationshipTypeViewModel()
                                      }).FirstOrDefault() ?? new BedRelationshipViewModel(),
                                 Photos = (from photo in photos.Where(p => p.RoomTypeId == roomType.Id)
                                           select new PhotoViewModel(photo)).ToList()
                             }).ToList(),

                Policies = policies.ToList(),

                Attractions = (from attraction in attractions
                               let attractionType = attractionTypes.FirstOrDefault(at => at.Id == attraction.AttractionTypeId)
                               select new AttractionViewModel(attraction)
                               {
                                   AttractionType = attractionType != null
                                       ? new AttractionTypeViewModel(attractionType)
                                       : new AttractionTypeViewModel(),
                                   Photos = (from photo in photos.Where(p => p.AttractionId == attraction.Id)
                                             select new PhotoViewModel(photo)).ToList()
                               }).ToList(),

                Photos = (from photo in photos
                          let photoType = photoTypes.FirstOrDefault(pt => pt.Id == photo.PhotoTypeId)
                          select new PhotoViewModel(photo)
                          {
                              PhotoType = photoType != null
                                  ? new PhotoTypeViewModel(photoType)
                                  : new PhotoTypeViewModel(),
                              Attraction = photo.AttractionId != null && attractions.Any(a => a.Id == photo.AttractionId)
                                  ? new AttractionViewModel(attractions.FirstOrDefault(a => a.Id == photo.AttractionId))
                                  : new AttractionViewModel(),
                              PromotionalEvent =
                                  photo.PromotionalEventId != null &&
                                  promotionalEvents.Any(p => p.Id == photo.PromotionalEventId)
                                      ? new PromotionalEventViewModel(
                                          promotionalEvents.FirstOrDefault(p => p.Id == photo.PromotionalEventId))
                                      : new PromotionalEventViewModel(),
                              TourPackage = photo.TourPackageId != null && tourPackages.Any(p => p.Id == photo.TourPackageId)
                                  ? new TourPackageViewModel(tourPackages.FirstOrDefault(p => p.Id == photo.TourPackageId))
                                  : new TourPackageViewModel(),
                              RoomType = photo.RoomTypeId != null && roomTypes.Any(rt => rt.Id == photo.RoomTypeId)
                                  ? new RoomTypeViewModel(roomTypes.FirstOrDefault(rt => rt.Id == photo.RoomTypeId))
                                  : new RoomTypeViewModel()
                          }).ToList(),

                Blogs = blogs.ToList(),

                HeadCustomHtmlTags = (from customHtmlTag in customHtmlTags
                                      where customHtmlTag.IsEnabled == true
                                      where customHtmlTag.TagLocation == "head"
                                      orderby customHtmlTag.Number
                                      select customHtmlTag).ToList(),

                BodyCustomHtmlTags = (from customHtmlTag in customHtmlTags
                                      where customHtmlTag.IsEnabled == true
                                      where customHtmlTag.TagLocation == "body"
                                      orderby customHtmlTag.Number
                                      select customHtmlTag).ToList(),

                CustomPrivacyPolicies = customPrivacyPolicies,

                Meetings = (from meeting in meetings
                            let meetingCallToActionType =
                                meetingCtas.FirstOrDefault(at => at.Id == meeting.MeetingCallToActionTypeId)
                            select new MeetingViewModel(meeting)
                            {
                                MeetingCallToActionType = meeting.MeetingCallToActionTypeId.HasValue &&
                                                          meetingCallToActionType != null
                                    ? new MeetingCallToActionTypeViewModel(meetingCallToActionType)
                                    : new MeetingCallToActionTypeViewModel(),
                                Photos = (from photo in photos.Where(p => p.MeetingId == meeting.Id)
                                          select new PhotoViewModel(photo)).ToList()
                            }).ToList(),

                Events = (from e in events
                          let eventCallToActionType = eventsCtas.FirstOrDefault(at => at.Id == e.EventCallToActionTypeId)
                          select new EventViewModel(e)
                          {
                              EventCallToActionType = e.EventCallToActionTypeId.HasValue && eventCallToActionType != null
                                  ? new EventCallToActionTypeViewModel(eventCallToActionType)
                                  : new EventCallToActionTypeViewModel(),
                              Photos = (from photo in photos.Where(p => p.EventId == e.Id)
                                        select new PhotoViewModel(photo)).ToList()
                          }).ToList(),
            };

            return viewModel;
        }

        public HotelViewModel GetHotel()
        {
            HotelViewModel hotel;

            using (var db = new LiteRepository(dbPath))
            {
                hotel = db.Query<HotelViewModel>().FirstOrDefault() ?? new HotelViewModel();
            }

            return hotel;
        }

        public CMSSettingViewModel GetCmsSetting()
        {
            CMSSettingViewModel cmsSetting;

            using (var db = new LiteRepository(dbPath))
            {
                cmsSetting = db.Query<CMSSettingViewModel>().FirstOrDefault() ?? new CMSSettingViewModel();
            }

            return cmsSetting;
        }

        public List<PromotionViewModel> GetPromotions()
        {
            List<PromotionViewModel> promotions;

            using (var db = new LiteRepository(dbPath))
            {
                promotions = db.Query<PromotionViewModel>().ToList();
            }

            return promotions;
        }


        public List<PromotionViewModel> GetExtranetPromotions()
        {
            List<PromotionViewModel> promotions;

            using (var db = new LiteRepository(dbPath))
            {
                promotions = db.Query<PromotionViewModel>().Where(p => p.IssuerName != "CMS").ToList();
            }

            return promotions;
        }

        public List<PromotionStatusViewModel> GetPromotionStatuses()
        {
            List<PromotionStatusViewModel> promotionStatuses;

            using (var db = new LiteRepository(dbPath))
            {
                promotionStatuses = db.Query<PromotionStatusViewModel>().ToList();
            }

            return promotionStatuses;
        }

        public List<IssuerTypeViewModel> GetIssuerTypes()
        {
            List<IssuerTypeViewModel> issuerTypes;

            using (var db = new LiteRepository(dbPath))
            {
                issuerTypes = db.Query<IssuerTypeViewModel>().ToList();
            }

            return issuerTypes;
        }

        public List<DiscountTypeViewModel> GetDiscountTypes()
        {
            List<DiscountTypeViewModel> discountTypes;

            using (var db = new LiteRepository(dbPath))
            {
                discountTypes = db.Query<DiscountTypeViewModel>().ToList();
            }

            return discountTypes;
        }

        public List<RoomTypeViewModel> GetRoomTypes()
        {
            List<RoomTypeViewModel> roomTypes;

            using (var db = new LiteRepository(dbPath))
            {
                roomTypes = db.Query<RoomTypeViewModel>().ToList();
            }

            return roomTypes;
        }

        public List<RackRateViewModel> GetRackRates()
        {
            List<RackRateViewModel> rackRates;

            using (var db = new LiteRepository(dbPath))
            {
                rackRates = db.Query<RackRateViewModel>().ToList();
            }

            return rackRates;
        }

        public List<RackRateTypeViewModel> GetRackRateTypes()
        {
            List<RackRateTypeViewModel> rackRateTypes;

            using (var db = new LiteRepository(dbPath))
            {
                rackRateTypes = db.Query<RackRateTypeViewModel>().ToList();
            }

            return rackRateTypes;
        }

        public List<BedRelationshipViewModel> GetBedRelationships()
        {
            List<BedRelationshipViewModel> bedRelationships;

            using (var db = new LiteRepository(dbPath))
            {
                bedRelationships = db.Query<BedRelationshipViewModel>().ToList();
            }

            return bedRelationships;
        }

        public List<RelationshipTypeViewModel> GetRelationshipTypes()
        {
            List<RelationshipTypeViewModel> relationshipTypes;

            using (var db = new LiteRepository(dbPath))
            {
                relationshipTypes = db.Query<RelationshipTypeViewModel>().ToList();
            }

            return relationshipTypes;
        }

        public List<BedViewModel> GetBeds()
        {
            List<BedViewModel> beds;

            using (var db = new LiteRepository(dbPath))
            {
                beds = db.Query<BedViewModel>().ToList();
            }

            return beds;
        }

        public List<PolicyViewModel> GetPolicies()
        {
            List<PolicyViewModel> policies;

            using (var db = new LiteRepository(dbPath))
            {
                policies = db.Query<PolicyViewModel>().ToList();
            }

            return policies;
        }

        public List<AttractionViewModel> GetAttractions()
        {
            List<AttractionViewModel> attractions;

            using (var db = new LiteRepository(dbPath))
            {
                attractions = db.Query<AttractionViewModel>().ToList();
            }

            return attractions;
        }

        public List<AttractionTypeViewModel> GetAttractionTypes()
        {
            List<AttractionTypeViewModel> attractionTypes;

            using (var db = new LiteRepository(dbPath))
            {
                attractionTypes = db.Query<AttractionTypeViewModel>().ToList();
            }

            return attractionTypes;
        }

        public List<PhotoViewModel> GetPhotos()
        {
            List<PhotoViewModel> photos;

            using (var db = new LiteRepository(dbPath))
            {
                photos = db.Query<PhotoViewModel>().ToList();
            }

            return photos;
        }

        public StateViewModel GetState()
        {
            StateViewModel state;

            using (var db = new LiteRepository(dbPath))
            {
                state = db.Query<StateViewModel>().FirstOrDefault() ?? new StateViewModel();
            }

            return state;
        }

        public CountryViewModel GetCountry()
        {
            CountryViewModel country;

            using (var db = new LiteRepository(dbPath))
            {
                country = db.Query<CountryViewModel>().FirstOrDefault() ?? new CountryViewModel();
            }

            return country;
        }

        public List<PhotoTypeViewModel> GetPhotoTypes()
        {
            List<PhotoTypeViewModel> photoTypes;

            using (var db = new LiteRepository(dbPath))
            {
                photoTypes = db.Query<PhotoTypeViewModel>().ToList();
            }

            return photoTypes;
        }

        public List<PhotoViewModel> GetGalleryPhotos()
        {
            List<PhotoViewModel> photos;

            using (var db = new LiteRepository(dbPath))
            {
                photos = db.Query<PhotoViewModel>().ToList();
            }

            return photos;
        }

        public List<PromotionalEventViewModel> GetPromotionalEvents()
        {
            List<PromotionalEventViewModel> promotionalEvents;

            using (var db = new LiteRepository(dbPath))
            {
                promotionalEvents = db.Query<PromotionalEventViewModel>().ToList();
            }

            return promotionalEvents;
        }

        public List<TourPackageViewModel> GetTourPackages()
        {
            List<TourPackageViewModel> tourPackages;

            using (var db = new LiteRepository(dbPath))
            {
                tourPackages = db.Query<TourPackageViewModel>().ToList();
            }

            return tourPackages;
        }

        public List<TourPackageCallToActionTypeViewModel> GetTourPackageCallToActionTypes()
        {
            List<TourPackageCallToActionTypeViewModel> tourPackageCallToActionTypes;

            using (var db = new LiteRepository(dbPath))
            {
                tourPackageCallToActionTypes = db.Query<TourPackageCallToActionTypeViewModel>().ToList();
            }

            return tourPackageCallToActionTypes;
        }

        public static List<GoogleReviewViewModel> GetGoogleReviews(string dbPath)
        {
            List<GoogleReviewViewModel> googleReviews;

            using (var db = new LiteRepository(dbPath))
            {
                googleReviews = db.Query<GoogleReviewViewModel>().ToList();
            }

            return googleReviews;
        }

        public static List<CustomReviewViewModel> GetCustomReviews(string dbPath)
        {
            List<CustomReviewViewModel> customReviews;

            using (var db = new LiteRepository(dbPath))
            {
                customReviews = db.Query<CustomReviewViewModel>().ToList();
            }

            return customReviews;
        }

        public List<BlogViewModel> GetBlogs()
        {
            List<BlogViewModel> blogs;

            using (var db = new LiteRepository(dbPath))
            {
                blogs = db.Query<BlogViewModel>().ToList();
            }

            return blogs;
        }

        public static List<CustomHtmlTagViewModel> GetCustomHtmlTags(string dbPath)
        {
            List<CustomHtmlTagViewModel> htmlTags;

            using (var db = new LiteRepository(dbPath))
            {
                htmlTags = db.Query<CustomHtmlTagViewModel>().ToList();
            }

            return htmlTags;
        }

        public static List<CustomPrivacyPolicyViewModel> GetCustomPrivacyPolicies(string dbPath)
        {
            List<CustomPrivacyPolicyViewModel> privacyPolicies;

            using (var db = new LiteRepository(dbPath))
            {
                privacyPolicies = db.Query<CustomPrivacyPolicyViewModel>().ToList();
            }

            return privacyPolicies;
        }

        public List<MeetingViewModel> GetMeetings()
        {
            List<MeetingViewModel> meetings;

            using (var db = new LiteRepository(dbPath))
            {
                meetings = db.Query<MeetingViewModel>().ToList();
            }

            return meetings.ToList();
        }

        public List<MeetingCallToActionTypeViewModel> GetMeetingCallToActionTypes()
        {
            List<MeetingCallToActionTypeViewModel> meetingCallToActionTypes;

            using (var db = new LiteRepository(dbPath))
            {
                meetingCallToActionTypes = db.Query<MeetingCallToActionTypeViewModel>().ToList();
            }

            return meetingCallToActionTypes;
        }

        public List<EventViewModel> GetEvents()
        {
            List<EventViewModel> events;

            using (var db = new LiteRepository(dbPath))
            {
                events = db.Query<EventViewModel>().ToList();
            }

            return events;
        }

        public List<EventCallToActionTypeViewModel> GetEventCallToActionTypes()
        {
            List<EventCallToActionTypeViewModel> eventsCallToActionTypes;

            using (var db = new LiteRepository(dbPath))
            {
                eventsCallToActionTypes = db.Query<EventCallToActionTypeViewModel>().ToList();
            }

            return eventsCallToActionTypes;
        }
    }
}
