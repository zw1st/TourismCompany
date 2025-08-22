using IvanSusaninProject_Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.ViewModels;

public class GroupViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int HumanAmount { get; set; }
    public HumanType HumanType { get; set; }
}
