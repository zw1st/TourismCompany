using IvanSusaninProject_BusinessLogic.OfficePackage;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.StorageContracts;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class ReportContract(
    IExcursionStorageContract excursionStorage,
    ITripStorageContract tripStorage,
    IPlaceStorageContract placeStorageContract,
    ITourStorageContract tourStorageContract,
    BaseWordBuilder baseWordBuilder,
    BaseExcelBuilder baseExcelBuilder) : IReportContract
{
    private readonly IExcursionStorageContract _excursionStorage = excursionStorage;
    private readonly ITripStorageContract _tripStorage = tripStorage;

    private readonly IPlaceStorageContract _placeStorageContract = placeStorageContract;
    private readonly ITourStorageContract _tourStorageContract = tourStorageContract;

    private readonly BaseWordBuilder _baseWordBuilder = baseWordBuilder;
    private readonly BaseExcelBuilder _baseExcelBuilder = baseExcelBuilder;

    private static readonly string[] item = ["Поездка", "Экскурсии"];
    private static readonly string[] itemArray = ["Поездка", "Экскурсионная группа", "Гиды"];

    //Заголовки Executor`а
    private static readonly string[] heading = ["Туры", "Места для посещения"];
    private static readonly string[] headingArray = ["Поездка", "Экскурсионная группа", "Гиды"];

    public List<ExcursionDataModel> GetExcursionsByTrips(List<string> tripIds, string executorId)
    {
        return _excursionStorage.GetExcursionsByTourIds(executorId, tripIds);
    }

    public List<object> GetTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");

        return _excursionStorage.GetTripsWithDetailsByPeriod(startDate, endDate, guarantorId);
    }

    public Stream CreateWordDocumentExcursionsByTrips(List<string> tripIds, string executorId)
    {
        var data = GetExcursionsByTrips(tripIds, executorId) ??
                  throw new InvalidOperationException("No data found");

        var groupedExcursions = data
            .GroupBy(a => a.Name)
            .Select(g => new { TripName = g.Key, Excursions = g.Select(e => e.Name) });

        var tableData = new List<string[]>
        {
            item
        };

        foreach (var group in groupedExcursions)
        {
            tableData.Add([
            group.TripName,
            string.Join(", ", group.Excursions)
        ]);
        }

        return _baseWordBuilder
            .AddHeader("Список экскурсий по поездкам:")
            .AddTable([3000, 5000], tableData)
            .Build();
    }

    public Stream CreateWordDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        var data = GetTripsDetailsByPeriod(startDate, endDate, guarantorId)
            .Cast<dynamic>()
            .ToList();

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
        {
            itemArray
        };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Places)
                    .Select(p => p.Group?.Name as string)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Guides)
                    .Select(g => g.Fio as string));

            tableData.Add(
            [
            tripDetail.Trip.Name as string ?? string.Empty,
            groups,
            guides
        ]);
        }

        return _baseWordBuilder
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}")
            .AddTable([3000, 3000, 3000], tableData)
            .Build();
    }

    public Stream CreateExcelDocumentExcursionsByTrips(List<string> tripIds, string executorId)
    {
        var data = GetExcursionsByTrips(tripIds, executorId) ??
                  throw new InvalidOperationException("No data found");

        var groupedExcursions = data
            .GroupBy(a => a.Name)
            .Select(g => new { TripName = g.Key, Excursions = g.Select(e => e.Name) });

        var tableRows = new List<string[]>
        {
            item
        };

        foreach (var group in groupedExcursions)
        {
            foreach (var excursion in group.Excursions)
            {
                tableRows.Add([group.TripName, excursion]);
            }
        }

        return _baseExcelBuilder
            .AddHeader("Список экскурсий по поездкам", 0, 2)
            .AddParagraph("", 0)
            .AddTable([20, 40], tableRows)
            .Build();
    }

    public Stream CreateExcelDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        var data = GetTripsDetailsByPeriod(startDate, endDate, guarantorId)
            .Cast<dynamic>()
            .ToList();

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
        {
            itemArray
        };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Places)
                    .Select(p => p.Group?.Name as string)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Guides)
                    .Select(g => g.Fio as string));

            tableRows.Add(
            [
            tripDetail.Trip.Name as string ?? string.Empty,
            groups,
            guides
            ]);
        }

        return _baseExcelBuilder
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}", 0, 3)
            .AddParagraph("", 0)
            .AddTable([20, 20, 20], tableRows)
            .Build();
    }


    //Методы Executor`а
    public List<PlaceDataModel> GetPlacesByTours(List<string> tourIds, string executorId)
    {
        return _placeStorageContract.GetPlacesByTourIds(executorId, tourIds);
    }

    public List<object> GetTourDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");
        return _placeStorageContract.GetToursWithDetailsByPeriod(startDate, endDate, guarantorId);
    }

    public Stream CreateWordDocumentPlacesByTours(List<string> tourIds, string executorId)
    {
        var data = GetPlacesByTours(tourIds, executorId) ??
                  throw new InvalidOperationException("No data found");

        var groupedPlaces = data
            .GroupBy(a => a.Name)
            .Select(g => new { TourName = g.Key, Places = g.Select(e => e.Name) });

        var tableData = new List<string[]>
        {
            heading
        };

        foreach (var group in groupedPlaces)
        {
            tableData.Add([
            group.TourName,
            string.Join(", ", group.Places)
        ]);
        }

        return _baseWordBuilder
            .AddHeader("Список мест посещения по тура:")
            .AddTable([3000, 5000], tableData)
            .Build();
    }

    public Stream CreateWordDocumentTourDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        var data = GetTourDetailsByPeriod(startDate, endDate, guarantorId)
            .Cast<dynamic>()
            .ToList();

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
        {
            headingArray
        };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Places)
                    .Select(p => p.Group?.Name as string)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Guides)
                    .Select(g => g.Fio as string));

            tableData.Add(
            [
            tripDetail.Trip.Name as string ?? string.Empty,
            groups,
            guides
        ]);
        }

        return _baseWordBuilder
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}")
            .AddTable([3000, 3000, 3000], tableData)
            .Build();
    }

    public Stream CreateExcelDocumentPlacesByTours(List<string> tourIds, string executorId)
    {
        {
            var data = GetPlacesByTours(tourIds, executorId) ??
                      throw new InvalidOperationException("No data found");

            var groupedPlaces = data
                .GroupBy(a => a.Name) // Группировка по названию тура
                .Select(g => new { TourName = g.Key, Places = g.Select(p => p.Name) });

            var tableRows = new List<string[]>
    {
        new[] { "Название тура", "Место для посещения" } // Заголовки таблицы
    };

            foreach (var group in groupedPlaces)
            {
                foreach (var place in group.Places)
                {
                    tableRows.Add([group.TourName, place]);
                }
            }

            return _baseExcelBuilder
                .AddHeader("Список мест для посещения по турам", 0, 2)
                .AddParagraph("", 0)
                .AddTable([20, 40], tableRows)
                .Build();
        }
    }

    public Stream CreateExcelDocumentTourDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId)
    {
        var data = GetTripsDetailsByPeriod(startDate, endDate, guarantorId)
            .Cast<dynamic>()
            .ToList();

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
        {
            itemArray
        };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Places)
                    .Select(p => p.Group?.Name as string)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                ((IEnumerable<dynamic>)tripDetail.Guides)
                    .Select(g => g.Fio as string));

            tableRows.Add(
            [
            tripDetail.Trip.Name as string ?? string.Empty,
            groups,
            guides
            ]);
        }

        return _baseExcelBuilder
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}", 0, 3)
            .AddParagraph("", 0)
            .AddTable([20, 20, 20], tableRows)
            .Build();
    }

}