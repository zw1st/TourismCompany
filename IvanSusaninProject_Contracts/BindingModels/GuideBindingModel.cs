using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.BindingModels;

public class GuideBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "ФИО обязательны.")]
    public string? Fio { get; set; }

    [Required(ErrorMessage = "Стаж обязателен.")]
    [Range(1, 100, ErrorMessage = "Должно быть от 1 до 100")]
    public int Experience { get; set; }

    [Required(ErrorMessage = "Возраст обязателен.")]
    [Range(18, 100, ErrorMessage = "Должно быть от 1 до 100")]
    public int Age { get; set; }

    public string? UserId { get; set; }
}