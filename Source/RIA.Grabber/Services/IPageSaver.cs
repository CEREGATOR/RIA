namespace RIA.Grabber.Services
{
    using RIA.Grabber.Model;

    /// <summary>
    /// Интерфейс, предоставляющий методы сохранения модели страницы риа.
    /// </summary>
    public interface IPageSaver
    {
        /// <summary>
        /// Cохрание модели страницы.
        /// </summary>
        /// <param name="pageModel"></param>
        /// <param name="dirPath"></param>
        void SavePageModel(PageModel pageModel, string dirPath);
    }
}