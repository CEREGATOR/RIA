namespace RIA.Grabber.Services.PageSaver
{
    using RIA.Grabber.Data;
    using RIA.Grabber.Model;

    /// <summary>
    /// Сохраняет страницу в ДБ.
    /// </summary>
    public class DbPageSaver : IPageSaver
    {
        /// <summary>
        /// Интерфейс, предоставляющий доступ к базе данных.
        /// </summary>
        private readonly IDbAccessor _accessor;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DbPageSaver"/>.
        /// </summary>
        /// <param name="accessor">Интерфейс, предоставляющий доступ к базе данных.</param>
        public DbPageSaver(IDbAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <inheritdoc />
        public void SavePageModel(PageModel pageModel) => _accessor.SavePageModel(pageModel);
    }
}

