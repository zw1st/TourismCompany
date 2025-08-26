using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.BindingModels.ReportBindingModels;

public class RequestReportBindingModel
{
    [Display(Name = "С какой даты")]
    [Required]
    public required DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);
    [Required]
    [Display(Name = "По какую дату")]
    public required DateTime EndDate { get; set; } = DateTime.Now;
    public string? FileName { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate > EndDate)
        {
            yield return new ValidationResult(
                "Дата начала не может быть позже даты окончания.",
                new[] { nameof(StartDate), nameof(EndDate) }
            );
        }
    }
}
