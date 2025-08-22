namespace IvanSusaninProject_Contracts.BindingModels;

public class GuideBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public string? Fio { get; set; }

    public int Experience { get; set; }

    public int Age { get; set; }

    public string? UserId { get; set; }
}