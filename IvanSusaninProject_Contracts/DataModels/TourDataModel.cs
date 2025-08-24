namespace IvanSusaninProject_Contracts.DataModels;

public class TourDataModel(string id, string name, string city, DateTime startDate, DateTime endDate, string userId)
{
    public string Id {  get; set; } = id;
    public string Name { get; set; } = name;
    public string City { get; set; } = city;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public string UserId { get; set; } = userId;
}
