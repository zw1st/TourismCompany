namespace IvanSusaninProject_Contracts.ViewModels;

public class GuideViewModel
{
    public string Id {  get; set; }
    public required string Fio { get; set; }

    public int Experience { get; set; }

    public int Age { get; set; }
}