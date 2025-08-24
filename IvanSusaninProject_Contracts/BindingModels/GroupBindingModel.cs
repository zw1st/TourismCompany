using IvanSusaninProject_Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.BindingModels;

public class GroupBindingModel
{
    public string? Id { get;  set; } = Guid.NewGuid().ToString();
    [Required(ErrorMessage = "Укажите название группы")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Укажите количество")]
    [Range(1, 100, ErrorMessage = "Должно быть от 1 до 100")]
    public int HumanAmount { get;  set; }

    [Required(ErrorMessage = "Выберите тип")]
    public HumanType HumanType { get; set; }
    public string? UserId { get; set; }
}
