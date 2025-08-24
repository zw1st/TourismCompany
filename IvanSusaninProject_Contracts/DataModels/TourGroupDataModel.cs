using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class TourGroupDataModel(string tourId,string groupId)
{
    public string TourId { get;  set; } = tourId;
    public string GroupId { get;  set; } = groupId;
}
