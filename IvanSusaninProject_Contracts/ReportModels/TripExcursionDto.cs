using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ReportModels;

public class TripExcursionDto
{
    public string TripId { get; set; } = string.Empty;

    public string TripName { get; set; } = string.Empty;

    public List<string> Excursions { get; set; } = new();
}
