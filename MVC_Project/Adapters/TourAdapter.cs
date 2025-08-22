using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;

namespace MVC_Project.Adapters;

public class TourAdapter : ITourAdapter
{
    private readonly ITourBusinessLogicsContract _tourBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public TourAdapter(ITourBusinessLogicsContract tourBusinessLogicContract, ILogger<TourAdapter> logger)
    {
        _tourBusinessLogicContract = tourBusinessLogicContract;
        _logger = logger;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TourBindingModel, TourDataModel>();
            cfg.CreateMap<TourDataModel, TourViewModel>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public TourOperationResponse GetElement(string creatorId, string data)
    {
        try
        {
            return TourOperationResponse.OK(_mapper.Map<TourViewModel>(_tourBusinessLogicContract.GetTourById(creatorId, data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return TourOperationResponse.BadRequest("Data is empty");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return TourOperationResponse.NotFound($"Not found element by data {data}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return TourOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return TourOperationResponse.InternalServerError(ex.Message);
        }
    }

    public TourOperationResponse GetList(string creatorId, DateTime date)
    {
        try
        {
            return TourOperationResponse.OK([.. _tourBusinessLogicContract.GetAllTours(creatorId, date).Select(x => _mapper.Map<TourViewModel>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return TourOperationResponse.NotFound("The list is not initialized");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return TourOperationResponse.InternalServerError($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return TourOperationResponse.InternalServerError(ex.Message);
        }
    }

    public TourOperationResponse RegisterTour(TourBindingModel tourModel)
    {
        try
        {
            _tourBusinessLogicContract.InsertTour(_mapper.Map<TourDataModel>(tourModel));
            return TourOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return TourOperationResponse.BadRequest("Data is empty");
        }
        catch (IvanSusaninProject_Contracts.Exceptions.MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            return TourOperationResponse.BadRequest($"Incorrect data transmitted: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return TourOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return TourOperationResponse.BadRequest($"Error while working with data storage: {ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return TourOperationResponse.InternalServerError(ex.Message);
        }
    }
}