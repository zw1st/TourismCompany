using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITripGuideStorageContract
{
    void Create(TripGuideDataModel model);
    List<TripGuideDataModel> GetByTripId(string tripId);
    //void Delete(string tripId, string guideId);
}
