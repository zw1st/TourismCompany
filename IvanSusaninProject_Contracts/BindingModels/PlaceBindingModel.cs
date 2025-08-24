using System.ComponentModel.DataAnnotations;

namespace IvanSusaninProject_Contracts.BindingModels;

public class PlaceBindingModel
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Адрес обязателен.")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Город обязателен.")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Название обязательно.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Группы обязательны.")]
    public string? GroupId { get; set; }

    public required string GroupName {  get; set; }

    public string? UserId { get; set; }
}