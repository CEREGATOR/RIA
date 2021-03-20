namespace RIA.Grabber.Services.PageSaver
{
    using RIA.Grabber.Model;

    /// <summary>
    /// Интерфейс, предоставляющий методы сохранения модели страницы риа.
    /// </summary>
    public interface IPageSaver
    {
        /// <summary>
        /// Сохранение модели страницы.
        /// </summary>
        /// <param name="pageModel">Модель страницы.</param>
        void SavePageModel(PageModel pageModel);
    }
}