using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface ITripGuideBusinessLogicContract
{
    void AddGuideToTrip(string tripId, string guideId);
    List<GuideViewModel> GetGuidesForTrip(string tripId);
}
