namespace RIA.Grabber.Services.ModelConverter
{
    using RIA.Grabber.Data;
    using RIA.Grabber.Model;

    /// <summary>
    /// Читает страницы из базы данных.
    /// </summary>
    public class PageModelFromDbReader : IPageModelReader
    {
        /// <summary>
        /// Интерфейс, предоставляющий доступ к базе данных.
        /// </summary>
        private readonly IDbAccessor _accessor;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PageModelFromDbReader"/>.
        /// </summary>
        /// <param name="accessor">Интерфейс, предоставляющий доступ к базе данных.</param>
        public PageModelFromDbReader(IDbAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <inheritdoc />
        public PageModel GetPage(string pageName) => _accessor.GetPageModel(pageName);
    }
}