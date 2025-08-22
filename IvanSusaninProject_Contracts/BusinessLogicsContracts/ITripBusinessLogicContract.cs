using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITripBusinessLogicContract
{
    List<TripDataModel> GetAllTrips(string creatorId);

    List<TripDataModel> GetAllTripsByPeriod(string creatorId, DateTime fromDate, DateTime toDate);
    
    List<TripDataModel> GetAllTripsByDate(string creatorId, DateTime tripDate);

    TripDataModel GetTripById(string creatorId, string id);

    void InsertTrip(TripDataModel model);

    void UpdateTrip(TripDataModel model);
}