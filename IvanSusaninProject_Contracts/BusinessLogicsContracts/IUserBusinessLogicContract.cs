using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IUserBusinessLogicContract
{
    public UserDataModel GetUserById(string id);
    public UserDataModel GetUserByLogin(string login);
    public bool CheckLogin(string login, string password);
    public void AddUser(UserDataModel worker, string password);
    public void UpdateUser(UserDataModel worker, string? newPassword);
    public void DeleteUser(string id);
}
