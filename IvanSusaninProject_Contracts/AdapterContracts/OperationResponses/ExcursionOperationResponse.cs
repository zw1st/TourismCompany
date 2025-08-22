using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class ExcursionOperationResponse : OperationResponse
{
    public static ExcursionOperationResponse OK(List<ExcursionViewModel> data) => OK<ExcursionOperationResponse, List<ExcursionViewModel>>(data);

    public static ExcursionOperationResponse OK(ExcursionViewModel data) => OK<ExcursionOperationResponse, ExcursionViewModel>(data);

    public static ExcursionOperationResponse NoContent() => NoContent<ExcursionOperationResponse>();

    public static ExcursionOperationResponse BadRequest(string message) => BadRequest<ExcursionOperationResponse>(message);

    public static ExcursionOperationResponse NotFound(string message) => NotFound<ExcursionOperationResponse>(message);

    public static ExcursionOperationResponse InternalServerError(string message) => InternalServerError<ExcursionOperationResponse>(message);
}