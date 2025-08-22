using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class LinkGroupToPlaceViewModel
{
    public string SelectedGroupId { get; set; }
    public string SelectedPlaceId { get; set; }
    public List<GroupViewModel> Groups { get; set; } = new();
    public List<PlaceViewModel> Places { get; set; } = new();
}
