using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITripPlaceBusinessLogicContract
{
    void AddPlaceToTrip(string tripId, string placeId);
    List<PlaceViewModel> GetPlacesForTrip(string tripId);
}
