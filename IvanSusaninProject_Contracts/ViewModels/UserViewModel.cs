using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class UserViewModel
{
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string UserRole {  get; set; }
}
