using AutoMapper;
using IvanSusaninProject_Contracts.AdapterContracts.OperationResponses;
using IvanSusaninProject_Contracts.AdapterContracts;
using IvanSusaninProject_Contracts.BindingModels;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;

namespace MVC_Project.Adapters;

public class GroupAdapter : IGroupAdapter
{
    private readonly IGroupBusinessLogicsContract _groupBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly Mapper _mapper;

    public GroupAdapter(IGroupBusinessLogicsContract groupBusinessLogicContract, ILogger<GroupAdapter> logger)
    {
        _groupBusinessLogicContract = groupBusinessLogicContract;
        _logger = logger;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GroupBindingModel, GroupDataModel>();
            cfg.CreateMap<GroupDataModel, GroupViewModel>();
            cfg.CreateMap<GroupDataModel, GroupBindingModel>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void ChangeGroupInfo(GroupBindingModel groupModel)
    {
        try
        {
            _groupBusinessLogicContract.UpdateGroup(_mapper.Map<GroupDataModel>(groupModel));
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public GroupBindingModel GetElement(string? creatorId, string id)
    {
        try
        {
            return _mapper.Map<GroupBindingModel>(_groupBusinessLogicContract.GetGroupById(creatorId, id));
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    public List<GroupViewModel> GetList(string? creatorId = null)
    {
        try
        {
            return [.. _groupBusinessLogicContract.GetAllGroups(creatorId).Select(x => _mapper.Map<GroupViewModel>(x))];
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving purchase list.", ex);
        }
    }

    public void LinkGroupWithPlace(string creatorId, string groupId, string placeId)
    {
        
        try
        {
            _groupBusinessLogicContract.LinkingGroupWithPlace(creatorId, groupId, placeId);
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving purchase list.", ex);
        }
    }

    public void RegisterGroup(GroupBindingModel groupModel)
    {
        try
        {
            _groupBusinessLogicContract.InsertGroup(_mapper.Map<GroupDataModel>(groupModel));
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving purchase list.", ex);
        }
    }

    public void RemoveGroup(string creatorId, string id)
    {
        try
        {
            _groupBusinessLogicContract.DeleteGroup(creatorId, id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving purchase list.", ex);
        }
    }
}