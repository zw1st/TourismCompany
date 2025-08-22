using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class GuideBusinessLogicsContract(IGuideStrorageContract guideStrorageContract, IExcursionStorageContract excursionStorageContract, ILogger logger) : IGuideBusinessLogicsContract
{
    private readonly ILogger _logger = logger;

    private readonly IGuideStrorageContract _guideStorageContract = guideStrorageContract;

    private readonly IExcursionStorageContract _excursionStorageContract = excursionStorageContract;

    public void DeleteGuide(string creatorId, string id)
    {
        _logger.LogInformation("Delete by id: {creatorId} {id}", creatorId, id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
       /* if (!id.IsGuid())
        {
            throw new MyMyValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyMyValidationException("Id is not a unique identifier");
        }*/
        _guideStorageContract.DelElement(creatorId, id);
    }

    public List<GuideDataModel> GetAllGuides(string? creatorId = null)
    {
        _logger.LogInformation("GetAllGuides params: {creatorId}", creatorId);
        //if (creatorId.IsEmpty())
        //{
        //    throw new ArgumentNullException(nameof(creatorId));
        //}
        /*if (!creatorId.IsGuid())
        {
            throw new MyMyValidationException("Id is not a unique identifier");
        }*/
        return _guideStorageContract.GetList(creatorId) ?? throw new NullListException();
    }

    public GuideDataModel GetGuideByData(string creatorId, string data)
    {
        _logger.LogInformation("Get element by data: {creatorId}, {data}", creatorId, data);
        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        /*if (!data.IsGuid())
        {
            throw new MyMyValidationException("Id is not a unique identifier");
        }
        if (creatorId.IsEmpty())
        {
            throw new ArgumentNullException(nameof(creatorId));
        }
        if (!creatorId.IsGuid())
        {
            throw new MyMyValidationException("Id is not a unique identifier");
        }*/
        return _guideStorageContract.GetElementById(creatorId, data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertGuide(GuideDataModel model)
    {
        _logger.LogInformation("New data: {json}",
        JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _guideStorageContract.AddElement(model);
    }

    public void UpdateGuide(GuideDataModel model)
    {
        _logger.LogInformation("Update data: {json}",
        JsonSerializer.Serialize(model));
        ArgumentNullException.ThrowIfNull(model);
        _guideStorageContract.UpdElement(model);
    }

    public void LinkingGuideToExcursion(string creatorId, string guideId, string excursionId)
    {
        if (string.IsNullOrEmpty(creatorId))
            throw new ArgumentNullException(nameof(creatorId));

        if (string.IsNullOrEmpty(guideId))
            throw new ArgumentNullException(nameof(guideId));

        if (string.IsNullOrEmpty(excursionId))
            throw new ArgumentNullException(nameof(excursionId));

        // Получаем экскурсию
        var excursion = _excursionStorageContract.GetElementById(null,  excursionId);
        if (excursion == null)
            throw new ElementNotFoundException(excursionId);

        // Проверяем существование гида
        var guide = _guideStorageContract.GetElementById(creatorId, guideId);
        if (guide == null)
        {
            // Если гида не существует, устанавливаем GuideId в null
            excursion.GuideId = null;
        }
        else
        {
            // Если гид существует, устанавливаем связь
            excursion.GuideId = guideId;
        }

        // Обновляем экскурсию
        _excursionStorageContract.UpdElement(excursion);
    }
}