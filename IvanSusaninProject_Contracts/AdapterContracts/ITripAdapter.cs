using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface ITripAdapter
{
    TripOperationResponse GetList(string creatorId);

    TripOperationResponse GetListByPeriod(string creatorId, DateTime fromDate, DateTime toDate);

    TripOperationResponse GetListByDate(string creatorId, DateTime tripDate);

    TripOperationResponse GetElement(string creatorId, string id);

    TripOperationResponse RegisterTrip(TripBindingModel model);

    TripOperationResponse ChangeTripInfo(TripBindingModel model);
}