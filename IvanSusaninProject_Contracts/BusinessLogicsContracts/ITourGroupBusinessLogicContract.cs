using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITourGroupBusinessLogicContract
{
    void AddGroupToTour(string tourId, string groupId);
    List<GroupShortInfo> GetGroupsForTour(string tourId);
}
