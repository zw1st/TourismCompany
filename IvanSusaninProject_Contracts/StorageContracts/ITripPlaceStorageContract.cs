using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITripPlaceStorageContract
{
    void Create(TripPlaceDataModel model);
    List<TripPlaceDataModel> GetByTripId(string tripId);
    //void Delete(string tripId, string placeId);
}
