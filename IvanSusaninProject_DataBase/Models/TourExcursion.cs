namespace IvanSusaninProject_Database.Models;

public class TourExcursion
{
    public required string TourId { get; set; }

    public required string ExcursionId { get; set; }

    public Tour? Tour { get; set; }

    public Excursion? Excursion { get; set; }
}
