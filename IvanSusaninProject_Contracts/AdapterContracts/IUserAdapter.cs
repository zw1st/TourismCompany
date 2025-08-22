using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.AdapterContracts;

public interface IUserAdapter
{
    public void Register(UserRegisterBindingModel user);
    public UserViewModel? Login(string login, string password);
    public void Update(UserRegisterBindingModel user, string id);
    public UserRegisterBindingModel? GetElementById(string id);
    public bool CheckLoginExists(string login);
}
