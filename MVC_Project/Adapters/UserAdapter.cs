using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;

namespace MVC_Project.Adapters;

public class UserAdapter : IUserAdapter
{
    private readonly IUserBusinessLogicContract _workerBusinessLogic;
    private Mapper _mapper;
    public UserAdapter(IUserBusinessLogicContract workerBusinessLogic)
    {
        _workerBusinessLogic = workerBusinessLogic;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserRegisterBindingModel, UserDataModel>()
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role))
            .ReverseMap();
            cfg.CreateMap<UserDataModel, UserViewModel>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }
    public void Register(UserRegisterBindingModel user)
    {
        try
        {
            _workerBusinessLogic.AddUser(_mapper.Map<UserDataModel>(user), user.Password);
        }
        catch (StorageException ex)
        {
            // Log the exception
            throw new StorageException(ex);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }
    public UserViewModel? Login(string login, string password)
    {
        try
        {
            var isValid = _workerBusinessLogic.CheckLogin(login, password);
            if (!isValid) return null;

            var worker = _workerBusinessLogic.GetUserByLogin(login);
            return _mapper.Map<UserViewModel>(worker);
        }
        catch (StorageException ex)
        {
            // Log the exception
            throw new StorageException(ex);
        }
        catch (Exception ex)
        {
            throw new StorageException(ex);
        }
    }

    public UserRegisterBindingModel? GetElementById(string id)
    {
        try
        {
            var worker = _workerBusinessLogic.GetUserById(id);
            if (worker == null)
            {
                return null;
            }
            return _mapper.Map<UserRegisterBindingModel>(worker);
        }
        catch
        {
            throw;
        }
    }
    public bool CheckLoginExists(string login)
    {
        // if worker doesnt in datatabase returns false, if exists returns true
        try
        {
            var worker = _workerBusinessLogic.GetUserByLogin(login);
            if (worker == null)
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Update(UserRegisterBindingModel user, string id)
    {
        try
        {
            var worker = _workerBusinessLogic.GetUserById(id);
            worker = _mapper.Map(user, worker);
            _workerBusinessLogic.UpdateUser(worker, user.Password);
        }
        catch
        {
            throw;
        }
    }
}