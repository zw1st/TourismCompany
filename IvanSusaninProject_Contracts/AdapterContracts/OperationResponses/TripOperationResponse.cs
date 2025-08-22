using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class TripOperationResponse : OperationResponse
{
    public static TripOperationResponse OK(List<TripViewModel> data) => OK<TripOperationResponse, List<TripViewModel>>(data);

    public static TripOperationResponse OK(TripViewModel data) => OK<TripOperationResponse, TripViewModel>(data);

    public static TripOperationResponse NoContent() => NoContent<TripOperationResponse>();

    public static TripOperationResponse NotFound(string message) => NotFound<TripOperationResponse>(message);

    public static TripOperationResponse BadRequest(string message) => BadRequest<TripOperationResponse>(message);

    public static TripOperationResponse InternalServerError(string message) => InternalServerError<TripOperationResponse>(message);
}