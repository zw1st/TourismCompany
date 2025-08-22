using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_DataBase.Models
{
    public class Trip
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? StartCity { get; set; }

        public string? EndCity { get; set; }

        public DateTime TripDate { get; set; }

        public int Duration { get;  set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

        [ForeignKey("TripId")]
        public List<TripPlace>? TripPlaces { get; set; }

        [ForeignKey("TripId")]
        public List<TripGuide>? TripGuides { get; set; }
    }
}