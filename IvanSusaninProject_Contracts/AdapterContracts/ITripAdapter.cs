using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface ITripAdapter
{
    List<TripViewModel> GetList(string creatorId);
    List<TripViewModel> GetListByPeriod(string creatorId, DateTime fromDate, DateTime toDate);
    List<TripViewModel> GetListByDate(string creatorId, DateTime tripDate);
    TripViewModel GetElement(string? creatorId, string id);
    void RegisterTrip(TripBindingModel model);
    void ChangeTripInfo(TripBindingModel model);

    public void RegisterTripWithRelations(TripBindingModel model, List<string> placeIds, List<string> guideIds);
    public List<TripViewModel> GetListWithDetails();
}