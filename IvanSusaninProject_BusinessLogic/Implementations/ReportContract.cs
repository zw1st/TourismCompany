using IvanSusaninProject_BusinessLogic.OfficePackage;
using IvanSusaninProject_Contracts.BusinessLogicsContracts;
using IvanSusaninProject_Contracts.DataModels;
using IvanSusaninProject_Contracts.ReportModels;
using IvanSusaninProject_Contracts.StorageContracts;

namespace IvanSusaninProject_BusinessLogic.Implementations;

public class ReportContract(
    IExcursionStorageContract excursionStorage,
    ITripStorageContract tripStorage,
    ITourStorageContract tourStorage) : IReportContract
{
    private readonly IExcursionStorageContract _excursionStorage = excursionStorage;
    private readonly ITripStorageContract _tripStorage = tripStorage;
    private readonly ITourStorageContract _tourStorage = tourStorage;
    //private readonly BaseWordBuilder _baseWordBuilder = baseWordBuilder;
    //private readonly BaseExcelBuilder _baseExcelBuilder = baseExcelBuilder;
    private static readonly string[] item = ["Поездка", "Экскурсии"];
    private static readonly string[] itemArray = ["Поездка", "Экскурсионные группы", "Гиды"];
    private static readonly string[] item1 = ["Тур", "Места для посещения"];
    private static readonly string[] itemArray1 = ["Тур", "Гиды", "Экскурсионные группы"];

    public Task<List<TripExcursionDto>> GetExcursionsByTrips(List<string> tripIds, CancellationToken ct)
    {
        return _excursionStorage.GetExcursionsByTourIds(tripIds, ct);
    }

    public async Task<List<TripDetailsDto>> GetTripsDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");

        return await _excursionStorage.GetTripsWithDetailsByPeriod(startDate, endDate, ct);
    }

    public Task<List<TourPlacesDto>> GetPlacesByTours(List<string> tourIds, CancellationToken ct)
    {
        return _tourStorage.GePlacesByTourIds(tourIds, ct);
    }

    public async Task<List<TourDetailsDto>> GetToursDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be later than end date");

        return await _tourStorage.GetToursWithDetailsByPeriod(startDate, endDate, ct);
    }

    public async Task<Stream> CreateWordDocumentExcursionsByTrips(List<string> tripIds, CancellationToken ct)
    {
        var data = await _excursionStorage.GetExcursionsByTourIds(tripIds, ct) ??
                  throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
    {
        item
    };

        foreach (var trip in data)
        {
            tableData.Add(
            [
                trip.TripName, // "Начальный город - Конечный город"
            string.Join(", ", trip.Excursions) // Список экскурсий
            ]);
        }
        BaseWordBuilder bwd = new OpenXmlWordBuilder();
        return bwd
            .AddHeader("Список экскурсий по поездкам:")
            .AddTable([3000, 5000], tableData)
            .Build();
    }
    public async Task<Stream> CreateWordDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var data = await GetTripsDetailsByPeriod(startDate, endDate, ct);

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
        {
            itemArray
        };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                tripDetail.Places
                    .Select(p => p.Group?.HumanType.ToString() + " " + p.Group?.HumanAmount.ToString())
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                tripDetail.Guides
                    .Select(g => g.Fio));

            tableData.Add(
            [
                tripDetail.Trip.StartCity + " " + tripDetail.Trip.EndCity,
                groups,
                guides
            ]);
        }

        BaseWordBuilder bwd = new OpenXmlWordBuilder();
        return bwd
            .AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}")
            .AddTable([3000, 3000, 3000], tableData)
            .Build();
    }
    public async Task<Stream> CreateWordDocumentPlacesByTours(List<string> tourIds, CancellationToken ct)
    {
        var data = await GetPlacesByTours(tourIds, ct) ??
                  throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
    {
        item1
    };

        foreach (var tour in data)
        {
            tableData.Add(
            [
                tour.TourName,
            string.Join(", ", tour.Places)
            ]);
        }

        BaseWordBuilder bwd = new OpenXmlWordBuilder();
        return bwd
            .AddHeader("Список мест для посещения по турам:")
            .AddTable([3000, 5000], tableData)
            .Build();
    }
    public async Task<Stream> CreateWordDocumentToursDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var data = await GetToursDetailsByPeriod(startDate, endDate, ct);

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableData = new List<string[]>
        {
            itemArray1
        };

        foreach (var tourDetail in data)
        {
            var giudes = string.Join(", ",
                tourDetail.Excursions
                    .Select(p => p.Guide?.Fio)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var groups = string.Join(", ",
                tourDetail.Groups
                    .Select(g => g.HumanType.ToString() + " " + g.HumanAmount.ToString()));

            tableData.Add(
            [
                tourDetail.Tour.Name,
                giudes,
                groups
            ]);
        }

        BaseWordBuilder bwd = new OpenXmlWordBuilder();
        return bwd
            .AddHeader($"Обзор туров за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}")
            .AddTable([3000, 3000, 3000], tableData)
            .Build();
    }
    public async Task<Stream> CreateExcelDocumentExcursionsByTrips(List<string> tripIds, CancellationToken ct)
    {
        var data = await GetExcursionsByTrips(tripIds, ct) ??
                  throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
    {
        item // ["Поездка", "Экскурсии"]
    };

        // Группируем по поездкам
        var excursionsByTrip = data
            .GroupBy(x => x.TripName)
            .ToList();

        foreach (var tripGroup in excursionsByTrip)
        {
            // Для каждой поездки получаем все уникальные экскурсии
            var allExcursions = tripGroup
                .SelectMany(x => x.Excursions)
                .Distinct()
                .ToList();

            // Добавляем первую строку с названием поездки и первой экскурсией
            if (allExcursions.Any())
            {
                tableRows.Add([tripGroup.Key, allExcursions[0]]);
            }

            // Добавляем остальные экскурсии (если есть) с пустым первым столбцом
            for (int i = 1; i < allExcursions.Count; i++)
            {
                tableRows.Add(["", allExcursions[i]]);
            }

            // Добавляем пустую строку между поездками для лучшей читаемости
            if (tripGroup != excursionsByTrip.Last())
            {
                tableRows.Add(["", ""]);
            }
        }

        BaseExcelBuilder bwd = new OpenXmlExcelBuilder();
        return bwd
            .AddHeader("Список экскурсий по поездкам", 0, 2)
            .AddParagraph("", 0)
            .AddTable([20, 40], tableRows)
            .Build();
    }
    public async Task<Stream> CreateExcelDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var data = await GetTripsDetailsByPeriod(startDate, endDate, ct);

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
        {
            itemArray
        };

        foreach (var tripDetail in data)
        {
            var groups = string.Join(", ",
                tripDetail.Places
                    .Select(p => p.Group?.HumanType.ToString() + " " + p.Group?.HumanAmount.ToString())
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var guides = string.Join(", ",
                tripDetail.Guides
                    .Select(g => g.Fio));

            tableRows.Add(
            [
                tripDetail.Trip.StartCity + " " + tripDetail.Trip.EndCity,
                groups,
                guides
            ]);
        }

        BaseExcelBuilder bwd = new OpenXmlExcelBuilder();
        return bwd.AddHeader($"Обзор поездок за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}", 0, 3)
            .AddParagraph("", 0)
            .AddTable([20, 20, 20], tableRows)
            .Build();
    }
    public async Task<Stream> CreateExcelDocumentPlacesByTours(List<string> tourIds, CancellationToken ct)
    {
        var data = await GetPlacesByTours(tourIds, ct) ??
                  throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
    {
        item1
    };

        var placesByTour = data
            .GroupBy(x => x.TourName)
            .ToList();

        foreach (var tourGroup in placesByTour)
        {
            var allPlaces = tourGroup
                .SelectMany(x => x.Places)
                .Distinct()
                .ToList();

            if (allPlaces.Any())
            {
                tableRows.Add([tourGroup.Key, allPlaces[0]]);
            }

            for (int i = 1; i < allPlaces.Count; i++)
            {
                tableRows.Add(["", allPlaces[i]]);
            }

            if (tourGroup != placesByTour.Last())
            {
                tableRows.Add(["", ""]);
            }
        }

        BaseExcelBuilder bwd = new OpenXmlExcelBuilder();
        return bwd.AddHeader("Список мест для посещения по турам", 0, 2)
            .AddParagraph("", 0)
            .AddTable([20, 40], tableRows)
            .Build();
    }
    public async Task<Stream> CreateExcelDocumentToursDetailsByPeriod(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var data = await GetToursDetailsByPeriod(startDate, endDate, ct);

        if (data.Count == 0)
            throw new InvalidOperationException("No data found");

        var tableRows = new List<string[]>
        {
            itemArray1
        };

        foreach (var tourDetail in data)
        {
            var guides = string.Join(", ",
                tourDetail.Excursions
                    .Select(p => p.Guide?.Fio)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct());

            var groups = string.Join(", ",
                tourDetail.Groups
                    .Select(g => g.HumanType.ToString() + " " + g.HumanAmount.ToString()));

            tableRows.Add(
            [
                tourDetail.Tour.Name,
                guides,
                groups
            ]);
        }

        BaseExcelBuilder bwd = new OpenXmlExcelBuilder();
        return bwd.AddHeader($"Обзор туров за период с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}", 0, 3)
            .AddParagraph("", 0)
            .AddTable([20, 20, 20], tableRows)
            .Build();
    }
}