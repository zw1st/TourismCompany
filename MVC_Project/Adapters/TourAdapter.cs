using AutoMapper;

using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using IvanSusaninProject_Database;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_Contracts.StorageContracts;

namespace MVC_Project.Adapters;

public class TourAdapter : ITourAdapter
{
    private readonly IvanSusaninProject_DbContext _dbContext;
    private readonly ITourBusinessLogicsContract _tourBusinessLogicContract;
    private readonly IGroupBusinessLogicsContract _groupBusinessLogicsContract;
    private readonly IExcursionBusinessLogicsContract _excursionBusinessLogicsContract;
    private readonly ITourGroupBusinessLogicContract _tourGroupBusinessLogicContract;
    private readonly ITourExcursionBusinessLogicContract _tourExcursionBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public TourAdapter(IvanSusaninProject_DbContext dbContext, ITourBusinessLogicsContract tourBusinessLogicContract, IGroupBusinessLogicsContract groupBusinessLogicsContract, IExcursionBusinessLogicsContract excursionBusinessLogicsContract, ITourGroupBusinessLogicContract tourGroupBusinessLogicContract, ITourExcursionBusinessLogicContract tourExcursionStorageContract, ILogger logger)
    {
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TourBindingModel, TourDataModel>();
            cfg.CreateMap<TourDataModel, TourViewModel>();

            cfg.CreateMap<TourDataModel, TourBindingModel>();


            cfg.CreateMap<TourGroupDataModel, TourGroup>();
            cfg.CreateMap<TourExcursionDataModel, TourExcursion>();

        }, loggerFactory);
        _dbContext = dbContext;
        _tourBusinessLogicContract = tourBusinessLogicContract;
        _groupBusinessLogicsContract = groupBusinessLogicsContract;
        _excursionBusinessLogicsContract = excursionBusinessLogicsContract;
        _tourGroupBusinessLogicContract = tourGroupBusinessLogicContract;
        _tourExcursionBusinessLogicContract = tourExcursionStorageContract;
        _logger = logger;
        _mapper = new Mapper(config);
    }

    public TourViewModel GetElement(string? creatorId, string tourId)
    {
        try
        {
            var tour = _tourBusinessLogicContract.GetTourById(creatorId, tourId);
            if (tour == null)
                return null;

            // 2. Маппим основные свойства
            var viewModel = _mapper.Map<TourViewModel>(tour);

            // 3. Явно загружаем группы
                viewModel.Groups = _tourGroupBusinessLogicContract.GetGroupsForTour(tourId);

                // 4. Явно загружаем экскурсии
                viewModel.Excursions = _tourExcursionBusinessLogicContract.GetExcursionForTour(tourId);

            return viewModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tour with details. TourId: {TourId}, CreatorId: {CreatorId}",
                tourId, creatorId);
            throw new Exception("Error getting tour details", ex);
        }
    }

    public List<TourViewModel> GetList(string? creatorId = null, DateTime? date = null)
    {
        try
        {
            return [.. _tourBusinessLogicContract.GetAllTours(creatorId, date).Select(x => _mapper.Map<TourViewModel>(x))];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            throw new Exception("Error", ex);
        }
    }

    public void RegisterTour(TourBindingModel tourModel)
    {
        try
        {
            _tourBusinessLogicContract.InsertTour(_mapper.Map<TourDataModel>(tourModel));
        }
        catch (MyValidationException ex)
        {
            _logger.LogError(ex, "MyValidationException");
            throw new MyValidationException("Error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            throw new Exception("Error", ex);
        }
    }

    public void RegisterTourWithRelations(TourBindingModel model, List<string> groupIds, List<string> excursionIds)
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            // 1. Создаем основной тур (без связей)
            var tourId = model.Id;
            RegisterTour(model);

            // 2. Добавляем связи с группами
            if (groupIds != null && groupIds.Any())
            {
                foreach (var groupId in groupIds.Where(id => !string.IsNullOrEmpty(id)))
                {
                    var group = _groupBusinessLogicsContract.GetGroupById(null, groupId);
                    var tourGroup = new TourGroupDataModel(
                        tourId: tourId,
                        groupId: groupId
                    );
                    _dbContext.TourGroups.Add(_mapper.Map<TourGroup>(tourGroup));
                }
            }

            // 3. Добавляем связи с экскурсиями
            if (excursionIds != null && excursionIds.Any())
            {
                foreach (var excursionId in excursionIds.Where(id => !string.IsNullOrEmpty(id)))
                {
                    var excursion = _excursionBusinessLogicsContract.GetExcursionByData(null, excursionId);
                    var tourExcursion = new TourExcursionDataModel(
                        tourId: tourId,
                        excursionId: excursionId
                    );
                    _dbContext.TourExcursions.Add(_mapper.Map<TourExcursion>(tourExcursion));
                }
            }

            _dbContext.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public List<TourViewModel> GetListWithDetails(string userId)
    {
        // 1. Получаем все туры
        var tours = _dbContext.Tours.Where(t => t.UserId == userId)
            .AsNoTracking()
            .ToList();

        if (!tours.Any())
            return new List<TourViewModel>();

        var tourIds = tours.Select(t => t.Id).ToList();

        // 2. Получаем всех пользователей для туров
        var userIds = tours.Select(t => t.UserId).Distinct().ToList();
        var users = _dbContext.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToDictionary(u => u.Id, u => u.Login);

        // 3. Получаем все связи с группами
        var tourGroups = _dbContext.TourGroups
            .AsNoTracking()
            .Where(tg => tourIds.Contains(tg.TourId))
            .ToList();

        var groupIds = tourGroups.Select(tg => tg.GroupId).Distinct().ToList();
        var groupsDict = _dbContext.Groups
            .AsNoTracking()
            .Where(g => groupIds.Contains(g.Id))
            .ToDictionary(g => g.Id);

        // 4. Получаем все связи с экскурсиями
        var tourExcursions = _dbContext.TourExcursions
            .AsNoTracking()
            .Where(te => tourIds.Contains(te.TourId))
            .ToList();

        var excursionIds = tourExcursions.Select(te => te.ExcursionId).Distinct().ToList();
        var excursionsDict = _dbContext.Excursions
            .AsNoTracking()
            .Where(e => excursionIds.Contains(e.Id))
            .ToDictionary(e => e.Id);

        // 5. Собираем результат
        var result = new List<TourViewModel>();

        foreach (var tour in tours)
        {
            var tourGroupsForTour = tourGroups
                .Where(tg => tg.TourId == tour.Id)
                .ToList();

            var tourExcursionsForTour = tourExcursions
                .Where(te => te.TourId == tour.Id)
                .ToList();

            result.Add(new TourViewModel
            {
                Id = tour.Id,
                Name = tour.Name,
                City = tour.City,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                UserId = tour.UserId,
                UserLogin = users.TryGetValue(tour.UserId, out var login) ? login : "Неизвестный",
                Groups = tourGroupsForTour
                    .Select(tg => groupsDict.TryGetValue(tg.GroupId, out var group) ? group : null)
                    .Where(g => g != null)
                    .Select(g => new GroupShortInfo
                    {
                        Id = g.Id,
                        Name = g.Name,
                        HumanAmount = g.HumanAmount,
                        HumanType = g.HumanType
                    })
                    .ToList(),
                Excursions = tourExcursionsForTour
                    .Select(te => excursionsDict.TryGetValue(te.ExcursionId, out var excursion) ? excursion : null)
                    .Where(e => e != null)
                    .Select(e => new ExcursionShortInfo
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ExcursionDate = e.ExcursionDate
                    })
                    .ToList()
            });
        }

        return result;
    }
}