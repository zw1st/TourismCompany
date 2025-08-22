using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IExcursionStorageContract
{
    List<ExcursionDataModel> GetList(string creatorId, DateTime? dateTime, string? guideId);

    ExcursionDataModel? GetElementById(string creatorId, string id);

    ExcursionDataModel? GetElementByName(string creatorId, string name);

    void AddElement(ExcursionDataModel element);
    void UpdElement(ExcursionDataModel element);

    public List<ExcursionDataModel> GetExcursionsByTourIds(string executorId, List<string> tripIds);

    public List<object> GetTripsWithDetailsByPeriod(DateTime startDate, DateTime endDate, string guaranderId);
}
