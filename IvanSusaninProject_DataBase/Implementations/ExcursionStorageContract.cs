using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ReportModels;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            //---

            cfg.CreateMap<TripGuide, TripGuideDataModel>()
                .ConstructUsing(src => new TripGuideDataModel(src.TripId, src.GuideId));

            cfg.CreateMap<TripGuideDataModel, TripGuide>()
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
                .ForMember(dest => dest.GuideId, opt => opt.MapFrom(src => src.GuideId))
                .ForMember(dest => dest.Trip, opt => opt.Ignore())
                .ForMember(dest => dest.Guide, opt => opt.Ignore());

            // Конфигурация маппинга для TripPlace
            cfg.CreateMap<TripPlace, TripPlaceDataModel>()
                .ConstructUsing(src => new TripPlaceDataModel(src.PlaceId, src.TripId));

            cfg.CreateMap<TripPlaceDataModel, TripPlace>()
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId));
                //.ForMember(dest => dest.PlaceId, opt => opt.MapFrom(src => src.PlaceId))
                //.ForMember(dest => dest.Trip, opt => opt.Ignore())
                //.ForMember(dest => dest.Place, opt => opt.Ignore());

            // Конфигурация маппинга для Trip
            cfg.CreateMap<Trip, TripDataModel>()
                .ForMember(dest => dest.TripPlaces, opt => opt.MapFrom(src => src.TripPlaces))
                .ForMember(dest => dest.TripGuides, opt => opt.MapFrom(src => src.TripGuides));

            cfg.CreateMap<TripDataModel, Trip>()
                .ForMember(dest => dest.TripPlaces, opt => opt.Ignore())
                .ForMember(dest => dest.TripGuides, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid().ToString()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.StartCity, opt => opt.MapFrom(src => src.StartCity))
                .ForMember(dest => dest.EndCity, opt => opt.MapFrom(src => src.EndCity))
                .ForMember(dest => dest.TripDate, opt => opt.MapFrom(src => src.TripDate))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));

            cfg.CreateMap<Place, PlaceDataModel>();
            cfg.CreateMap<PlaceDataModel, Place>();

            cfg.CreateMap<Group, GroupDataModel>();
            cfg.CreateMap<GroupDataModel, Group>();

            cfg.CreateMap<Guide, GuideDataModel>();
            cfg.CreateMap<GuideDataModel, Guide>();
        }, loggerFactory)
        {

        };
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

    public ExcursionDataModel? GetElementByName(string? creatorId, string name)
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

    public async Task<List<TripExcursionDto>> GetExcursionsByTourIds(List<string> tripIds, CancellationToken ct)
    {
        try
        {
            // Получаем гидов и их поездки
            var guideTrips = await _dbContext.TripGuides
                .Where(tg => tripIds.Contains(tg.TripId))
                .Include(tg => tg.Trip)
                .Select(tg => new
                {
                    tg.GuideId,
                    TripId = tg.TripId,
                    StartCity = tg.Trip.StartCity,
                    EndCity = tg.Trip.EndCity
                })
                .ToListAsync(ct);

            // Получаем экскурсии
            var excursions = await _dbContext.Excursions
                .Where(e => guideTrips.Select(gt => gt.GuideId).Contains(e.GuideId))
                .ToListAsync(ct);

            // Группируем по поездкам
            var result = guideTrips
                .GroupBy(gt => new { gt.TripId, gt.StartCity, gt.EndCity })
                .Select(g => new TripExcursionDto
                {
                    TripId = g.Key.TripId,
                    TripName = $"{g.Key.StartCity} - {g.Key.EndCity}",
                    Excursions = excursions
                        .Where(e => g.Any(gt => gt.GuideId == e.GuideId))
                        .Select(e => e.Name)
                        .Distinct()
                        .ToList()
                })
                .Where(x => x.Excursions.Any())
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }
    public async Task<List<TripDetailsDto>> GetTripsWithDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        try
        {
            var query =
                from trip in _dbContext.Trips
                where trip.TripDate >= startDate
                      && trip.TripDate <= endDate
                join tp in _dbContext.TripPlaces on trip.Id equals tp.TripId into tripPlaces
                from tp in tripPlaces.DefaultIfEmpty()
                join place in _dbContext.Places on tp.PlaceId equals place.Id into places
                from place in places.DefaultIfEmpty()
                join groupData in _dbContext.Groups on place.GroupId equals groupData.Id into groups
                from groupData in groups.DefaultIfEmpty()
                join tg in _dbContext.TripGuides on trip.Id equals tg.TripId into tripGuides
                from tg in tripGuides.DefaultIfEmpty()
                join guide in _dbContext.Guides on tg.GuideId equals guide.Id into guides
                from guide in guides.DefaultIfEmpty()
                select new
                {
                    Trip = trip,
                    Place = place,
                    Group = groupData,
                    Guide = guide
                };

            var tripDetails = await query.ToListAsync(ct);

            if (tripDetails.Count == 0)
                return new List<TripDetailsDto>();

            var groupedResults = tripDetails
                .GroupBy(x => x.Trip.Id)
                .Select(g => new TripDetailsDto
                {
                    Trip = _mapper.Map<TripDataModel>(g.First().Trip),
                    Places = g.Where(x => x.Place != null)
                             .Select(x => new PlaceWithGroupDto
                             {
                                 Place = _mapper.Map<PlaceDataModel>(x.Place!),
                                 Group = x.Group != null ? _mapper.Map<GroupDataModel>(x.Group) : null
                             })
                             .Distinct()
                             .ToList(),
                    Guides = g.Where(x => x.Guide != null)
                             .Select(x => _mapper.Map<GuideDataModel>(x.Guide!))
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


    private Excursion? GetExcursionById(string id, string? creatorId)
    {
        var query = _dbContext.Excursions.AsQueryable();

        if (creatorId is not null)
        {
            query = query.Where(x => x.UserId == creatorId);
        }

        return query.FirstOrDefault(x => x.Id == id);
    }
    private Excursion? GetExcursionByName(string name, string? creatorId)
    {
        var query = _dbContext.Excursions.AsQueryable();
        if (creatorId is not null)
        {
            query = query.Where(x => x.UserId == creatorId);
        }
        return query.FirstOrDefault(x => x.Name == name);
    }

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
