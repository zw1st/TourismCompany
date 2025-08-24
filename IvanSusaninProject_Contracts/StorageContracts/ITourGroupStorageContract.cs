using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface ITourGroupStorageContract
{
    void Create(TourGroupDataModel model);
    List<TourGroupDataModel> GetByTourId(string tourId);
    List<TourGroupDataModel> GetByGroupId(string groupId);
}
