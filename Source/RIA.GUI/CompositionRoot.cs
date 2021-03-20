using RIA.Grabber;
using RIA.Grabber.Services;

namespace RIA.GUI
{
    using System.Configuration;

    using RIA.Grabber.Data;
    using RIA.Grabber.Services.ModelConverter;
    using RIA.Grabber.Services.PageSaver;
    using RIA.Grabber.Services.ResultWatcher;

    /// <summary>
    /// Корень компоновки приложения.
    /// </summary>
    public class CompositionRoot
    {
        /// <summary>
        /// Получает главную форму приложения.
        /// </summary>
        public MainForm MainForm { get; private set; }

        /// <summary>
        /// Инициализирует главную форму приложения и её зависимости.
        /// </summary>
        public void Initialize()
        {
            var dataDownloaded = new DataDownloader();
            var htmlParser = new HtmlParser();

            IPageSaver pageSaver;
            IPageModelReader reader;
            IResultWatcher watcher;
            if (bool.Parse(ConfigurationManager.AppSettings["UseSqlite"]))
            {
                var dbAccessor = new DbAccessor();
                dbAccessor.Initialize(new DbConfiguration { DataSource = ConfigurationManager.AppSettings["SqliteDbPath"] });
                pageSaver = new DbPageSaver(dbAccessor);
                reader = new PageModelFromDbReader(dbAccessor);
                watcher = new ResultDbWatcher(dbAccessor);
            }
            else
            {
                var path = ConfigurationManager.AppSettings["JsonDirPath"];

                var jsonPageSaver = new JsonPageSaver();
                jsonPageSaver.Initialize(path);
                pageSaver = jsonPageSaver;

                var jsonConverter = new PageModelFromJsonReader();
                jsonConverter.Initialize(path);
                reader = jsonConverter;

                var jsonWatcher = new ResultDirectoryWatcher();
                jsonWatcher.Initialize(path);
                watcher = jsonWatcher;
            }

            var processor = new RiaPageProcessor(dataDownloaded, htmlParser, pageSaver);

            MainForm = new MainForm();
            MainForm.InjectDependencies(processor, reader, watcher);
        }
    }
}
