namespace RIA.Grabber.Services.ResultWatcher
{
    using System.Collections.Generic;

    using RIA.Grabber.Data;

    /// <summary>
    /// Предоставляет список названий страниц из БД.
    /// </summary>
    public class ResultDbWatcher : IResultWatcher
    {
        /// <summary>
        /// Интерфейс, предоставляющий доступ к базе данных.
        /// </summary>
        private readonly IDbAccessor _accessor;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ResultDbWatcher"/>.
        /// </summary>
        /// <param name="accessor">Интерфейс, предоставляющий доступ к базе данных.</param>
        public ResultDbWatcher(IDbAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <inheritdoc />
        public IList<string> GetPagesList() => _accessor.GetPageNames();
    }
}