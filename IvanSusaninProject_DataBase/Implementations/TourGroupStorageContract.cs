using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_DataBase.Implementationsl;

public class TourGroupStorageContract : ITourGroupStorageContract
{

    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public TourGroupStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        var loggerFactory = NullLoggerFactory.Instance;
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TourGroup, TourGroupDataModel>();

            cfg.CreateMap<TourGroupDataModel, TourGroup>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void Create(TourGroupDataModel model)
    {
        try
        {
            _dbContext.TourGroups.Add(_mapper.Map<TourGroup>(model));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<TourGroupDataModel> GetByGroupId(string groupId)
    {
        try
        {
            try
            {
                var query = _dbContext.TourGroups.AsQueryable();

                if (!string.IsNullOrEmpty(groupId))
                {
                    query = query.Where(x => x.GroupId == groupId);
                }

                return query.Select(x => _mapper.Map<TourGroupDataModel>(x)).ToList();
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

    public List<TourGroupDataModel> GetByTourId(string tourId)
    {
        try
        {
            try
            {
                var query = _dbContext.TourGroups.AsQueryable();

                if (!string.IsNullOrEmpty(tourId))
                {
                    query = query.Where(x => x.TourId == tourId);
                }

                return query.Select(x => _mapper.Map<TourGroupDataModel>(x)).ToList();
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
