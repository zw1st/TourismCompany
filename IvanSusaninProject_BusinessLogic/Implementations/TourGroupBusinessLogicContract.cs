using DocumentFormat.OpenXml.Office2010.Excel;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
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

public class TourGroupBusinessLogicContract : ITourGroupBusinessLogicContract
{
    private readonly ILogger _logger;
    private ITourGroupStorageContract _tourGroupStorageContract;
    private ITourStorageContract _tourStorageContract;
    private IGroupStorageContract _groupStorageContract;

    public TourGroupBusinessLogicContract(ILogger logger, ITourGroupStorageContract tourGroupStorageContract, ITourStorageContract tourStorageContract = null, IGroupStorageContract groupStorageContract = null)
    {
        _logger = logger;
        _tourGroupStorageContract = tourGroupStorageContract;
        _tourStorageContract = tourStorageContract;
        _groupStorageContract = groupStorageContract;
    }

    public void AddGroupToTour(string tourId, string groupId)
    {
        try
        {
            // Валидация входных параметров
            if (string.IsNullOrEmpty(tourId))
                throw new ArgumentNullException(nameof(tourId), "Tour ID cannot be null or empty");

            if (string.IsNullOrEmpty(groupId))
                throw new ArgumentNullException(nameof(groupId), "Group ID cannot be null or empty");

            // Проверяем существование тура
            var tour = _tourStorageContract.GetElementById(null, tourId);
            if (tour == null)
                throw new ArgumentException($"Tour with ID {tourId} not found", nameof(tourId));

            // Проверяем существование группы
            var group = _groupStorageContract.GetElementById(null, groupId);
            if (group == null)
                throw new ArgumentException($"Group with ID {groupId} not found", nameof(groupId));

            // Проверяем, не добавлена ли уже группа в тур
            var existingRelations = _tourGroupStorageContract.GetByTourId(tourId);
            if (existingRelations.Any(rel => rel.GroupId == groupId))
                throw new InvalidOperationException($"Group {groupId} already exists in tour {tourId}");

            // Создаем связь
            var tourGroup = new TourGroupDataModel
            (
                tourId,
                groupId
            );

            _tourGroupStorageContract.Create(tourGroup);

            _logger.LogInformation("Group {GroupId} successfully added to tour {TourId}", groupId, tourId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding group {GroupId} to tour {TourId}", groupId, tourId);
            throw new Exception($"Failed to add group to tour: {ex.Message}", ex);
        }
    }

    public List<GroupShortInfo> GetGroupsForTour(string tourId)
    {
        try
        {
            // Валидация входного параметра
            if (string.IsNullOrEmpty(tourId))
                throw new ArgumentNullException(nameof(tourId), "Tour ID cannot be null or empty");

            // Получаем связи тур-группа
            var tourGroups = _tourGroupStorageContract.GetByTourId(tourId);
            if (!tourGroups.Any())
            {
                _logger.LogInformation("No groups found for tour {TourId}", tourId);
                return new List<GroupShortInfo>();
            }

            // Получаем ID всех групп
            var groupIds = tourGroups.Select(tg => tg.GroupId).ToList();

            // Получаем детальную информацию о группах
            var groups = new List<GroupDataModel>();

            foreach (var groupId in groupIds)
            {
                // Используем null для creatorId, так как нам нужны все группы без фильтрации по создателю
                var group = _groupStorageContract.GetElementById(null, groupId);
                if (group != null)
                {
                    groups.Add(group);
                }
            }

            // Маппим в GroupShortInfo
            var result = groups.Select(g => new GroupShortInfo
            {
                Id = g.Id,
                Name = g.Name,
                HumanAmount = g.HumanAmount,
                HumanType = g.HumanType
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
