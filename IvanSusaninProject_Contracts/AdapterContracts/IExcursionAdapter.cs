using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IExcursionAdapter
{
    List<ExcursionViewModel> GetList(string? creatorId = null, DateTime? dateTime = null, string? guideId = null);

    ExcursionBindingModel GetElement(string? creatorId, string data);

    void RegisterExcursion(ExcursionBindingModel excursionModel);
}
