using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class PlaceOperationResponse : OperationResponse
{
    public static PlaceOperationResponse OK(List<PlaceViewModel> data) => OK<PlaceOperationResponse, List<PlaceViewModel>>(data);

    public static PlaceOperationResponse OK(PlaceViewModel data) => OK<PlaceOperationResponse, PlaceViewModel>(data);

    public static PlaceOperationResponse NoContent() => NoContent<PlaceOperationResponse>();

    public static PlaceOperationResponse NotFound(string message) => NotFound<PlaceOperationResponse>(message);

    public static PlaceOperationResponse BadRequest(string message) => BadRequest<PlaceOperationResponse>(message);

    public static PlaceOperationResponse InternalServerError(string message) => InternalServerError<PlaceOperationResponse>(message);
}