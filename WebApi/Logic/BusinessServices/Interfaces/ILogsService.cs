// Domain/Services/Interfaces/ILogsService.cs
using Domain.DTOs.Log.Responses;
using Domain.Enums;

namespace WebApi.Logic.BusinessServices.Interfaces
{
    public interface ILogsService
    {
        /// <summary>
        /// Возвращает постраничный список логов и общее число записей по заданным фильтрам.
        /// </summary>
        /// <param name="pageSize">Размер страницы (количество записей).</param>
        /// <param name="pageIndex">Индекс страницы (0-based).</param>
        /// <param name="from">Начальная граница фильтра по дате (включительно).</param>
        /// <param name="to">Конечная граница фильтра по дате (включительно).</param>
        /// <param name="level">Фильтр по уровню лога (если задан).</param>
        /// <returns>
        /// Пару: 
        ///  - IEnumerable<LogEntryResponse> — список записей,
        ///  - int — общее количество записей, подходящих под фильтр.
        /// </returns>
        Task<(IEnumerable<LogEntryResponse> Logs, int TotalCount)> GetLogsAsync(
            int pageSize,
            int pageIndex,
            DateTime? from,
            DateTime? to,
            LogLevelEnum? level);
    }
}
