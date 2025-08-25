using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ReportModels;

public class PlaceWithGroupDto
{
    public PlaceDataModel Place { get; set; } = null!;

    public GroupDataModel? Group { get; set; }
}