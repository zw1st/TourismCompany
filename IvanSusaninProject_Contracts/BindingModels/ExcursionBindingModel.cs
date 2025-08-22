using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IvanSusaninProject_Contracts.BindingModels;

public class ExcursionBindingModel
{
    public string? Id { get;  set; } = Guid.NewGuid().ToString();
    public string? Name { get;  set; } 
    public DateTime ExcursionDate { get;  set; }
    public string? UserId { get;  set; }
    public string? GuideId { get;  set; }
    public string? GuideName { get; set; }
}
