using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITourBusinessLogicsContract
{
    List<TourDataModel> GetAllTours(string creatorId, DateTime date);

    TourDataModel GetTourById(string creatorId, string id);

    void InsertTour(TourDataModel tourDataModel);
}
