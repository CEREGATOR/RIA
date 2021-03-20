namespace RIA.Grabber.Services.ResultWatcher
{
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс, предоставляющий список названий страниц.
    /// </summary>
    public interface IResultWatcher
    {
        /// <summary>
        /// Получает список названий страниц.
        /// </summary>
        /// <returns>Список названий страниц.</returns>
        IList<string> GetPagesList();
    }
}