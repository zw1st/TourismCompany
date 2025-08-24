using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Implementations;

public class TripPlaceStorageContract : ITripPlaceStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public TripPlaceStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        var loggerFactory = NullLoggerFactory.Instance;
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TripPlace, TripPlaceDataModel>();

            cfg.CreateMap<TripPlaceDataModel, TripPlace>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }
    public void Create(TripPlaceDataModel model)
    {
        try
        {
            _dbContext.TripPlaces.Add(_mapper.Map<TripPlace>(model));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TripPlaceDataModel> GetByTripId(string tripId)
    {
        try
        {
            try
            {
                var query = _dbContext.TripPlaces.AsQueryable();

                if (!string.IsNullOrEmpty(tripId))
                {
                    query = query.Where(x => x.TripId == tripId);
                }

                return query.Select(x => _mapper.Map<TripPlaceDataModel>(x)).ToList();
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new StorageException(ex);
            }
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }
}
