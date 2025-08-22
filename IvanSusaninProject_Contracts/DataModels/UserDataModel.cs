using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class UserDataModel
{
    public UserDataModel(string id, string login, string passwordHash, string salt, string? email, UserRole role)
    {
        Id = id;
        Login = login;
        Salt = salt;
        PasswordHash = passwordHash;
        Email = email;
        UserRole = role;
    }

    public UserDataModel(string id, string login, string passwordHash, string salt, string email)
    {
        Id = id;
        Login = login;
        Salt = salt;
        PasswordHash = passwordHash;
        Email = email;
        UserRole = UserRole.Executor;
    }

    public UserDataModel(string login, string email)
    {
        Id = Guid.NewGuid().ToString();
        Login = login;
        PasswordHash = "";
        Salt = "";
        Email = email;
        UserRole = UserRole.Executor;
    }

    public string Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; private set; }
    public string? Email { get; set; }
    public UserRole UserRole { get; set; }
}
