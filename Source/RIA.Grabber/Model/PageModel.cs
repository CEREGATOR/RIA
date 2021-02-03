namespace RIA.Grabber.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Описывает модель страницы риа ру.
    /// </summary>
    public class PageModel
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PageModel"/>.
        /// </summary>
        public PageModel()
        {
            LinksInText = new List<LinkWithDescription>();
            ImageLinks = new List<string>();
            ImagesInBase64 = new List<string>();
        }

        /// <summary>
        /// Получает или задает заголовок новости.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Получает или задает текст новости.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Получает или задает дату публикации новости.
        /// </summary>
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Получает или задает дату последнего изменения новости.
        /// </summary>
        public DateTime? LastChangeDate { get; set; }

        /// <summary>
        /// Получает или задает список ссылок из текста.
        /// </summary>
        public List<LinkWithDescription> LinksInText { get; set; }

        /// <summary>
        /// Получает или задает список ссылок на изображения из новости.
        /// </summary>
        public List<string> ImageLinks { get; set; }

        /// <summary>
        /// Получает или задает список изображений из новости в Base64 представлении.
        /// </summary>
        public List<string> ImagesInBase64 { get; set; }
    }
}