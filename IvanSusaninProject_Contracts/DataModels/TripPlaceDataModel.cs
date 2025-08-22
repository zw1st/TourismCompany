using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanSusaninProject_Contracts.DataModels;

public class TripPlaceDataModel(string placeId, string tripId)
{
    public string PlaceId { get; private set; } = placeId;

    public string TripId { get; private set; } = tripId;
}