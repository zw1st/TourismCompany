using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.ReportModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITourStorageContract
{
    List<TourDataModel> GetList(string? creatorId, DateTime? dateTime);

    TourDataModel? GetElementById(string? creatorId, string id);

    TourDataModel? GetElementByName(string creatorId, string name);

    void AddElement(TourDataModel element);

    Task<List<TourPlacesDto>> GePlacesByTourIds(List<string> tourIds, CancellationToken ct);

    Task<List<TourDetailsDto>> GetToursWithDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
}
