using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Contracts.ViewModels;
using IvanSusaninProject_DataBase.Implementations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class TripGuideBusinessLogicContract : ITripGuideBusinessLogicContract
{
    private readonly ILogger _logger;
    private IGuideStrorageContract _guideStoragesContract;
    private ITripStorageContract _tripStorageContract;
    private ITripGuideStorageContract _tripGuideStorageContract;

    public TripGuideBusinessLogicContract(ILogger logger, IGuideStrorageContract guideStoragesContract, ITripStorageContract tripStoragesContract, ITripGuideStorageContract tripGuideStorageContract)
    {
        _logger = logger;
        _guideStoragesContract = guideStoragesContract;
        _tripStorageContract = tripStoragesContract;
        _tripGuideStorageContract = tripGuideStorageContract;
    }

    public void AddGuideToTrip(string tripId, string guideId)
    {
        try
        {
            // Валидация входных параметров
            if (string.IsNullOrEmpty(tripId))
                throw new ArgumentNullException(nameof(tripId), "Tour ID cannot be null or empty");

            if (string.IsNullOrEmpty(guideId))
                throw new ArgumentNullException(nameof(guideId), "Group ID cannot be null or empty");

            // Проверяем существование тура
            var trip = _tripStorageContract.GetElementById(null, tripId);
            if (trip == null)
                throw new ArgumentException($"Tour with ID {tripId} not found", nameof(tripId));

            // Проверяем существование группы
            var excursion = _guideStoragesContract.GetElementById(null, guideId);
            if (excursion == null)
                throw new ArgumentException($"Group with ID {guideId} not found", nameof(guideId));

            // Проверяем, не добавлена ли уже группа в тур
            var existingRelations = _tripGuideStorageContract.GetByTripId(tripId);
            if (existingRelations.Any(rel => rel.GuideId == guideId))
                throw new InvalidOperationException($"Group {guideId} already exists in tour {tripId}");

            // Создаем связь
            var tripPlace = new TripGuideDataModel
            (
            tripId,
            guideId
            );
            _tripGuideStorageContract.Create(tripPlace);

            _logger.LogInformation("Group {GroupId} successfully added to tour {TourId}", guideId, tripId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding group {GroupId} to tour {TourId}", guideId, tripId);
            throw new Exception($"Failed to add group to tour: {ex.Message}", ex);
        }
    }

    public List<GuideViewModel> GetGuidesForTrip(string tripId)
    {
        try
        {
            // Валидация входного параметра
            if (string.IsNullOrEmpty(tripId))
                throw new ArgumentNullException(nameof(tripId), "Tour ID cannot be null or empty");

            // Получаем связи тур-группа
            var tripGuides = _tripGuideStorageContract.GetByTripId(tripId);
            if (!tripGuides.Any())
            {
                _logger.LogInformation("No groups found for tour {TourId}", tripId);
                return new List<GuideViewModel>();
            }

            // Получаем ID всех групп
            var guidesIds = tripGuides.Select(tg => tg.GuideId).ToList();

            // Получаем детальную информацию о группах
            var guides = new List<GuideDataModel>();

            foreach (var guideId in guidesIds)
            {
                var group = _guideStoragesContract.GetElementById(null, guideId);
                if (group != null)
                {
                    guides.Add(group);
                }
            }

            // Маппим в GroupShortInfo
            var result = guides.Select(g => new GuideViewModel
            {
                Id = g.Id,
                Fio = g.Fio,
                Age = g.Age,
                Experience = g.Experience
            }).ToList();

            _logger.LogInformation("Retrieved {Count} groups for tour {TourId}", result.Count, tripId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving groups for tour {TourId}", tripId);
            throw new Exception($"Failed to get groups for tour: {ex.Message}", ex);
        }
    }
}
