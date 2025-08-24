using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class GroupBusinessLogicsContract(IGroupStorageContract groupStorageContract, IPlaceStorageContract placeStorageContract, ILogger logger) : IGroupBusinessLogicsContract
{
    IGroupStorageContract _groupStorageContract = groupStorageContract;
    IPlaceStorageContract _placeStorageContract = placeStorageContract;

    private readonly ILogger _logger = logger;

    public void DeleteGroup(string creatorId, string id)
    {
        _logger.LogInformation("Delete by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        if (!id.IsGuid())
        {
            throw new MyValidationException("Id is not a unique identifier");
        }
        _groupStorageContract.DeleteElement(creatorId, id);
    }

    public List<GroupDataModel> GetAllGroups(string? creatorId = null)
    {
        _logger.LogInformation("GetAllPosts params");
        return _groupStorageContract.GetList(creatorId) ?? throw new NullListException();
    }

    public GroupDataModel GetGroupById(string? creatorId, string id)
    {
        _logger.LogInformation("Get element by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        return _groupStorageContract.GetElementById(creatorId, id) ?? throw new ElementNotFoundException(id);
    }

    public void InsertGroup(GroupDataModel groupDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(groupDataModel));
        ArgumentNullException.ThrowIfNull(groupDataModel);
        _groupStorageContract.AddElement(groupDataModel);
    }

    public void LinkingGroupWithPlace(string creatorId, string groupId, string placeId)
    {
        if (string.IsNullOrEmpty(creatorId))
            throw new ArgumentNullException(nameof(creatorId));

        if (string.IsNullOrEmpty(groupId))
            throw new ArgumentNullException(nameof(groupId));

        if (string.IsNullOrEmpty(placeId))
            throw new ArgumentNullException(nameof(placeId));

        // Получаем место для посещения
        var place = _placeStorageContract.GetElementById(null, placeId);
        if (place == null)
            throw new ElementNotFoundException(placeId);

        // Проверяем существование группы
        var group = _groupStorageContract.GetElementById(creatorId, groupId);
        if (group == null)
        {
            // Если группа не существует, устанавливаем groupId в null
            place.GroupId = null;
        }
        else
        {
            // Если группа существует, устанавливаем связь
            place.GroupId = groupId;
        }

        // Обновляем место для посещения
        _placeStorageContract.UpdElement(place);
    }

    public void UpdateGroup(GroupDataModel groupDataModel)
    {
        _logger.LogInformation("Update data: {json}", JsonSerializer.Serialize(groupDataModel));
        ArgumentNullException.ThrowIfNull(groupDataModel);
        _groupStorageContract.UpdateElement(groupDataModel);
    }
}

