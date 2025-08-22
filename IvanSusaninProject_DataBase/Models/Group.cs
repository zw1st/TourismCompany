using IvanSusaninProject_Contracts.Enums;
using IvanSusaninProject_DataBase.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvanSusaninProject_Database.Models;

public class Group
{
    public required string Id {get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public int HumanAmount { get; set;}

    public HumanType HumanType {  get; set;}

    public required string UserId { get; set;}

    public User? User { get; set; }

    [ForeignKey("GroupId")]
    public List<TourGroup>? TourGroups { get; set; }
}