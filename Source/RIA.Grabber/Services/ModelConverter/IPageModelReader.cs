namespace RIA.Grabber.Services.ModelConverter
{
    using RIA.Grabber.Model;

    /// <summary>
    /// Интерфейс, позволяющий считывать хранимые страницы.
    /// </summary>
    public interface IPageModelReader
    {
        /// <summary>
        /// Получает страницу по её названию.
        /// </summary>
        /// <param name="pageTitle">Название статьи.</param>
        /// <returns>Модель страницы.</returns>
        PageModel GetPage(string pageTitle);
    }
}