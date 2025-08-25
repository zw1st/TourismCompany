using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TripExcursionsReportBindingModel
{
    [Required(ErrorMessage = "Выберите хотя бы одну поездку")]
    public List<string> SelectedTripIds { get; set; } = new List<string>();

    [Required]
    public string FileFormat { get; set; } = ".xlsx";
}
