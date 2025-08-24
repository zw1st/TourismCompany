using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TripBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Стартовый город обязателен.")]
    public string? StartCity { get; set; }

    [Required(ErrorMessage = "Конечный город обязателен.")]
    public string? EndCity { get; set; }

    [Required(ErrorMessage = "Дата обязательна.")]
    public DateTime TripDate { get; set; }

    [Required(ErrorMessage = "Длительность обязательна.")]
    public int Duration { get; set; }

    public string? UserId { get; set; }

    [Required(ErrorMessage = "Наличие мест обязательно.")]
    public List<string>? SelectedPlacesIds { get; set; }

    [Required(ErrorMessage = "Наличие гидов обязательно.")]
    public List<string>? SelectedGuidesIds { get; set; }
}