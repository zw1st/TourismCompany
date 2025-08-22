using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface ITourAdapter
{
    TourOperationResponse GetList(string creatorId, DateTime date);

    TourOperationResponse GetElement(string creatorId, string data);

    TourOperationResponse RegisterTour(TourBindingModel tourModel);
}
