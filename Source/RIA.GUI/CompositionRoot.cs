using RIA.Grabber;
using RIA.Grabber.Services;

namespace RIA.GUI
{
    public class CompositionRoot
    {
        public MainForm MainForm { get; private set; }

        public void Initialize()
        {
            var dataDownloaded = new DataDownloader();
            var htmlParser = new HtmlParser();
            var jsonPageSaver = new JsonPageSaver();
            var dbPageSaver = new DbPageSaver();
            var processor = new RiaPageProcessor(dataDownloaded, htmlParser, jsonPageSaver,dbPageSaver);

            var converter = new PageModelFromJsonConverter();
            var converterdb = new PageModelFromDbConverter();
            var dirWatcher = new ResultDirectoryWatcher();
            var dbWatcher = new ResultDbWatcher();

            MainForm = new MainForm();

            MainForm.InjectDependencies(processor, converter, dirWatcher, dbWatcher, converterdb);
        }
    }
}
