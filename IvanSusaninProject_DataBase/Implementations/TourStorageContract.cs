using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ReportModels;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;
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
            //---
            cfg.CreateMap<Excursion, ExcursionDataModel>();
            cfg.CreateMap<ExcursionDataModel, Excursion>();

            cfg.CreateMap<Group, GroupDataModel>();
            cfg.CreateMap<GroupDataModel, Group>();

            cfg.CreateMap<Guide, GuideDataModel>();
            cfg.CreateMap<GuideDataModel, Guide>();
        }, loggerFactory)
        {

        };
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

    public TourDataModel? GetElementById(string? creatorId, string id)
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

    private Tour? GetTourById(string id, string creatorId)
    {
        var query = _dbContext.Tours.AsQueryable();
        if (creatorId is not null)
        {
            query = query.Where(x => x.UserId == creatorId);
        }
        return query.FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<TourPlacesDto>> GePlacesByTourIds(List<string> tourIds, CancellationToken ct)
    {
        try
        {
            var groupTours = await _dbContext.TourGroups
                .Where(tg => tourIds.Contains(tg.TourId))
                .Include(tg => tg.Tour)
                .Select(tg => new
                {
                    tg.GroupId,
                    TourId = tg.TourId,
                    TourName = tg.Tour.Name
                })
                .ToListAsync(ct);

            var places = await _dbContext.Places
                .Where(e => groupTours.Select(gt => gt.GroupId).Contains(e.GroupId))
                .ToListAsync(ct);

            var result = groupTours
                .GroupBy(gt => new { gt.TourId, gt.TourName })
                .Select(g => new TourPlacesDto
                {
                    TourId = g.Key.TourId,
                    TourName = g.Key.TourName,
                    Places = places
                        .Where(e => g.Any(gt => gt.GroupId == e.GroupId))
                        .Select(e => e.Name)
                        .Distinct()
                        .ToList()
                })
                .Where(x => x.Places.Any())
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public async Task<List<TourDetailsDto>> GetToursWithDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        try
        {
            var query =
                from tour in _dbContext.Tours
                where tour.StartDate >= startDate
                      && tour.EndDate <= endDate
                join tp in _dbContext.TourExcursions on tour.Id equals tp.TourId into tourExcursions
                from tp in tourExcursions.DefaultIfEmpty()
                join excursion in _dbContext.Excursions on tp.ExcursionId equals excursion.Id into excursions
                from excursion in excursions.DefaultIfEmpty()
                join guide in _dbContext.Guides on excursion.GuideId equals guide.Id into guides
                from guide in guides.DefaultIfEmpty()
                join tg in _dbContext.TourGroups on tour.Id equals tg.TourId into tourGroups
                from tg in tourGroups.DefaultIfEmpty()
                join groupdata in _dbContext.Groups on tg.GroupId equals groupdata.Id into groups
                from groupdata in groups.DefaultIfEmpty()
                select new
                {
                    Tour = tour,
                    Excursion = excursion,
                    Group = groupdata,
                    Guide = guide
                };

            var tourDetails = await query.ToListAsync(ct);

            if (tourDetails.Count == 0)
                return new List<TourDetailsDto>();

            var groupedResults = tourDetails
                .GroupBy(x => x.Tour.Id)
                .Select(g => new TourDetailsDto
                {
                    Tour = _mapper.Map<TourDataModel>(g.First().Tour),
                    Excursions = g.Where(x => x.Excursion != null)
                             .Select(x => new ExcursionWithGuideDto
                             {
                                 Excursion = _mapper.Map<ExcursionDataModel>(x.Excursion!),
                                 Guide = x.Guide != null ? _mapper.Map<GuideDataModel>(x.Guide) : null
                             })
                             .Distinct()
                             .ToList(),
                    Groups = g.Where(x => x.Group != null)
                             .Select(x => _mapper.Map<GroupDataModel>(x.Group!))
                             .Distinct()
                             .ToList()
                })
                .ToList();

            return groupedResults;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }
}
