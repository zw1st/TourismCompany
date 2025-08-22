using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class GroupOperationResponse : OperationResponse
{
    public static GroupOperationResponse OK(List<GroupViewModel> data) => OK<GroupOperationResponse, List<GroupViewModel>>(data);

    public static GroupOperationResponse OK(GroupViewModel data) => OK<GroupOperationResponse, GroupViewModel>(data);

    public static GroupOperationResponse NoContent() => NoContent<GroupOperationResponse>();

    public static GroupOperationResponse BadRequest(string message) => BadRequest<GroupOperationResponse>(message);

    public static GroupOperationResponse NotFound(string message) => NotFound<GroupOperationResponse>(message);

    public static GroupOperationResponse InternalServerError(string message) => InternalServerError<GroupOperationResponse>(message);
}