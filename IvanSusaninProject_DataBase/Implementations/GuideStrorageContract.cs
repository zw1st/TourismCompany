
using AutoMapper;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.StorageContracts;
using IvanSusaninProject_Database;
using IvanSusaninProject_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace IvanSusaninProject_DataBase.Implementations;

public class GuideStrorageContract : IGuideStrorageContract
{
    private readonly IvanSusaninProject_DbContext _dbContext;

    private readonly Mapper _mapper;

    public GuideStrorageContract(IvanSusaninProject_DbContext dbContext)
    {
        _dbContext = dbContext;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Guide, GuideDataModel>();
            cfg.CreateMap<GuideDataModel, Guide>();
        }, loggerFactory);
        _mapper = new Mapper(config);
    }

    public void AddElement(GuideDataModel guideDataModel)
    {
        try
        {

            _dbContext.Guides.Add(_mapper.Map<Guide>(guideDataModel));
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelElement(string creatorId, string id)
    {
        try
        {
            var element = GetGuideById(id, creatorId) ?? throw new ElementNotFoundException(id);
            _dbContext.Guides.Remove(element);
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public GuideDataModel? GetElementByFIO(string creatorId, string fio)
    {
        try
        {
            return _mapper.Map<GuideDataModel>(GetGuideByFIO(fio, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public GuideDataModel? GetElementById(string creatorId, string id)
    {
        try
        {
            return _mapper.Map<GuideDataModel>(GetGuideById(id, creatorId));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }


    public List<GuideDataModel> GetList(string? guarantorId = null)
    {
        try
        {
            var query = _dbContext.Guides.Include(x => x.TripGuides).AsQueryable();

            if (!string.IsNullOrEmpty(guarantorId))
            {
                query = query.Where(x => x.UserId == guarantorId);
            }

            return query.Select(x => _mapper.Map<GuideDataModel>(x)).ToList();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(GuideDataModel guideDataModel)
    {
        try
        {
            var element = GetGuideById(guideDataModel.Id, guideDataModel.UserId) ?? throw new ElementNotFoundException(guideDataModel.Id);
            _dbContext.Guides.Update(_mapper.Map(guideDataModel, element));
            _dbContext.SaveChanges();
        }
        catch (ElementNotFoundException)
        {
            _dbContext.ChangeTracker.Clear();
            throw;
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Guide? GetGuideById(string id, string creatorId) => _dbContext.Guides.Where(x=> x.UserId == creatorId).FirstOrDefault(x => x.Id == id);

    private Guide? GetGuideByFIO(string fio, string creatorId) => _dbContext.Guides.Where(x => x.UserId == creatorId).FirstOrDefault(x => x.Fio == fio);
}