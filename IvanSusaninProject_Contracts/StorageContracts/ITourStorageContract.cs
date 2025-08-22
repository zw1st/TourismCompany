using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITourStorageContract
{
    List<TourDataModel> GetList(string creatorId, DateTime? dateTime);

    TourDataModel? GetElementById(string creatorId, string id);

    TourDataModel? GetElementByName(string creatorId, string name);

    void AddElement(TourDataModel element);
}
