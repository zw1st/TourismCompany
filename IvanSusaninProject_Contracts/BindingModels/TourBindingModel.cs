using IvanSusaninProject_Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TourBindingModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ExecutorId { get; set; }
    public List<TourExcursionDataModel>? Excursions { get;  set; }
    public List<TourGroupDataModel>? Groups { get;  set; }
}