using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITripStorageContract
{
    List<TripDataModel> GetList(string creatorId, DateTime? fromDate = null, DateTime? toDate = null, DateTime ? tripDate = null);

    TripDataModel? GetElementById(string creatorId, string id);

    void AddElement(TripDataModel tripDataModel);

    void UpdElement(TripDataModel tripDataModel);
}