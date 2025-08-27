using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts;

using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MVC_Project.Adapters;

public class GuideAdapter : IGuideAdapter
{
    private readonly IGuideBusinessLogicsContract _guideBusinessLogicsContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public GuideAdapter(IGuideBusinessLogicsContract guideBusinessLogicsContract, ILogger<GuideAdapter> logger)
    {
        _guideBusinessLogicsContract = guideBusinessLogicsContract;
        _logger = logger;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GuideBindingModel, GuideDataModel>();
            cfg.CreateMap<GuideDataModel, GuideViewModel>();
            cfg.CreateMap<GuideDataModel, GuideBindingModel>();

        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void ChangeGuideInfo(GuideBindingModel model)
    {
        try
        {
            _guideBusinessLogicsContract.UpdateGuide(_mapper.Map<GuideDataModel>(model));
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public GuideBindingModel GetElement(string creatorId, string data)
    {
        try
        {
            return _mapper.Map<GuideBindingModel>(_guideBusinessLogicsContract.GetGuideByData(creatorId, data));
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public List<GuideViewModel> GetList(string? creatorId = null)
    {
        try
        {
            return [.. _guideBusinessLogicsContract.GetAllGuides(creatorId).Select(x => _mapper.Map<GuideViewModel>(x))];
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public void LinkGuideToExcursion(string creatorId, string guideId, string excursionId)
    {
        try
        {
            _guideBusinessLogicsContract.LinkingGuideToExcursion(creatorId, guideId, excursionId);
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public void RegisterGuide(GuideBindingModel model)
    {
        try
        {
            _guideBusinessLogicsContract.InsertGuide(_mapper.Map<GuideDataModel>(model));
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public void RemoveGuide(string creatorId, string id)
    {
        try
        {
            _guideBusinessLogicsContract.DeleteGuide(creatorId , id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }
}