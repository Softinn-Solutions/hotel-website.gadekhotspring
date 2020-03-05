using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbunLuxuryVillas.ViewModels;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Softinn.EntityModels;
using Softinn.EntityModels.ViewModel;

namespace EmbunLuxuryVillas.Helpers
{
    public class LiteDbHelper
    {
        public string dbPath = $"database.db";

        public FullHotelViewModel GetFullHotelViewModel()
        {
            var hotel = GetHotel();

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

            var mices = GetMices().OrderBy(m => m.Number);
            var miceCallToActionTypes = GetMiceCallToActionTypes();

            var googleReviews = GetGoogleReviews(dbPath);
            var customReviews = GetCustomReviews(dbPath);

            var viewModel = new FullHotelViewModel()
            {
                Hotel = hotel,
                State = state,
                Country = country,

                Promotions = (from promotion in promotions
                              select new PromotionViewModel(promotion)
                              {
                                  IssuerType = issuerTypes.FirstOrDefault(it => it.Id == promotion.IssuerTypeId),
                                  DiscountType = discountTypes.FirstOrDefault(dt => dt.Id == promotion.DiscountTypeId),
                                  PromotionStatus = promotionStatuses.FirstOrDefault(ps => ps.Id == promotion.PromotionStatusId),
                              }).ToList(),

                PromotionalEvents = (from promotionalEvent in promotionalEvents
                                     select new PromotionalEventViewModel(promotionalEvent)
                                     {
                                         Photos = (from photo in photos.Where(p => p.PromotionalEventId == promotionalEvent.Id)
                                                   select new PhotoViewModel(photo)).ToList()
                                     }).ToList(),

                TourPackages = (from tourPackage in tourPackages
                                select new TourPackageViewModel(tourPackage)
                                {
                                    TourPackageCallToActionType = new TourPackageCallToActionTypeViewModel(tourPackageCallToActionTypes.FirstOrDefault(at => at.Id == tourPackage.TourPackageCallToActionTypeId)),
                                    Photos = (from photo in photos.Where(p => p.TourPackageId == tourPackage.Id)
                                              select new PhotoViewModel(photo)).ToList()
                                }).ToList(),

                Mices = (from mice in mices
                         select new MiceViewModel(mice)
                         {
                             MiceCallToActionType = new MiceCallToActionTypeViewModel(miceCallToActionTypes.FirstOrDefault(at => at.Id == mice.MiceCallToActionTypeId)),
                             Photos = (from photo in photos.Where(p => p.MiceId == mice.Id)
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
                                              select new RackRateViewModel(rackRate)
                                              {
                                                  RackRateType = new RackRateTypeViewModel(rackRateTypes.FirstOrDefault(rrt => rrt.Id == rackRate.RackRateTypeId))
                                              }).ToList(),
                                 BedRelationship = (from bedRelationship in bedRelationships.Where(br => br.Id == roomType.BedRelationshipId)
                                                    select new BedRelationshipViewModel(bedRelationship)
                                                    {
                                                        Beds = beds.Where(b => b.BedRelationshipId == bedRelationship.Id).ToList(),
                                                        RelationshipType = relationshipTypes.FirstOrDefault(rt => rt.Id == bedRelationship.RelationshipTypeId)
                                                    }).FirstOrDefault(),
                                 Photos = (from photo in photos.Where(p => p.RoomTypeId == roomType.Id)
                                           select new PhotoViewModel(photo)).ToList()
                             }).ToList(),

                Policies = policies.ToList(),

                Attractions = (from attraction in attractions
                               select new AttractionViewModel(attraction)
                               {
                                   AttractionType = new AttractionTypeViewModel(attractionTypes.FirstOrDefault(at => at.Id == attraction.AttractionTypeId)),
                                   Photos = (from photo in photos.Where(p => p.AttractionId == attraction.Id)
                                             select new PhotoViewModel(photo)).ToList()
                               }).ToList(),

                Photos = (from photo in photos
                          select new PhotoViewModel(photo)
                          {
                              PhotoType = new PhotoTypeViewModel(photoTypes.FirstOrDefault(pt => pt.Id == photo.PhotoTypeId)),
                              Attraction = photo.AttractionId != null && attractions.Any(a => a.Id == photo.AttractionId) ? new AttractionViewModel(attractions.FirstOrDefault(a => a.Id == photo.AttractionId)) : new AttractionViewModel(),
                              PromotionalEvent = photo.PromotionalEventId != null && promotionalEvents.Any(p => p.Id == photo.PromotionalEventId) ? new PromotionalEventViewModel(promotionalEvents.FirstOrDefault(p => p.Id == photo.PromotionalEventId)) : new PromotionalEventViewModel(),
                              TourPackage = photo.TourPackageId != null && tourPackages.Any(p => p.Id == photo.TourPackageId) ? new TourPackageViewModel(tourPackages.FirstOrDefault(p => p.Id == photo.TourPackageId)) : new TourPackageViewModel(),
                              Mice = photo.MiceId != null && mices.Any(p => p.Id == photo.MiceId) ? new MiceViewModel(mices.FirstOrDefault(p => p.Id == photo.MiceId)) : new MiceViewModel(),
                              RoomType = photo.RoomTypeId != null && roomTypes.Any(rt => rt.Id == photo.RoomTypeId) ? new RoomTypeViewModel(roomTypes.FirstOrDefault(rt => rt.Id == photo.RoomTypeId)) : new RoomTypeViewModel()
                          }).ToList()
            };

            return viewModel;
        }

        public HotelViewModel GetHotel()
        {
            HotelViewModel hotel;

            using (var db = new LiteRepository(dbPath))
            {
                hotel = db.Query<HotelViewModel>().FirstOrDefault();
            }

            return hotel;
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
                state = db.Query<StateViewModel>().FirstOrDefault();
            }

            return state;
        }

        public CountryViewModel GetCountry()
        {
            CountryViewModel country;

            using (var db = new LiteRepository(dbPath))
            {
                country = db.Query<CountryViewModel>().FirstOrDefault();
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

        public List<MiceViewModel> GetMices()
        {
            List<MiceViewModel> mices;

            using (var db = new LiteRepository(dbPath))
            {
                mices = db.Query<MiceViewModel>().ToList();
            }

            return mices;
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

        public List<MiceCallToActionTypeViewModel> GetMiceCallToActionTypes()
        {
            List<MiceCallToActionTypeViewModel> miceCallToActionTypes;

            using (var db = new LiteRepository(dbPath))
            {
                miceCallToActionTypes = db.Query<MiceCallToActionTypeViewModel>().ToList();
            }

            return miceCallToActionTypes;
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
    }
}
