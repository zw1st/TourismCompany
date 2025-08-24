using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;

namespace MVC_Project.Adapters;

public class ExcursionAdapter : IExcursionAdapter
{
    private readonly IExcursionBusinessLogicsContract _excursionBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;


    public ExcursionAdapter(IExcursionBusinessLogicsContract excursionBusinessLogicContract, ILogger<ExcursionAdapter> logger)
    {
        _excursionBusinessLogicContract = excursionBusinessLogicContract;
        _logger = logger;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ExcursionBindingModel, ExcursionDataModel>();
            cfg.CreateMap<ExcursionDataModel, ExcursionViewModel>();
            cfg.CreateMap<ExcursionDataModel, ExcursionBindingModel>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public ExcursionBindingModel GetElement(string? creatorId, string data)
    {
        try
        {
            return _mapper.Map<ExcursionBindingModel>(_excursionBusinessLogicContract.GetExcursionByData(creatorId, data));
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving animal with data {data}.", ex);
        }
    }

    public List<ExcursionViewModel> GetList(string? creatorId = null, DateTime? dateTime = null, string? guideId = null)
    {
        try
        {
            return [.. _excursionBusinessLogicContract.GetAllExcursions(creatorId, dateTime, guideId).Select(x => _mapper.Map<ExcursionViewModel>(x))];

        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving excursion list.", ex);
        }
    }

    public void RegisterExcursion(ExcursionBindingModel excursionModel)
    {
        try
        {
            _excursionBusinessLogicContract.InsertExcursion(_mapper.Map<ExcursionDataModel>(excursionModel));
        }
        catch (Exception ex)
        {
            throw new Exception("Probably mapper problems.", ex);
        }
    }
}
