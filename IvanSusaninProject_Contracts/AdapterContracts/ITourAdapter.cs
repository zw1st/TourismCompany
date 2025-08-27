
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface ITourAdapter
{
    List<TourViewModel> GetList(string? creatorId = null, DateTime? date = null);
    TourViewModel GetElement(string? creatorId, string data);
    void RegisterTour(TourBindingModel tourModel);
    public void RegisterTourWithRelations(TourBindingModel model, List<string> groupIds, List<string> excursionIds);
    public List<TourViewModel> GetListWithDetails(string userId);
}
