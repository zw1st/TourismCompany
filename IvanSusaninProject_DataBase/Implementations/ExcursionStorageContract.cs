using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace IvanSusaninProject_DataBase.Implementations;

public class ExcursionStorageContract : IExcursionStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly Mapper _mapper;

    public ExcursionStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var loggerFactory = NullLoggerFactory.Instance;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Guide, GuideDataModel>();
            cfg.CreateMap<User, UserDataModel>();

            cfg.CreateMap<Excursion, ExcursionDataModel>();
            cfg.CreateMap<ExcursionDataModel, Excursion>();

            cfg.CreateMap<TourExcursion, TourExcursionDataModel>();
            cfg.CreateMap<TourExcursionDataModel, TourExcursion>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void AddElement(ExcursionDataModel excursionDataModel)
    {
        try
        {
            _dbContext.Excursions.Add(_mapper.Map<Excursion>(excursionDataModel));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", excursionDataModel.Id);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ExcursionDataModel? GetElementById(string? creatorId, string id)
    {
        try
        {
            return _mapper.Map<ExcursionDataModel>(GetExcursionById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ExcursionDataModel? GetElementByName(string creatorId, string name)
    {
        try
        {
            return _mapper.Map<ExcursionDataModel>(GetExcursionByName(name, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<ExcursionDataModel> GetList(string? executorId, DateTime? dateTime, string? guideId)
    {
        try
        {
            var query = _dbContext.Excursions.Include(x => x.TourExcursions).Include(x => x.Guide).Include(x => x.User).AsQueryable();
            
            if (guideId is not null)
            {
                query = query.Where(x => x.GuideId == guideId);
            }
            if (dateTime is not null)
            {
                query = query.Where(x => x.ExcursionDate == dateTime);
            }
            if (executorId is not null)
            {
                query = query.Where(x => x.UserId == executorId);
            }
            return [.. query.Select(x => _mapper.Map<ExcursionDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<ExcursionDataModel> GetExcursionsByTourIds(string executorId, List<string> tripIds)
    {
        try
        {
            var guideIds = _dbContext.TripGuides
                .Where(tg => tripIds.Contains(tg.TripId))
                .Select(tg => tg.GuideId)
                .Distinct()
                .ToList();

            return [.. _dbContext.Excursions
                .Where(e => e.UserId == executorId && guideIds.Contains(e.GuideId))
                .Select(e => _mapper.Map<ExcursionDataModel>(e))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<object> GetTripsWithDetailsByPeriod(DateTime startDate, DateTime endDate, string guaranderId)
    {
        try
        {
            // Получаем поездки за указанный период
            var trips = _dbContext.Trips
                .Where(t => t.TripDate >= startDate &&
                           t.TripDate <= endDate &&
                           t.UserId == guaranderId)
                .ToList();

            if (trips.Count == 0)
                return [];

            var tripIds = trips.Select(t => t.Id).ToList();

            // Получаем все связанные данные за один запрос
            var tripDetails = (
                from trip in trips
                join tp in _dbContext.TripPlaces on trip.Id equals tp.TripId into tripPlaces
                from tp in tripPlaces.DefaultIfEmpty()
                join place in _dbContext.Places on tp?.PlaceId equals place.Id into places
                from place in places.DefaultIfEmpty()
                join groupData in _dbContext.Groups on place?.GroupId equals groupData.Id into groups
                from groupData in groups.DefaultIfEmpty()
                join tg in _dbContext.TripGuides on trip.Id equals tg.TripId into tripGuides
                from tg in tripGuides.DefaultIfEmpty()
                join guide in _dbContext.Guides on tg?.GuideId equals guide.Id into guides
                from guide in guides.DefaultIfEmpty()
                select new
                {
                    Trip = trip,
                    Place = place,
                    Group = groupData,
                    Guide = guide
                }
            ).ToList();

            // Группируем данные по поездкам
            var groupedResults = tripDetails
                .GroupBy(x => x.Trip.Id)
                .Select(g => new
                {
                    Trip = _mapper.Map<TripDataModel>(g.First().Trip),
                    Places = g.Where(x => x.Place != null)
                             .Select(x => new
                             {
                                 Place = _mapper.Map<PlaceDataModel>(x.Place),
                                 Group = x.Group != null ? _mapper.Map<GroupDataModel>(x.Group) : null
                             })
                             .Distinct()
                             .ToList(),
                    Guides = g.Where(x => x.Guide != null)
                             .Select(x => _mapper.Map<GuideDataModel>(x.Guide))
                             .Distinct()
                             .ToList()
                })
                .ToList();

            return [.. groupedResults.Cast<object>()];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Excursion? GetExcursionById(string id, string? creatorId)
    {
        var query = _dbContext.Excursions.AsQueryable();

        if (creatorId is not null)
        {
            query = query.Where(x => x.UserId == creatorId);
        }

        return query.FirstOrDefault(x => x.Id == id);
    }
    private Excursion? GetExcursionByName(string name, string creatorId) => _dbContext.Excursions.Where(x => x.UserId == creatorId).FirstOrDefault(x => x.Name == name);

    public void UpdElement(ExcursionDataModel excursionDataModel)
    {
        try
        {
            var element = GetExcursionById(excursionDataModel.Id, excursionDataModel.UserId) ?? throw new
            ElementNotFoundException(excursionDataModel.Id);
            _dbContext.Excursions.Update(_mapper.Map(excursionDataModel, element));
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
}
