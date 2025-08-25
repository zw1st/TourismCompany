using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ReportModels;

public class TourPlacesDto
{
    public string TourId { get; set; } = string.Empty;

    public string TourName { get; set; } = string.Empty;

    public List<string> Places { get; set; } = new();
}