using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class TourExcursionBusinessLogicContract : ITourExcursionBusinessLogicContract
{
    private readonly ILogger _logger;
    private ITourExcursionStorageContract _tourExcursionStorageContract;
    private ITourStorageContract _tourStorageContract;
    private IExcursionStorageContract _excursionStorageContract;

    public TourExcursionBusinessLogicContract(ILogger logger, ITourExcursionStorageContract tourExcursionStorageContract, ITourStorageContract tourStorageContract, IExcursionStorageContract excursionStorageContract)
    {
        _logger = logger;
        _tourExcursionStorageContract = tourExcursionStorageContract;
        _tourStorageContract = tourStorageContract;
        _excursionStorageContract = excursionStorageContract;
    }

    public void AddExcursionToTour(string tourId, string excursionId)
    {
        try
        {
            // Валидация входных параметров
            if (string.IsNullOrEmpty(tourId))
                throw new ArgumentNullException(nameof(tourId), "Tour ID cannot be null or empty");

            if (string.IsNullOrEmpty(excursionId))
                throw new ArgumentNullException(nameof(excursionId), "Group ID cannot be null or empty");

            // Проверяем существование тура
            var tour = _tourStorageContract.GetElementById(null, tourId);
            if (tour == null)
                throw new ArgumentException($"Tour with ID {tourId} not found", nameof(tourId));

            // Проверяем существование группы
            var excursion = _excursionStorageContract.GetElementById(null, excursionId);
            if (excursion == null)
                throw new ArgumentException($"Group with ID {excursionId} not found", nameof(excursionId));

            // Проверяем, не добавлена ли уже группа в тур
            var existingRelations = _tourExcursionStorageContract.GetByTourId(tourId);
            if (existingRelations.Any(rel => rel.ExcursionId == excursionId))
                throw new InvalidOperationException($"Group {excursionId} already exists in tour {tourId}");

            // Создаем связь
            var tourExcursion = new TourExcursionDataModel
            (
                tourId,
                excursionId
            );

            _tourExcursionStorageContract.Create(tourExcursion);

            _logger.LogInformation("Group {GroupId} successfully added to tour {TourId}", excursionId, tourId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding group {GroupId} to tour {TourId}", excursionId, tourId);
            throw new Exception($"Failed to add group to tour: {ex.Message}", ex);
        }
    }

    public List<ExcursionShortInfo> GetExcursionForTour(string tourId)
    {
        try
        {
            // Валидация входного параметра
            if (string.IsNullOrEmpty(tourId))
                throw new ArgumentNullException(nameof(tourId), "Tour ID cannot be null or empty");

            // Получаем связи тур-группа
            var tourGroups = _tourExcursionStorageContract.GetByTourId(tourId);
            if (!tourGroups.Any())
            {
                _logger.LogInformation("No groups found for tour {TourId}", tourId);
                return new List<ExcursionShortInfo>();
            }

            // Получаем ID всех групп
            var groupIds = tourGroups.Select(tg => tg.ExcursionId).ToList();

            // Получаем детальную информацию о группах
            var groups = new List<ExcursionDataModel>();

            foreach (var groupId in groupIds)
            {
                // Используем null для creatorId, так как нам нужны все группы без фильтрации по создателю
                var group = _excursionStorageContract.GetElementById(null, groupId);
                if (group != null)
                {
                    groups.Add(group);
                }
            }

            // Маппим в GroupShortInfo
            var result = groups.Select(g => new ExcursionShortInfo
            {
                Id = g.Id,
                Name = g.Name,
                ExcursionDate = g.ExcursionDate
            }).ToList();

            _logger.LogInformation("Retrieved {Count} groups for tour {TourId}", result.Count, tourId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving groups for tour {TourId}", tourId);
            throw new Exception($"Failed to get groups for tour: {ex.Message}", ex);
        }
    }

}
