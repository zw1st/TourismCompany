using IvanSusaninProject_Contracts.DataModels;

namespace IvanSusaninProject_Contracts.ViewModels;

public class TripViewModel
{
    public required string? Id { get; set; }

    public required string? StartCity { get; set; }

    public required string? EndCity { get; set; }

    public DateTime TripDate { get; set; }

    public int Duration { get; set; }

    public required string? GuaranderId { get; set; }

    public required string GuarantorLogin { get; set; }

    public List<TripPlaceDataModel>? TripPlaces { get; set; }

    public List<TripGuideDataModel>? TripGuides { get; set; }
}