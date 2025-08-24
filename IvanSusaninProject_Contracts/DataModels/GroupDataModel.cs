using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class GroupDataModel(string id, string name, int humanAmount, HumanType humanType, string userId)
{
    public string Id { get;  set; } = id;
    public string Name { get;  set; } = name;
    public int HumanAmount { get;  set; } = humanAmount;
    public HumanType HumanType { get;  set; } = humanType;
    public string UserId { get;  set; } = userId;
}
