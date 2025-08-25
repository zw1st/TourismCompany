using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.ReportModels;
using System.Collections.Generic;
using System.IO;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts;

public interface IReportContract
{
    // Отчет со списком экскурсий по выбранным поездкам
    Task<List<TripExcursionDto>> GetExcursionsByTrips(List<string> tripIds, CancellationToken ct);
    Task<Stream> CreateWordDocumentExcursionsByTrips(List<string> tripIds, CancellationToken ct);
    Task<Stream> CreateExcelDocumentExcursionsByTrips(List<string> tripIds, CancellationToken ct);


    // Отчет со сведениями за период по поездкам
    Task<List<TripDetailsDto>> GetTripsDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
    Task<Stream> CreateWordDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
    Task<Stream> CreateExcelDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);


    Task<List<TourPlacesDto>> GetPlacesByTours(List<string> tourIds, CancellationToken ct);
    Task<Stream> CreateExcelDocumentPlacesByTours(List<string> tourIds, CancellationToken ct);
    Task<Stream> CreateWordDocumentPlacesByTours(List<string> tourIds, CancellationToken ct);


    Task<Stream> CreateWordDocumentToursDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
    Task<List<TourDetailsDto>> GetToursDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
    Task<Stream> CreateExcelDocumentToursDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct);
}