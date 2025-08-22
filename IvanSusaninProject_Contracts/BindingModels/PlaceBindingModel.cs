namespace IvanSusaninProject_Contracts.BindingModels;

public class PlaceBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Name { get; set; }

    public string? GroupId { get; set; }

    public string? UserId { get; set; }
}