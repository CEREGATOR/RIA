namespace RIA.Grabber.Model
{
    /// <summary>
    /// Описывает модель ссылки с текстом.
    /// </summary>
    public class LinkWithDescription
    {
        /// <summary>
        /// Получает или задает ссылку (адрес в сети).
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Получает или задает описание или текст ссылки на странице.
        /// </summary>
        public string Description { get; set; }
    }
}