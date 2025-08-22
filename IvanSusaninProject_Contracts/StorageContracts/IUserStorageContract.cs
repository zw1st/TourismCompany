using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.StorageContracts;

public interface IUserStorageContract
{
    UserDataModel GetUserById(string id);
    UserDataModel GetUserByLogin(string login);
    void AddUser(UserDataModel worker);
    void UpdateUser(UserDataModel worker);
    void DeleteUser(string id);
}
