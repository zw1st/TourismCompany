using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Implementations;

public class TourExcursionStorageContract : ITourExcursionStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public TourExcursionStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        var loggerFactory = NullLoggerFactory.Instance;
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TourExcursion, TourExcursionDataModel>();

            cfg.CreateMap<TourExcursionDataModel, TourExcursion>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }


    public void Create(TourExcursionDataModel model)
    {
        try
        {
            _dbContext.TourExcursions.Add(_mapper.Map<TourExcursion>(model));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TourExcursionDataModel> GetByExcursionId(string excursionId)
    {
        try
        {
            try
            {
                var query = _dbContext.TourExcursions.AsQueryable();

                if (!string.IsNullOrEmpty(excursionId))
                {
                    query = query.Where(x => x.ExcursionId == excursionId);
                }

                return query.Select(x => _mapper.Map<TourExcursionDataModel>(x)).ToList();
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

    public List<TourExcursionDataModel> GetByTourId(string tourId)
    {
        try
        {
            try
            {
                var query = _dbContext.TourExcursions.AsQueryable();

                if (!string.IsNullOrEmpty(tourId))
                {
                    query = query.Where(x => x.TourId == tourId);
                }

                return query.Select(x => _mapper.Map<TourExcursionDataModel>(x)).ToList();
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
