using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Models;

public class User
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();

    public required string Login { get; set; }

    public required string PasswordHash { get; set; }

    public required string Salt { get; set; }

    public string? Email { get; set; }

    public UserRole UserRole { get; set; }
}
