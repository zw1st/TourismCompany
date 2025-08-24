using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IGroupStorageContract
{
    List<GroupDataModel> GetList(string? creatorId = null);

    GroupDataModel? GetElementById(string? creatorId, string id);

    void AddElement(GroupDataModel element);

    void UpdateElement(GroupDataModel element);

    void DeleteElement(string creatorId, string id);
}
