
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IPlaceAdapter
{
    List<PlaceViewModel> GetList(string? creatorId = null);

    List<PlaceViewModel> GetListByGroup(string creatorId, string groupId);

    PlaceBindingModel GetElement(string creatorId, string data);

    void RegisterPlace(PlaceBindingModel model);

    void ChangePlaceInfo(PlaceBindingModel model);

    void RemovePlace(string creatorId, string id);
}