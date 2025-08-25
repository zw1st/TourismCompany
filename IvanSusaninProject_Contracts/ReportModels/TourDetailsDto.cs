using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ReportModels;

public class TourDetailsDto
{
    public TourDataModel Tour { get; set; } = null!;

    public List<ExcursionWithGuideDto> Excursions { get; set; } = new();

    public List<GroupDataModel> Groups { get; set; } = new();
}
