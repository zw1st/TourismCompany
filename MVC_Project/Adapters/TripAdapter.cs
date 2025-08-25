using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using IvanSusaninProject_Database.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC_Project.Adapters
{
    public class TripAdapter : ITripAdapter
    {
        private readonly ITripBusinessLogicContract _tripBusinessLogicContract;
        private readonly IPlaceBusinessLogicContract _placeBusinessLogicContract;
        private readonly IGuideBusinessLogicsContract _guideBusinessLogicsContract;
        private readonly ITripPlaceBusinessLogicContract _tripPlaceBusinessLogicContract;
        private readonly ITripGuideBusinessLogicContract _tripGuideBusinessLogicContract;

        private readonly ILogger _logger;

        private readonly Mapper _mapper;

        public TripAdapter(ITripBusinessLogicContract workerBusinessLogicContract, ILogger<TripAdapter> logger, IPlaceBusinessLogicContract placeBusinessLogicContract, IGuideBusinessLogicsContract guideBusinessLogicsContract, ITripPlaceBusinessLogicContract tripPlaceBusinessLogicContract, ITripGuideBusinessLogicContract tripGuideBusinessLogicContract)
        {
            _tripBusinessLogicContract = workerBusinessLogicContract;
            _logger = logger;
            var loggerFactory = NullLoggerFactory.Instance;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TripBindingModel, TripDataModel>()
            .ConstructUsing(src => new TripDataModel(
                src.Id,
                src.StartCity,
                src.EndCity,
                src.TripDate,
                src.Duration,
                src.UserId,
                new List<TripPlaceDataModel>(), // пустые списки
                new List<TripGuideDataModel>()  // будут заполнены позже
            ))
            .ForMember(dest => dest.TripPlaces, opt => opt.Ignore()) // игнорируем при маппинге
            .ForMember(dest => dest.TripGuides, opt => opt.Ignore());
                cfg.CreateMap<TripDataModel, TripViewModel>();

            }, loggerFactory);
            _mapper = new Mapper(config);
            _placeBusinessLogicContract = placeBusinessLogicContract;
            _guideBusinessLogicsContract = guideBusinessLogicsContract;
            _tripPlaceBusinessLogicContract = tripPlaceBusinessLogicContract;
            _tripGuideBusinessLogicContract = tripGuideBusinessLogicContract;
        }

        public void ChangeTripInfo(TripBindingModel model)
        {
            try
            {
                _tripBusinessLogicContract.UpdateTrip(_mapper.Map<TripDataModel>(model));
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
            }
            catch (MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogError(ex, "ElementNotFoundException");
            }
            catch (ElementExistsException ex)
            {
                _logger.LogError(ex, "ElementExistsException");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
            }
        }

        public TripViewModel GetElement(string? creatorId, string id)
        {
            try
            {
                var trip = _tripBusinessLogicContract.GetTripById(creatorId, id);
                if (trip == null) return null;

                var viewModel = _mapper.Map<TripViewModel>(trip);
                viewModel.Places = _tripPlaceBusinessLogicContract.GetPlacesForTrip(id);
                viewModel.Guides = _tripGuideBusinessLogicContract.GetGuidesForTrip(id);
                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting tour details", ex);
            }
        }
        

        public List<TripViewModel> GetList(string creatorId)
        {
            try
            {
                return [.. _tripBusinessLogicContract.GetAllTrips(creatorId).Select(x => _mapper.Map<TripViewModel>(x))];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                throw new Exception("Error", ex);
            }
        }

        public List<TripViewModel> GetListByDate(string creatorId, DateTime tripDate)
        {
            try
            {
                return [.. _tripBusinessLogicContract.GetAllTripsByDate(creatorId, tripDate).Select(x => _mapper.Map<TripViewModel>(x))];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                throw new Exception("Error", ex);
            }
        }

        public List<TripViewModel> GetListByPeriod(string creatorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return [.. _tripBusinessLogicContract.GetAllTripsByPeriod(creatorId, fromDate, toDate).Select(x => _mapper.Map<TripViewModel>(x))];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                throw new Exception("Error", ex);
            }
        }

        public List<TripViewModel> GetListWithDetails()
        {
            try
            {
                var trips = _tripBusinessLogicContract.GetAllTrips(null);

                if (!trips.Any())
                    return new List<TripViewModel>();

                var tripIds = trips.Select(t => t.Id).ToList();

                var placesByTrip = new Dictionary<string, List<PlaceViewModel>>();
                foreach (var tripId in tripIds)
                {
                    var places = _tripPlaceBusinessLogicContract.GetPlacesForTrip(tripId);
                    placesByTrip[tripId] = places;
                }

                var guidesByTrip = new Dictionary<string, List<GuideViewModel>>();
                foreach (var tripId in tripIds)
                {
                    var guides = _tripGuideBusinessLogicContract.GetGuidesForTrip(tripId);
                    guidesByTrip[tripId] = guides;
                }

                var result = new List<TripViewModel>();

                foreach (var trip in trips)
                {
                    result.Add(new TripViewModel
                    {
                        Id = trip.Id,
                        StartCity = trip.StartCity,
                        EndCity = trip.EndCity,
                        TripDate = trip.TripDate,
                        Duration = trip.Duration,
                        Places = placesByTrip.TryGetValue(trip.Id, out var places) ? places : new List<PlaceViewModel>(),
                        Guides = guidesByTrip.TryGetValue(trip.Id, out var guides) ? guides : new List<GuideViewModel>()
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trips with details");
                throw new Exception("Failed to get trips with details", ex);
            }
        }

        public void RegisterTrip(TripBindingModel model)
        {
            try
            {
                _tripBusinessLogicContract.InsertTrip(_mapper.Map<TripDataModel>(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                throw new Exception("Error", ex);
            }
        }

        public void RegisterTripWithRelations(TripBindingModel model, List<string> placeIds, List<string> guideIds)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model), "Trip model cannot be null");

                var tripDataModel = _mapper.Map<TripDataModel>(model);

                _tripBusinessLogicContract.InsertTrip(tripDataModel);

                if (placeIds != null && placeIds.Any())
                {
                    foreach (var placeId in placeIds)
                    {
                        if (!string.IsNullOrEmpty(placeId))
                        {
                            try
                            {
                                _tripPlaceBusinessLogicContract.AddPlaceToTrip(tripDataModel.Id, placeId);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to add place {PlaceId} to trip {TripId}",
                                    placeId, tripDataModel.Id);
                            }
                        }
                    }
                }

                if (guideIds != null && guideIds.Any())
                {
                    foreach (var guideId in guideIds)
                    {
                        if (!string.IsNullOrEmpty(guideId))
                        {
                            try
                            {
                                _tripGuideBusinessLogicContract.AddGuideToTrip(tripDataModel.Id, guideId);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to add guide {GuideId} to trip {TripId}",
                                    guideId, tripDataModel.Id);
                            }
                        }
                    }
                }

                _logger.LogInformation("Successfully created trip {TripId} with {PlaceCount} places and {GuideCount} guides",
                    tripDataModel.Id,
                    placeIds?.Count ?? 0,
                    guideIds?.Count ?? 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating trip with relations");
                throw new Exception("Failed to create trip with relations", ex);
            }
        }
    }
}