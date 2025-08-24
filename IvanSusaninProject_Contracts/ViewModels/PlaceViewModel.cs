namespace IvanSusaninProject_Contracts.ViewModels;

public class PlaceViewModel
{
    public required string Id { get; set; }

    public required string? Address { get; set; }

    public required string? City { get; set; }

    public required string Name { get; set; }

    public required string? GroupName { get; set; }
}