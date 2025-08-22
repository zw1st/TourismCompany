using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class LinkGuideToExcursionViewModel
{
    public string SelectedGuideId { get; set; }
    public string SelectedExcursionId { get; set; }
    public List<GuideViewModel> Guides { get; set; } = new();
    public List<ExcursionViewModel> Excursions { get; set; } = new();
}
