
using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.DataModels;

public class TripDataModel(string id, string startCity, string endCity, DateTime tripDate, int duration, string userId)
{
    public string Id { get;  set; } = id;
    public string StartCity { get;  set; } = startCity;
    public string EndCity { get;  set; } = endCity;
    public DateTime TripDate {  get; set; } = tripDate;
    public int Duration { get; set; } = duration;
    public string UserId { get; set; } = userId;
}