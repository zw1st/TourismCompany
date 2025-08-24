using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.ViewModels;

public class TripViewModel
{
    public required string? Id { get; set; }

    public required string? StartCity { get; set; }

    public required string? EndCity { get; set; }

    public DateTime TripDate { get; set; }

    public int Duration { get; set; }

    public List<PlaceViewModel> Places { get; set; }

    public List<GuideViewModel> Guides { get; set; }
}