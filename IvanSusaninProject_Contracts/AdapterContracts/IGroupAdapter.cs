using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IGroupAdapter
{
    List<GroupViewModel> GetList(string? creatorId = null);

    GroupBindingModel GetElement(string creatorId, string data);

    void RegisterGroup(GroupBindingModel groupModel);

    void ChangeGroupInfo(GroupBindingModel groupModel);

    void RemoveGroup(string creatorId, string id);

    void LinkGroupWithPlace(string creatorId, string groupId, string placeId);
}
