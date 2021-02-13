using RIA.Grabber;
using RIA.Grabber.Services;

namespace RIA.GUI
{
    public class CompositionRoot
    {
        public MainForm MainForm { get; private set; }

        public void Initialize()
        {
            var dataDownloader = new DataDownloader();
            var htmlParser = new HtmlParser();
            var jsonPageSaver = new JsonPageSaver();
            var processor = new RiaPageProcessor(dataDownloader, htmlParser, jsonPageSaver);

            var converter = new PageModelFromJsonConverter();
            var dirWatcher = new ResultDirectoryWatcher();

            MainForm = new MainForm();

            MainForm.InjectDependencies(processor, converter, dirWatcher);
        }
    }
}
