using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class TripBusinessLogicContract(ITripStorageContract tripStorageContract, ILogger logger) : ITripBusinessLogicContract
{
    private readonly ILogger _logger = logger;

    private readonly ITripStorageContract _tripStorageContract = tripStorageContract;

    public List<TripDataModel> GetAllTrips(string creatorId)
    {
        _logger.LogInformation("GetAllTrips params: {creatorId}", creatorId);
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        return _tripStorageContract.GetList(creatorId) ?? throw new NullListException();
    }

    public List<TripDataModel> GetAllTripsByDate(string creatorId, DateTime tripDate)
    {
        _logger.LogInformation("GetAllTripsByDate params: {creatorId}, {tripDate}", creatorId, tripDate);
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }

        return _tripStorageContract.GetList(creatorId, null, null, tripDate) ?? throw new NullListException();
    }

    public List<TripDataModel> GetAllTripsByPeriod(string creatorId, DateTime fromDate, DateTime toDate)
    {
        _logger.LogInformation("GetAllTripsByPeriod params: {creatorId}, {fromDate}, {toDate}", creatorId, fromDate, toDate);
        if (fromDate.IsDateNotOlder(toDate))
        {
            throw new IncorrectDatesException(fromDate, toDate);
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        return _tripStorageContract.GetList(creatorId, fromDate, toDate, null) ?? throw new NullListException();
    }

    public TripDataModel GetTripById(string creatorId, string id)
    {
        _logger.LogInformation("Get element by id: {creatorId}, {data}", creatorId, id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!id.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        return _tripStorageContract.GetElementById(creatorId, id) ?? throw new ElementNotFoundException(id);
    }

    public void InsertTrip(TripDataModel model)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _tripStorageContract.AddElement(model);
    }

    public void UpdateTrip(TripDataModel model)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _tripStorageContract.UpdElement(model);
    }
}