using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.StorageContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class ExcursionBusinessLogicsContracts(IExcursionStorageContract excursionStorageContract, ILogger logger) : IExcursionBusinessLogicsContract
{
    IExcursionStorageContract _excursionStorageContract = excursionStorageContract;
    private readonly ILogger _logger = logger;

    public List<ExcursionDataModel> GetAllExcursions(string? creatorId, DateTime? dateTime, string? guideId)
    {
        _logger.LogInformation("GetAllPosts params");
        return _excursionStorageContract.GetList(creatorId, dateTime, guideId) ?? throw new NullListException();
    }

    public ExcursionDataModel GetExcursionByData(string? creatorId, string data)
    {
        _logger.LogInformation("Get element by data: {data}", data);
        if (data.IsEmpty())
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data.IsGuid())
        {
            return _excursionStorageContract.GetElementById(creatorId, data) ?? throw new ElementNotFoundException(data);
        }
        return _excursionStorageContract.GetElementByName(creatorId, data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertExcursion(ExcursionDataModel excursionDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(excursionDataModel));
        ArgumentNullException.ThrowIfNull(excursionDataModel);
        _excursionStorageContract.AddElement(excursionDataModel);
    }
}
