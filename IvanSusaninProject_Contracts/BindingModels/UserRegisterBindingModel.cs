using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels;

public class UserRegisterBindingModel
{
    [Required(ErrorMessage = "Логин обязателен.")]
    [Display(Name = "Логин")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Логин должен содержать от 4 до 50 символов.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Пароль обязателен.")]
    [Display(Name = "Пароль")]
    [StringLength(100, MinimumLength = 7, ErrorMessage = "Пароль должен содержать минимум 7 символов.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Электронная почта обязательна.")]
    [EmailAddress(ErrorMessage = "Некорректный формат электронной почты.")]
    [Display(Name = "Email (эл. почта)")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Выберите роль")]
    [Display(Name = "Роль")]
    public UserRole Role { get; set; }
}
