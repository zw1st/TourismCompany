using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Contracts.ViewModels;
using IvanSusaninProject_DataBase.Implementations;
using IvanSusaninProject_Database.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class TripPlaceBusinessLogicContract : ITripPlaceBusinessLogicContract
{
    private readonly ILogger _logger;
    private ITripPlaceStorageContract _tripPlaceStorageContract;
    private ITripStorageContract _tripStorageContract;
    private IPlaceStorageContract _placeStorageContract;

    public TripPlaceBusinessLogicContract(ILogger logger, ITripPlaceStorageContract tripPlaceStorageContract, ITripStorageContract tripStorageContract, IPlaceStorageContract placeStorageContract)
    {
        _logger = logger;
        _tripPlaceStorageContract = tripPlaceStorageContract;
        _tripStorageContract = tripStorageContract;
        _placeStorageContract = placeStorageContract;
    }

    public void AddPlaceToTrip(string tripId, string placeId)
    {
        try
        {
            // Валидация входных параметров
            if (string.IsNullOrEmpty(tripId))
                throw new ArgumentNullException(nameof(tripId), "Tour ID cannot be null or empty");

            if (string.IsNullOrEmpty(placeId))
                throw new ArgumentNullException(nameof(placeId), "Group ID cannot be null or empty");

            // Проверяем существование тура
            var trip = _tripStorageContract.GetElementById(null, tripId);
            if (trip == null)
                throw new ArgumentException($"Tour with ID {tripId} not found", nameof(tripId));

            // Проверяем существование группы
            var excursion = _placeStorageContract.GetElementById(null, placeId);
            if (excursion == null)
                throw new ArgumentException($"Group with ID {placeId} not found", nameof(placeId));

            // Проверяем, не добавлена ли уже группа в тур
            var existingRelations = _tripPlaceStorageContract.GetByTripId(tripId);
            if (existingRelations.Any(rel => rel.PlaceId == placeId))
                throw new InvalidOperationException($"Group {placeId} already exists in tour {tripId}");

            // Создаем связь
            var tripPlace = new TripPlaceDataModel
            (
            placeId,
            tripId
            );
            _tripPlaceStorageContract.Create(tripPlace);

            _logger.LogInformation("Group {GroupId} successfully added to tour {TourId}", placeId, tripId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding group {GroupId} to tour {TourId}", placeId, tripId);
            throw new Exception($"Failed to add group to tour: {ex.Message}", ex);
        }
    }

    public List<PlaceViewModel> GetPlacesForTrip(string tripId)
    {
        try
        {
            // Валидация входного параметра
            if (string.IsNullOrEmpty(tripId))
                throw new ArgumentNullException(nameof(tripId), "Tour ID cannot be null or empty");

            // Получаем связи тур-группа
            var tripPlaces = _tripPlaceStorageContract.GetByTripId(tripId);
            if (!tripPlaces.Any())
            {
                _logger.LogInformation("No groups found for tour {TourId}", tripId);
                return new List<PlaceViewModel>();
            }

            // Получаем ID всех групп
            var placesIds = tripPlaces.Select(tg => tg.PlaceId).ToList();

            // Получаем детальную информацию о группах
            var groups = new List<PlaceDataModel>();

            foreach (var placeId in placesIds)
            {
                // Используем null для creatorId, так как нам нужны все группы без фильтрации по создателю
                var group = _placeStorageContract.GetElementById(null, placeId);
                if (group != null)
                {
                    groups.Add(group);
                }
            }

            // Маппим в GroupShortInfo
            var result = groups.Select(g => new PlaceViewModel
            {
                Id = g.Id,
                Name = g.Name,
                Address = g.Address,
                City = g.City,
                GroupName = g.GroupName
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
