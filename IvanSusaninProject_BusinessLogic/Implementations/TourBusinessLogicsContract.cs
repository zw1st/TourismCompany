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

public class TourBusinessLogicsContract(ITourStorageContract tourStorageContract, ILogger logger) : ITourBusinessLogicsContract
{
    ITourStorageContract _tourStorageContract = tourStorageContract;
    private readonly ILogger _logger = logger;

    public List<TourDataModel> GetAllTours(string creatorId, DateTime dateTime)
    {
        _logger.LogInformation("GetAllPosts params");
        return _tourStorageContract.GetList(creatorId, dateTime) ?? throw new NullListException();
    }

    public TourDataModel GetTourById(string createrId, string id)
    {
        _logger.LogInformation("Get element by id: {id}", id);
        if (id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(id));
        }
        return _tourStorageContract.GetElementById(createrId, id) ?? throw new ElementNotFoundException(id);

    }

    public void InsertTour(TourDataModel tourDataModel)
    {
        _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(tourDataModel));
        ArgumentNullException.ThrowIfNull(tourDataModel);
        _tourStorageContract.AddElement(tourDataModel);
    }
}
