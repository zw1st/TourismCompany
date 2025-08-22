using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TourGroupBindingModel
{
    public string? TourId { get;  set; }
    public string? TourName { get; set; }
    public string? GroupId { get; set; }
    public string? GroupName { get; set; }
}
