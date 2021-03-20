namespace RIA.Grabber.Services
{
    using Model;

    /// <summary>
    /// Интерфейс, предоставляющий методы сохранения модели страницы риа в БД.
    /// </summary>
    public interface IPageSaverDb
    {
        /// <summary>
        /// Сохранение модели страницы в ДБ.
        /// </summary>
        /// <param name="pageModel"></param>
        void SavePageModelDb(PageModel pageModel);
    }
}
