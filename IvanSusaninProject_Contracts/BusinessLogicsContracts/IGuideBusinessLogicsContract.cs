using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IGuideBusinessLogicsContract
{
    List<GuideDataModel> GetAllGuides(string? creatorId = null);

    GuideDataModel GetGuideByData(string creatorId, string data);

    void InsertGuide(GuideDataModel model);

    void UpdateGuide(GuideDataModel model);

    void DeleteGuide(string creatorId, string id);

    void LinkingGuideToExcursion(string creatorId, string guideId, string excursionId);

}