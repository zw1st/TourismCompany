using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.BindingModels;

public class ExcursionBindingModel
{
    public string? Id { get;  set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Укажите название экскурсии")]
    public string? Name { get;  set; } 
    public DateTime ExcursionDate { get;  set; }
    public string? UserId { get;  set; }

    public string? GuideId { get;  set; }
    public string? GuideName { get; set; }
}
