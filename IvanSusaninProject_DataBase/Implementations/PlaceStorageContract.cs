using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace IvanSusaninProject_DataBase.Implementations;

public class PlaceStorageContract : IPlaceStorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public PlaceStorageContract(IvanSusaninProject_DbContext dbContext)
    {
        var loggerFactory = NullLoggerFactory.Instance;
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Group, GroupDataModel>();

            cfg.CreateMap<Place, PlaceDataModel>();
            cfg.CreateMap<PlaceDataModel, Place>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void AddElement(PlaceDataModel placeDataModel)
    {
        try
        {
            _dbContext.Places.Add(_mapper.Map<Place>(placeDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelElement(string creatorId, string id)
    {
        try
        {
            var element = GetPlaceById(id, creatorId) ?? throw new ElementNotFoundException(id);
            _dbContext.Places.Remove(element);
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

    public PlaceDataModel? GetElementById(string? creatorId, string id)
    {
        try
        {
            return _mapper.Map<PlaceDataModel>(GetPlaceById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public PlaceDataModel? GetElementByName(string creatorId, string name)
    {
        try
        {
            return _mapper.Map<PlaceDataModel>(GetPlaceByName(name, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<PlaceDataModel> GetList(string? guarantorId, string? groupId = null)
    {
        try
        {
            var query = _dbContext.Places.Include(x => x.TripPlaces).Include(x => x.Group).AsQueryable();
            if (groupId is not null)
            {
                query = query.Where(x => x.GroupId == groupId);
            }
            if (guarantorId is not null)
            {
                query = query.Where(x => x.UserId == guarantorId);
            }
            return [.. query.Select(x => _mapper.Map<PlaceDataModel>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<PlaceDataModel> GetPlacesByTourIds(string guaranderId, List<string> tourIds)
    {
        try
        {
            var groupIds = _dbContext.TourGroups
                .Where(tg => tourIds.Contains(tg.TourId))
                .Select(tg => tg.GroupId)
                .Distinct()
                .ToList();

            return _dbContext.Places
                .Where(e => e.UserId == guaranderId && groupIds.Contains(e.GroupId))
                .Select(e => _mapper.Map<PlaceDataModel>(e))
                .ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<object> GetToursWithDetailsByPeriod(DateTime startDate, DateTime endDate, string executorId)
    {
        //try
        //{
        //    // Получаем поездки за указанный период
        //    var tours = _dbContext.Tours
        //        .Where(t => t.StartDate >= startDate &&
        //                   t.EndDate <= endDate &&
        //                   t.ExecutorId == executorId)
        //        .ToList();

        //    if (tours.Count == 0)
        //        return [];

        //    var toursIds = tours.Select(t => t.Id).ToList();

        //    // Получаем все связанные данные за один запрос
        //    var tourDetails = (
        //        from tour in tours
        //        join te in _dbContext.TourGroups on tour.Id equals te.TourId into tourGroups
        //        from tp in tourGroups.DefaultIfEmpty()
        //        join place in _dbContext.Places on tp?.PlaceId equals place.Id into places
        //        from place in places.DefaultIfEmpty()
        //        join groupData in _dbContext.Groups on place?.GroupId equals groupData.Id into groups
        //        from groupData in groups.DefaultIfEmpty()
        //        join tg in _dbContext.TripGuides on tour.Id equals tg.TripId into tripGuides
        //        from tg in tripGuides.DefaultIfEmpty()
        //        join guide in _dbContext.Guides on tg?.GuideId equals guide.Id into guides
        //        from guide in guides.DefaultIfEmpty()
        //        select new
        //        {
        //            Trip = tour,
        //            Place = place,
        //            Group = groupData,
        //            Guide = guide
        //        }
        //    ).ToList();

        //    // Группируем данные по поездкам
        //    var groupedResults = tourDetails
        //        .GroupBy(x => x.Trip.Id)
        //        .Select(g => new
        //        {
        //            Trip = _mapper.Map<TripDataModel>(g.First().Trip),
        //            Places = g.Where(x => x.Place != null)
        //                     .Select(x => new
        //                     {
        //                         Place = _mapper.Map<PlaceDataModel>(x.Place),
        //                         Group = x.Group != null ? _mapper.Map<GroupDataModel>(x.Group) : null
        //                     })
        //                     .Distinct()
        //                     .ToList(),
        //            Guides = g.Where(x => x.Guide != null)
        //                     .Select(x => _mapper.Map<GuideDataModel>(x.Guide))
        //                     .Distinct()
        //                     .ToList()
        //        })
        //        .ToList();

        //    return [.. groupedResults.Cast<object>()];
        //}
        //catch (Exception ex)
        //{
        //    _dbContext.ChangeTracker.Clear();
        //    throw new StorageException(ex);
        //}
        throw new NotImplementedException();
    }

    public void UpdElement(PlaceDataModel placeDataModel)
    {
        try
        {
            var element = GetPlaceById(placeDataModel.Id, placeDataModel.UserId) ?? throw new
            ElementNotFoundException(placeDataModel.Id);
            _dbContext.Places.Update(_mapper.Map(placeDataModel, element));
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

    private Place? GetPlaceById(string id, string? creatorId)
    {
        var query = _dbContext.Places.AsQueryable();

        if (creatorId is not null)
        {
            query = query.Where(x => x.UserId == creatorId);
        }

        return query.FirstOrDefault(x => x.Id == id);
    }

    private Place? GetPlaceByName(string name, string creatorId) => _dbContext.Places.Where(x => x.UserId == creatorId).FirstOrDefault(x => x.Name == name);
}
