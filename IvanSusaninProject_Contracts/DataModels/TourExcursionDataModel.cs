using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class TourExcursionDataModel(string tourId, string tourName, string excursionId, string excursionName)
{
    public string TourId { get; private set; } = tourId;
    public string TourName { get; private set; } = tourName;
    public string ExcursionId { get; private set; } = excursionId;
}
