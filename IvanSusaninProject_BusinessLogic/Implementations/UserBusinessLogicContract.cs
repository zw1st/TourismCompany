using DocumentFormat.OpenXml.Math;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_DataBase.Models;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Generators;
using System.Text.Json;
namespace IvanSusaninProject_BusinessLogic.Implementations;

public class UserBusinessLogicContract(IUserStorageContract userStorageContract) : IUserBusinessLogicContract
{
    private readonly IUserStorageContract _workerStorageContract = userStorageContract;

    public UserDataModel GetUserById(string id)
    {
        return _workerStorageContract.GetUserById(id);
    }
    public UserDataModel GetUserByLogin(string login)
    {
        return _workerStorageContract.GetUserByLogin(login);
    }
    public void AddUser(UserDataModel worker, string password)
    {
        var salt = PasswordHelper.GenerateSalt();
        var hash = PasswordHelper.HashPassword(password, salt);

        var _worker = new UserDataModel(worker.Id, worker.Login, hash, salt, worker.Email, worker.UserRole);

        _workerStorageContract.AddUser(_worker);
    }
    public void UpdateUser(UserDataModel updatedUser, string? newPassword = null)
    {
        var existingUser = _workerStorageContract.GetUserById(updatedUser.Id);
        if (existingUser == null)
        {
            throw new Exception("Работник не найден");
        }

        string salt = existingUser.Salt;
        string passwordHash = existingUser.PasswordHash;

        if (!string.IsNullOrEmpty(newPassword))
        {
            salt = PasswordHelper.GenerateSalt();
            passwordHash = PasswordHelper.HashPassword(newPassword, salt);
        }

        var workerToUpdate = new UserDataModel(
            updatedUser.Id,
            updatedUser.Login,
            passwordHash,
            salt,
            updatedUser.Email,
            updatedUser.UserRole
        );

        _workerStorageContract.UpdateUser(workerToUpdate);
    }

    public void DeleteUser(string id)
    {
        _workerStorageContract.DeleteUser(id);
    }

    public bool CheckLogin(string login, string password)
    {
        var worker = _workerStorageContract.GetUserByLogin(login);
        if (worker == null)
        {
            return false;
        }
        var hash = PasswordHelper.HashPassword(password, worker.Salt);
        return hash == worker.PasswordHash;
    }
}
