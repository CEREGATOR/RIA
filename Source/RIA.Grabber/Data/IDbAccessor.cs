namespace RIA.Grabber.Data
{
    using System.Collections.Generic;

    using RIA.Grabber.Model;

    /// <summary>
    /// Интерфейс, предоставляющий доступ к базе данных.
    /// </summary>
    public interface IDbAccessor
    {
        /// <summary>
        /// Получает список названий сохраненных статей.
        /// </summary>
        /// <returns>Список названий сохраненных статей.</returns>
        IList<string> GetPageNames();

        /// <summary>
        /// Получает статью по ее названию.
        /// </summary>
        /// <param name="title">Название статьи.</param>
        /// <returns>Статью.</returns>
        PageModel GetPageModel(string title);

        /// <summary>
        /// Получает идентификатор статьи по ее названию.
        /// </summary>
        /// <param name="title">Название статьи.</param>
        /// <returns>Идентификатор статьи.</returns>
        string GetPageIdByTitle(string title);

        /// <summary>
        /// Сохраняет статью.
        /// </summary>
        /// <param name="model"></param>
        void SavePageModel(PageModel model);
    }
}