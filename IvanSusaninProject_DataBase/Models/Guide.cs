using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_DataBase.Models
{
    public class Guide
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Fio { get;  set; }

        public int Experience { get;  set; }

        public int Age { get; set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

        [ForeignKey("GuideId")]
        public List<TripGuide>? TripGuides { get; set; }
    }
}