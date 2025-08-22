using IvanSusaninProject_Database.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_DataBase.Models;

public class Place
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Address { get; set; }

    public string? City { get; set; }

    public required string Name { get; set; }

    public string? GroupId { get; set; } = null;

    public required string UserId { get; set; }

    public Group? Group { get; set; } = null;

    public User? User { get; set; }

    [ForeignKey("PlaceId")]
    public List<TripPlace>? TripPlaces { get; set; }
}