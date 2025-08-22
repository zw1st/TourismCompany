using IvanSusaninProject_Contracts.DataModels;
using Microsoft.VisualBasic;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TripBindingModel
{
    public string? Id { get; set; }

    public string? StartCity { get; set; }

    public string? EndCity { get; set; }

    public DateTime TripDate { get; set; }

    public int Duration { get; set; }

    public string? GuaranderId { get; set; }

    public List<TripPlaceDataModel>? TripPlaces { get; set; }

    public List<TripGuideDataModel>? TripGuides { get; set; }
}