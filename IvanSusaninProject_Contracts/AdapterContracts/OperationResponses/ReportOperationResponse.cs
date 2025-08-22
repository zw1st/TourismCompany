using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.ViewModels;

namespace IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;

public class ReportOperationResponse : OperationResponse
{
//    public static ReportOperationResponse OK(List<ProductAndProductHistoryViewModel> data) => OK<ReportOperationResponse, List<ProductAndProductHistoryViewModel>>(data);

//    public static ReportOperationResponse OK(List<SaleViewModel> data) => OK<ReportOperationResponse, List<SaleViewModel>>(data);

//    public static ReportOperationResponse OK(List<WorkerSalaryByPeriodViewModel> data) => OK<ReportOperationResponse, List<WorkerSalaryByPeriodViewModel>>(data);

    public static ReportOperationResponse OK(Stream data, string fileName) => OK<ReportOperationResponse, Stream>(data, fileName);

    public static ReportOperationResponse BadRequest(string message) => BadRequest<ReportOperationResponse>(message);

    public static ReportOperationResponse InternalServerError(string message) => InternalServerError<ReportOperationResponse>(message);
}
