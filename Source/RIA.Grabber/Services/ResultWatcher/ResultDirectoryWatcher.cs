namespace RIA.Grabber.Services.ResultWatcher
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Предоставляет список названий json файлов.
    /// </summary>
    public class ResultDirectoryWatcher : IResultWatcher
    {
        /// <summary>
        /// Путь к папке с json файлами.
        /// </summary>
        private string _path;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ResultDirectoryWatcher"/>.
        /// </summary>
        /// <param name="path">Интерфейс, предоставляющий доступ к папке с json файлами.</param>
        public void Initialize(string path)
        {
            _path = path;
        }

        /// <inheritdoc />
        public IList<string> GetPagesList()
        {
            var dir = new DirectoryInfo(_path);
            var files = dir.GetFiles("*.json").Select(x => x.FullName).ToList();
            return files;
        }
    }
}
