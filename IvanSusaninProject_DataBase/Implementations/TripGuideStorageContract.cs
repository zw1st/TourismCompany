using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Implementations;

public class TripGuideStorageContract : ITripGuideStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public TripGuideStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        var loggerFactory = NullLoggerFactory.Instance;
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TripGuide, TripGuideDataModel>();

            cfg.CreateMap<TripGuideDataModel, TripGuide>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }
    public void Create(TripGuideDataModel model)
    {
        try
        {
            _dbContext.TripGuides.Add(_mapper.Map<TripGuide>(model));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }


    public List<TripGuideDataModel> GetByTripId(string tripId)
    {
        try
        {
            try
            {
                var query = _dbContext.TripGuides.AsQueryable();

                if (!string.IsNullOrEmpty(tripId))
                {
                    query = query.Where(x => x.TripId == tripId);
                }

                return query.Select(x => _mapper.Map<TripGuideDataModel>(x)).ToList();
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
