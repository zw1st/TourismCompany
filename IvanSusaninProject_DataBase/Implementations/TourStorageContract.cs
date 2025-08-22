using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace IvanSusaninProject_DataBase.Implementations;

public class TourStorageContract : ITourStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;

    public TourStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var loggerFactory = NullLoggerFactory.Instance;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Tour, TourDataModel>();
            cfg.CreateMap<TourDataModel, Tour>();

            cfg.CreateMap<TourGroup, TourGroupDataModel>();
            cfg.CreateMap<TourGroupDataModel, TourGroup>();

            cfg.CreateMap<TourExcursion, TourExcursionDataModel>();
            cfg.CreateMap<TourExcursionDataModel, TourExcursion>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }


    public void AddElement(TourDataModel tourDataModel)
    {
        try
        {
            _dbContext.Tours.Add(_mapper.Map<Tour>(tourDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public TourDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            return _mapper.Map<TourDataModel>(GetTourById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public TourDataModel? GetElementByName(string creatorId, string name)
    {
        try
        {
            return _mapper.Map<TourDataModel>(_dbContext.Tours.Where(x => x.UserId == creatorId).FirstOrDefault(x => x.Name == name));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TourDataModel> GetList(string? executorId, DateTime? dateTime) 
    {
        try
        {
            var query = _dbContext.Tours.Include(x => x.TourGroups).Include(x => x.TourExcursions).AsQueryable();
            if (executorId is not null)
            {
                query = query.Where(x => x.UserId == executorId);
            }
            if (dateTime is not null)
            {
                query = query.Where(x => x.StartDate <= dateTime && x.EndDate >= dateTime);
            }
            return [.. query.Select(x => _mapper.Map<TourDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Tour? GetTourById(string id, string creatorId) => _dbContext.Tours.Where(x => x.UserId == creatorId).FirstOrDefault(x => x.Id == id);

}
