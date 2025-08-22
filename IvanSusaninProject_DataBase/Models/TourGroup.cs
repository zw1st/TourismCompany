namespace IvanSusaninProject_Database.Models;

public class TourGroup
{
    public required string TourId { get; set; }

    public required string GroupId { get; set; }

    public Tour? Tour { get; set; }

    public Group? Group { get; set; }
}
