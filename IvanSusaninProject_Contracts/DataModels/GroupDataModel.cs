using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class GroupDataModel(string id, string name, int humanAmount, HumanType humanType, string userId)
{
    public string Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public int HumanAmount { get; private set; } = humanAmount;
    public HumanType HumanType { get; private set; } = humanType;
    public string UserId { get; private set; } = userId;
}
