
namespace IvanSusaninProject_Contracts.DataModels;

public class TripDataModel(string id, string startcity, string endcity, DateTime tripDate, int duration, string guaranderId, List<TripPlaceDataModel> tripPlaces, List<TripGuideDataModel> tripGuides)
{
    public string Id { get; private set; } = id;

    public string StartCity { get; private set; } = startcity;

    public string EndCity { get; private set; } = endcity;

    public DateTime TripDate {  get; private set; } = tripDate;

    public int Duration { get; private set; } = duration;

    public string GuaranderId { get; private set; } = guaranderId;

    public List<TripPlaceDataModel> TripPlaces { get; private set; } = tripPlaces;

    public List<TripGuideDataModel> TripGuides { get; private set; } = tripGuides;
}