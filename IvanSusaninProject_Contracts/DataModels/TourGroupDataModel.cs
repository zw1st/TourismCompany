using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class TourGroupDataModel(string tourId, string tourName, string groupId, string groupName)
{
    public string TourId { get; private set; } = tourId;
    public string TourName { get; private set; } = tourName;
    public string GroupId { get; private set; } = groupId;
    public string GroupName { get; private set; } = groupName;
}
