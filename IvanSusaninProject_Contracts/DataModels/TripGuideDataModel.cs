
namespace IvanSusaninProject_Contracts.DataModels
{
    public class TripGuideDataModel(string tripId, string guideId)
    {
        public string TripId { get; private set; } = tripId;

        public string GuideId { get; private set; } = guideId;
    }
}