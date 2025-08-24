using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class ExcursionViewModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public DateTime ExcursionDate { get; set; }
    public required string GuideId { get; set; }
    public required string GuideName { get; set; }
}
