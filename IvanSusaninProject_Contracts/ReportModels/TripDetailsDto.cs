using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ReportModels;

public class TripDetailsDto
{
    public TripDataModel Trip { get; set; } = null!;

    public List<PlaceWithGroupDto> Places { get; set; } = new();

    public List<GuideDataModel> Guides { get; set; } = new();
}
