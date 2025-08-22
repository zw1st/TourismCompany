using IvanSusaninProject_DataBase.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_Database.Models;

public class Excursion
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Name { get; set; }

    public DateTime ExcursionDate { get; set; }

    public required string UserId { get; set; }

    public User? User { get; set; }

    public string? GuideId { get; set; } = null;

    public Guide? Guide { get; set; } = null;

    [ForeignKey("ExcursionId")]
    public List<TourExcursion>? TourExcursions { get; set; }
}