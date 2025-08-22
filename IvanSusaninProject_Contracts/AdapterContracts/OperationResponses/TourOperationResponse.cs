using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class TourOperationResponse : OperationResponse
{
    public static TourOperationResponse OK(List<TourViewModel> data) => OK<TourOperationResponse, List<TourViewModel>>(data);

    public static TourOperationResponse OK(TourViewModel data) => OK<TourOperationResponse, TourViewModel>(data);

    public static TourOperationResponse NoContent() => NoContent<TourOperationResponse>();

    public static TourOperationResponse BadRequest(string message) => BadRequest<TourOperationResponse>(message);

    public static TourOperationResponse NotFound(string message) => NotFound<TourOperationResponse>(message);

    public static TourOperationResponse InternalServerError(string message) => InternalServerError<TourOperationResponse>(message);
}