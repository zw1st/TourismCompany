using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class ExcursionDataModel
{
    private readonly GuideDataModel? _guide;
    private readonly UserDataModel? _user;

    public ExcursionDataModel(string id, string name, DateTime excursionDate, string userId, string guideId)
    {
        Id = id;
        Name = name;
        ExcursionDate = excursionDate;
        UserId = userId;
        GuideId = guideId;
    }

    public ExcursionDataModel(GuideDataModel? guide, UserDataModel? user, string id, string name, DateTime excursionDate, string userId, string guideId)
        : this(id, name, excursionDate, userId, guideId)
    {
        _user = user;
        _guide = guide;
    }

    public string Id { get;  set; }
    public string Name { get;  set; }
    public DateTime ExcursionDate { get; set; }
    public string UserId { get;  set; }
    public string GuideId { get; set; }

    public string GuideName => _guide?.Fio ?? string.Empty;
    public string UserLogin => _user?.Login ?? string.Empty;
}
