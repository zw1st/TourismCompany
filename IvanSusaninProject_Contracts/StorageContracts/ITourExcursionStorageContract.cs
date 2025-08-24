using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITourExcursionStorageContract
{
    void Create(TourExcursionDataModel model);
    List<TourExcursionDataModel> GetByTourId(string tourId);
    List<TourExcursionDataModel> GetByExcursionId(string excursionId);
}
