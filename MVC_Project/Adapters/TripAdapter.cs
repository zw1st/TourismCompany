using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC_Project.Adapters
{
    public class TripAdapter : ITripAdapter
    {
        private readonly ITripBusinessLogicContract _tripBusinessLogicContract;

        private readonly ILogger _logger;

        private readonly Mapper _mapper;

        public TripAdapter(ITripBusinessLogicContract workerBusinessLogicContract, ILogger<TripAdapter> logger)
        {
            _tripBusinessLogicContract = workerBusinessLogicContract;
            _logger = logger;
            var loggerFactory = NullLoggerFactory.Instance;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TripBindingModel, TripDataModel>();
                cfg.CreateMap<TripDataModel, TripViewModel>();
            }, loggerFactory);
            _mapper = new Mapper(config);
        }

        public TripOperationResponse ChangeTripInfo(TripBindingModel model)
        {
            try
            {
                _tripBusinessLogicContract.UpdateTrip(_mapper.Map<TripDataModel>(model));
                return TripOperationResponse.NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return TripOperationResponse.BadRequest("Data is empty");
            }
            catch (IvanSusaninProject_Contracts.Exceptions.MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return TripOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogError(ex, "ElementNotFoundException");
                return TripOperationResponse.BadRequest($"Not found element by Id {model.Id} ");
            }
            catch (ElementExistsException ex)
            {
                _logger.LogError(ex, "ElementExistsException");
                return TripOperationResponse.BadRequest(ex.Message);
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return TripOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                TripOperationResponse.InternalServerError(ex.Message);
            }
        }

        public TripOperationResponse GetElement(string creatorId, string id)
        {
            try
            {
                return TripOperationResponse.OK(_mapper.Map<TripViewModel>(_tripBusinessLogicContract.GetTripById(creatorId, id)));
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return TripOperationResponse.BadRequest("Data is empty");
            }
            catch (IvanSusaninProject_Contracts.Exceptions.MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return TripOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementNotFoundException ex)
            {
                _logger.LogError(ex, "ElementNotFoundException");
                return TripOperationResponse.NotFound($"Not found element by id {id} ");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return TripOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                TripOperationResponse.InternalServerError(ex.Message);
            }
        }

        public TripOperationResponse GetList(string creatorId)
        {
            try
            {
                return TripOperationResponse.OK([.. _tripBusinessLogicContract.GetAllTrips(creatorId).Select(x => _mapper.Map<TripViewModel>(x))]);
            }
            catch (NullListException)
            {
                _logger.LogError("NullListException");
                return TripOperationResponse.NotFound("The list is not initialized");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return TripOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                TripOperationResponse.InternalServerError(ex.Message);
            }
        }

        public TripOperationResponse GetListByDate(string creatorId, DateTime tripDate)
        {
            try
            {
                return TripOperationResponse.OK([.. _tripBusinessLogicContract.GetAllTripsByDate(creatorId, tripDate).Select(x => _mapper.Map<TripViewModel>(x))]);
            }
            catch (IncorrectDatesException ex)
            {
                _logger.LogError(ex, "IncorrectDatesException");
                return TripOperationResponse.BadRequest($"Incorrect dates: {ex.Message} ");
            }
            catch (NullListException)
            {
                _logger.LogError("NullListException");
                return TripOperationResponse.NotFound("The list is not initialized");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return TripOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                TripOperationResponse.InternalServerError(ex.Message);
            }
        }

        public TripOperationResponse GetListByPeriod(string creatorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return TripOperationResponse.OK([.. _tripBusinessLogicContract.GetAllTripsByPeriod(creatorId, fromDate, toDate).Select(x => _mapper.Map<TripViewModel>(x))]);
            }
            catch (IncorrectDatesException ex)
            {
                _logger.LogError(ex, "IncorrectDatesException");
                return TripOperationResponse.BadRequest($"Incorrect dates: {ex.Message} ");
            }
            catch (NullListException)
            {
                _logger.LogError("NullListException");
                return TripOperationResponse.NotFound("The list is not initialized");
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return TripOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                TripOperationResponse.InternalServerError(ex.Message);
            }
        }

        public TripOperationResponse RegisterTrip(TripBindingModel model)
        {
            try
            {
                _tripBusinessLogicContract.InsertTrip(_mapper.Map<TripDataModel>(model));
                return TripOperationResponse.NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "ArgumentNullException");
                return TripOperationResponse.BadRequest("Data is empty");
            }
            catch (IvanSusaninProject_Contracts.Exceptions.MyValidationException ex)
            {
                _logger.LogError(ex, "MyValidationException");
                return TripOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message} ");
            }
            catch (ElementExistsException ex)
            {
                _logger.LogError(ex, "ElementExistsException");
                return TripOperationResponse.BadRequest(ex.Message);
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex, "StorageException");
                return TripOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                return
                TripOperationResponse.InternalServerError(ex.Message);
            }
        }
    }
}