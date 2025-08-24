using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TourBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Название обязательно.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Город обязателен.")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Дата начала обязательна.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Дата конца обязательна.")]
    public DateTime EndDate { get; set; }

    public string? UserId { get; set; }

    [Required(ErrorMessage = "Наличие групп обязательно.")]
    public List<string>? SelectedGroupIds { get; set; }

    [Required(ErrorMessage = "Наличие экскурсий обязательно.")]
    public List<string>? SelectedExcursionIds { get; set; }
}