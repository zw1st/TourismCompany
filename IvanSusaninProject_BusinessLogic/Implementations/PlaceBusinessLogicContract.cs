using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class PlaceBusinessLogicContract(IPlaceStorageContract placeStorageContract, ILogger logger) : IPlaceBusinessLogicContract
{
    private readonly ILogger _logger = logger;

    private readonly IPlaceStorageContract _placeStorageContract = placeStorageContract;

    public void DeletePlace(string creatorId, string id)
    {
        _logger.LogInformation("Delete by id: {creatorId}, {id}", creatorId, id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (!id.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        _placeStorageContract.DelElement(creatorId, id);
    }

    public List<PlaceDataModel> GetAllPlaces(string? creatorId)
    {
        _logger.LogInformation("GetAllPlaces params: {creatorId}", creatorId);
        //if (!creatorId.IsGuid())
        //{
        //    throw new MyValidationException("Id is not a unique identifier");
        //}
        return _placeStorageContract.GetList(creatorId) ?? throw new NullListException();
    }

    public List<PlaceDataModel> GetAllPlacesByGroup(string creatorId, string groupId)
    {
        _logger.LogInformation("GetAllPlaces by group: {creatorId}, {groupId}", creatorId, groupId);
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        if (groupId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(groupId));
        }
        if (!groupId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        return _placeStorageContract.GetList(creatorId, groupId) ?? throw new NullListException();
    }

    public PlaceDataModel GetPlaceByData(string creatorId, string data)
    {
        _logger.LogInformation("Get element by data: {creatorId}, {data}", creatorId, data);
        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (!data.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        return _placeStorageContract.GetElementById(creatorId, data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertPlace(PlaceDataModel model)
    {
        _logger.LogInformation("New data: {json}",
        JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _placeStorageContract.AddElement(model);
    }

    public void UpdatePlace(PlaceDataModel model)
    {
        _logger.LogInformation("Update data: {json}",
        JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _placeStorageContract.UpdElement(model);
    }
}