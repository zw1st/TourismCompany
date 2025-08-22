using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IGuideStrorageContract
{
    List<GuideDataModel> GetList(string? creatorId);

    GuideDataModel? GetElementById(string creatorId, string id);

    GuideDataModel? GetElementByFIO(string creatorId, string fio);

    void AddElement(GuideDataModel guideDataModel);

    void UpdElement(GuideDataModel guideDataModel);

    void DelElement(string creatorId, string id);
}