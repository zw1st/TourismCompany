using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IPlaceBusinessLogicContract
{
    List<PlaceDataModel> GetAllPlaces(string creatorId);

    List<PlaceDataModel> GetAllPlacesByGroup(string creatorId, string groupId);

    PlaceDataModel GetPlaceByData(string creatorId, string data);

    void InsertPlace(PlaceDataModel model);

    void UpdatePlace(PlaceDataModel model);

    void DeletePlace(string creatorId, string id);
}