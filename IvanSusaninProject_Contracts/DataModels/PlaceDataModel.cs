
using Microsoft.Extensions.Hosting;

namespace IvanSusaninProject_Contracts.DataModels;

public class PlaceDataModel
{
    private readonly GroupDataModel? _group;

    public PlaceDataModel(string id, string address, string city, string name, string groupId, string UserId)
    {
        Id = id;
        Address = address;
        City = city;
        Name = name;
        GroupId = groupId;
        this.UserId = UserId;
    }

    public PlaceDataModel(string id, string address, string city, string name, string groupId, string UserId, GroupDataModel group)
        : this(id, address, city, name, groupId, UserId)
    {
        _group = group;
    }

    public string Id { get; private set; }

    public string Address { get; private set; }

    public string City { get; private set; }

    public string Name { get; private set; }

    public string GroupId { get; set; }

    public string UserId { get; private set; }

    public string GroupName => _group?.HumanAmount + _group?.HumanType.ToString() ?? string.Empty;
}