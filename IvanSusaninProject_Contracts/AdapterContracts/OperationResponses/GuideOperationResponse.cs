using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class GuideOperationResponse : OperationResponse
{
    public static GuideOperationResponse OK(List<GuideViewModel> data) => OK<GuideOperationResponse, List<GuideViewModel>>(data);

    public static GuideOperationResponse OK(GuideViewModel data) => OK<GuideOperationResponse, GuideViewModel>(data);

    public static GuideOperationResponse NoContent() => NoContent<GuideOperationResponse>();

    public static GuideOperationResponse NotFound(string message) => NotFound<GuideOperationResponse>(message);

    public static GuideOperationResponse BadRequest(string message) => BadRequest<GuideOperationResponse>(message);

    public static GuideOperationResponse InternalServerError(string message) => InternalServerError<GuideOperationResponse>(message);
}