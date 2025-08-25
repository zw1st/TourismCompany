using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.ReportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IExcursionStorageContract
{
    List<ExcursionDataModel> GetList(string creatorId, DateTime? dateTime, string? guideId);

    ExcursionDataModel? GetElementById(string? creatorId, string id);

    ExcursionDataModel? GetElementByName(string? creatorId, string name);

    void AddElement(ExcursionDataModel element);
    void UpdElement(ExcursionDataModel element);

    Task<List<TripExcursionDto>> GetExcursionsByTourIds(List<string> tripIds, CancellationToken ct);
    Task<List<TripDetailsDto>> GetTripsWithDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
}
