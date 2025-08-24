using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITourExcursionBusinessLogicContract
{
    void AddExcursionToTour(string tourId, string excursionId);
    List<ExcursionShortInfo> GetExcursionForTour(string tourId);
}
