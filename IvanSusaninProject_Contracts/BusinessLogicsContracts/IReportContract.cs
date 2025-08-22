using IvanSusaninProject_Contracts.DataModels;
using System.Collections.Generic;
using System.IO;

namespace IvanSusaninProject_Contracts.BusinessLogicsContracts
{
    public interface IReportContract
    {
        // Отчет со списком экскурсий по выбранным поездкам
        List<ExcursionDataModel> GetExcursionsByTrips(List<string> tripIds, string executorId);

        public Stream CreateWordDocumentExcursionsByTrips(List<string> tripIds, string executorId);

        public Stream CreateExcelDocumentExcursionsByTrips(List<string> tripIds, string executorId);


        // Отчет со сведениями за период по поездкам
        List<object> GetTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId);

        public Stream CreateWordDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId);

        public Stream CreateExcelDocumentTripsDetailsByPeriod(DateTime startDate, DateTime endDate, string guarantorId);

    }
}