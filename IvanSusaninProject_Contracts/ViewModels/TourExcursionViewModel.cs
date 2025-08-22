using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class TourExcursionViewModel
{
    public required string TourId { get; set; }
    public required string TourName { get; set; }
    public required string ExcursionId { get; set; }
    public required string ExcursionName { get; set; }
}
