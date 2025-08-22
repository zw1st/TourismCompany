using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IGuideAdapter
{
    List<GuideViewModel> GetList(string? creatorId = null);

    GuideBindingModel GetElement(string creatorId, string data);

    void RegisterGuide(GuideBindingModel model);

    void ChangeGuideInfo(GuideBindingModel model);

    void RemoveGuide(string creatorId, string id);

    void LinkGuideToExcursion(string creatorId, string guideId, string excursionId);
}