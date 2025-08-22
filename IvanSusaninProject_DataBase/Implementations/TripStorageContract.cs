using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace IvanSusaninProject_DataBase.Implementations;

public class TripStorageContract : ITripStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public TripStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TripGuide, TripGuideDataModel>();
            cfg.CreateMap<TripGuideDataModel, TripGuide>();
            cfg.CreateMap<TripPlace, TripPlaceDataModel>();
            cfg.CreateMap<TripPlaceDataModel, TripPlace>();
            cfg.CreateMap<Trip, TripDataModel>();
            cfg.CreateMap<TripDataModel, Trip>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void AddElement(TripDataModel tripDataModel)
    {
        try
        {
            _dbContext.Trips.Add(_mapper.Map<Trip>(tripDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public TripDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            return _mapper.Map<TripDataModel>(GetTripById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TripDataModel> GetList(string guarantorId,DateTime? fromDate = null, DateTime? toDate = null, DateTime? tripDate = null)
    {
        try
        {
            var query = _dbContext.Trips.Include(x => x.TripPlaces).Include(x => x.TripGuides).Where(x => x.UserId == guarantorId).AsQueryable();
            if (tripDate is not null)
            {
                query = query.Where(x => x.TripDate == tripDate);
            }
            if (fromDate is not null && toDate is not null)
            {
                query = query.Where(x => x.TripDate >= fromDate && x.TripDate <= toDate);
            }
            return [.. query.Select(x => _mapper.Map<TripDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }
    
    public void UpdElement(TripDataModel tripDataModel)
    {
        try
        {
            var element = GetTripById(tripDataModel.Id, tripDataModel.GuaranderId) ?? throw new ElementNotFoundException(tripDataModel.Id);
            _dbContext.Trips.Update(_mapper.Map(tripDataModel, element));
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    

    private Trip? GetTripById(string id, string creatorId) => _dbContext.Trips.Where(x => x.UserId == creatorId).FirstOrDefault(x => x.Id == id);
}