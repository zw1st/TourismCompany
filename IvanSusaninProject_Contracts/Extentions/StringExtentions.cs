
namespace IvanSusaninProject_Contracts.Extentions;

public static class StringExtentions
{

    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static bool IsGuid(this string str)
    {
        return Guid.TryParse(str, out _);
    }
}