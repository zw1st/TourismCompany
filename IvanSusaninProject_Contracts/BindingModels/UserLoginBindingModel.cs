using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels;

public class UserLoginBindingModel
{
    [Required]
    [Display(Name = "Логин")]
    public required string Login { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public required string Password { get; set; }
}
