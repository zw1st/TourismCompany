namespace IvanSusaninProject_DataBase.Models;

public class TripGuide
{
    public required string TripId { get; set; }

    public required string GuideId { get; set; }
    public Trip? Trip { get; set; }

    public Guide? Guide { get; set; }
}