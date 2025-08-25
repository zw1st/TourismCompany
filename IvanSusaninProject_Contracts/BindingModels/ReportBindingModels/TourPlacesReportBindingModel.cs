using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels;

public class TourPlacesReportBindingModel
{
    [Required(ErrorMessage = "Выберите хотя бы один тур")]
    public List<string> SelectedTourIds { get; set; } = new List<string>();

    [Required]
    public string FileFormat { get; set; } = ".xlsx";
}
