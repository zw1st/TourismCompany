using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvanSusaninProject_DataBase.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace IvanSusaninProject_DataBase.Implementations;

public class UserStorageContract : IUserStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;


    public UserStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDataModel>()
                .ConstructUsing(w => new UserDataModel(w.Id, w.Login, w.PasswordHash, w.Salt, w.Email, w.UserRole));

            cfg.CreateMap<UserDataModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Обычно Id из модели не трогаем при записи

        }, loggerFactory);

        _mapper = new Mapper(config);
    }
    public UserDataModel GetUserById(string id)
    {
        try
        {
            return _mapper.Map<UserDataModel>(_dbContext.Users.Find(id));
        }
        catch (Exception ex)
        {
            // Log the exception
            throw new Exception("Error retrieving worker by ID", ex);
        }
    }
    public UserDataModel GetUserByLogin(string login)
    {
        try
        {
            var worker = _dbContext.Users.FirstOrDefault(w => w.Login == login);
            if (worker == null)
            {
                return null;
            }
            return _mapper.Map<UserDataModel>(worker);
        }
        catch (Exception ex)
        {
            // Log the exception
            throw new Exception("Error retrieving worker by login", ex);
        }
    }

    public void UpdateUser(UserDataModel worker)
    {
        try
        {
            var existingUser = _dbContext.Users.Find(worker.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }
            _mapper.Map(worker, existingUser);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            // Log the exception
            throw new Exception("Error updating worker", ex);
        }
    }
    public void AddUser(UserDataModel worker)
    {
        try
        {
            _dbContext.Users.Add(_mapper.Map<User>(worker));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Error inserting worker", ex);
        }

    }

    public void DeleteUser(string id)
    {
        try
        {
            var worker = _dbContext.Users.Find(id);
            if (worker == null)
            {
                throw new Exception("User not found");
            }
            _dbContext.Users.Remove(worker);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            // Log the exception
            throw new Exception("Error deleting worker", ex);

        }
    }
}

