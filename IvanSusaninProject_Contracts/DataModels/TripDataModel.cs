
using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.DataModels;

public class TripDataModel
{
    public string Id { get;  set; }
    public string StartCity { get;  set; }
    public string EndCity { get;  set; }
    public DateTime TripDate {  get; set; }
    public int Duration { get; set; }
    public string UserId { get; set; }
    public List<TripPlaceDataModel> TripPlaces { get; private set; }

    public List<TripGuideDataModel> TripGuides { get; private set; }

    public TripDataModel(string id, string startCity, string endCity, DateTime tripDate, int duration, string userId, List<TripPlaceDataModel> tripPlaces, List<TripGuideDataModel> tripGuides)
    {
        Id = id;
        StartCity = startCity;
        EndCity = endCity;
        TripDate = tripDate;
        Duration = duration;
        UserId = userId;
        TripPlaces = tripPlaces;
        TripGuides = tripGuides;
    }
}