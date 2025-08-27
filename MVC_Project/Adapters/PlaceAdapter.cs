using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;

using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Adapters
{
    public class PlaceAdapter : IPlaceAdapter
    {
        private readonly IPlaceBusinessLogicContract _placeBusinessLogicContract;

        private readonly ILogger _logger;

        private readonly Mapper _mapper;

        public PlaceAdapter(IPlaceBusinessLogicContract placeBusinessLogicContract, ILogger logger)
        {
            _placeBusinessLogicContract = placeBusinessLogicContract;
            _logger = logger;
            var loggerFactory = NullLoggerFactory.Instance;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PlaceBindingModel, PlaceDataModel>();
                cfg.CreateMap<PlaceDataModel, PlaceViewModel>();
                cfg.CreateMap<PlaceDataModel, PlaceBindingModel>();
            }, loggerFactory);
            _mapper = new Mapper(config);
        }

        public void ChangePlaceInfo(PlaceBindingModel model)
        {
            try
            {
                _placeBusinessLogicContract.UpdatePlace(_mapper.Map<PlaceDataModel>(model));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving animal with data.", ex);
            }
        }

        public PlaceBindingModel GetElement(string creatorId, string data)
        {
            try
            {
                return _mapper.Map<PlaceBindingModel>(_placeBusinessLogicContract.GetPlaceByData(creatorId, data));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving animal with data {data}.", ex);
            }
        }

        public List<PlaceViewModel> GetList(string? creatorId = null)
        {
            try
            {
                return [.. _placeBusinessLogicContract.GetAllPlaces(creatorId).Select(x => _mapper.Map<PlaceViewModel>(x))];
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving excursion list.", ex);
            }
        }

        public List<PlaceViewModel> GetListByGroup(string creatorId, string groupId)
        {
            try
            {
                return [.. _placeBusinessLogicContract.GetAllPlacesByGroup(creatorId, groupId).Select(x => _mapper.Map<PlaceViewModel>(x))];
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving excursion list.", ex);
            }
        }

        public void RegisterPlace(PlaceBindingModel model)
        {
            try
            {
                _placeBusinessLogicContract.InsertPlace(_mapper.Map<PlaceDataModel>(model));
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving excursion list.", ex);
            }
        }

        public void RemovePlace(string creatorId, string id)
        {
            try
            {
                _placeBusinessLogicContract.DeletePlace(creatorId, id);
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
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");

            }
        }
    }
}
