using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IGroupBusinessLogicsContract
{
    List<GroupDataModel> GetAllGroups(string? creatorId = null);

    GroupDataModel GetGroupById(string? creatorId, string id);

    void InsertGroup(GroupDataModel groupDataModel);

    void UpdateGroup(GroupDataModel groupDataModel);

    void DeleteGroup(string creatorId, string id);

    void LinkingGroupWithPlace(string creatorId, string groupId, string placeId);
}
