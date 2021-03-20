namespace RIA.Grabber.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Globalization;

    using RIA.Grabber.Model;

    /// <summary>
    /// Предоставляет доступ к БД.
    /// </summary>
    public class DbAccessor : IDbAccessor
    {
        /// <summary>
        /// Конфигурация подключения к БД.
        /// </summary>
        private DbConfiguration _config;

        /// <summary>
        /// Инициализирует параметры работы.
        /// </summary>
        /// <param name="configuration">Конфигурация подключения к БД.</param>
        public void Initialize(DbConfiguration configuration)
        {
            _config = configuration;
        }

        /// <inheritdoc />
        public IList<string> GetPageNames()
        {
            var result = new List<string>();

            using (var connection = CreateConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT [Title] FROM [Page]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var titleOrderInRow = reader.GetOrdinal("Title");
                            var title = reader.GetString(titleOrderInRow);
                            result.Add(title);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        /// <inheritdoc />
        public PageModel GetPageModel(string title)
        {
            var pageModel = new PageModel();
            using (var connection = CreateConnection())
            {
                connection.Open();

                string pageId;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [Page] WHERE [Title] = @title LIMIT 1";
                    command.Parameters.Add(new SQLiteParameter("@title", DbType.String) { Value = title });

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception($"Статья с названием \"{title}\" не найдена.");

                        pageId = reader["Id"].ToString();
                        pageModel.Title = reader["Title"].ToString();
                        pageModel.Text = reader["Text"].ToString();

                        DateTime? publicationDate = DateTime.ParseExact(
                            reader["PublicationDate"].ToString() ?? throw new InvalidOperationException(),
                            "HH:mm dd.MM.yyyy",
                            CultureInfo.InvariantCulture);

                        pageModel.PublicationDate = publicationDate;

                        DateTime? lastChangeDate;

                        if (string.IsNullOrEmpty(reader["ChangeDate"].ToString()))
                        {
                            lastChangeDate = null;
                        }
                        else
                        {
                            lastChangeDate = DateTime.ParseExact(
                                reader["ChangeDate"].ToString() ?? throw new InvalidOperationException(),
                                "HH:mm dd.MM.yyyy",
                                CultureInfo.InvariantCulture);
                        }

                        pageModel.LastChangeDate = lastChangeDate;
                    }
                }
                
                pageModel.LinksInText.AddRange(GetPageLinks(connection, pageId));
                pageModel.ImagesInBase64.AddRange(GetPageImages(connection, pageId));

                connection.Close();
            }

            return pageModel;
        }

        /// <inheritdoc />
        public string GetPageIdByTitle(string title)
        {
            string pageId;

            using (var connection = CreateConnection())
            {
                connection.Open();
                pageId = GetPageIdByTitle(connection, title);
                connection.Close();
            }

            return pageId;
        }

        /// <inheritdoc />
        public void SavePageModel(PageModel model)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var pageId = GetPageIdByTitle(connection, model.Title);

                try
                {
                    InsertPage(connection, model, transaction);
                    InsertPageLinks(connection, model, pageId, transaction);
                    InsertPageImages(connection, model, pageId, transaction);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

                connection.Close();
            }
        }

        /// <summary>
        /// Вызывает команду добавления страницы.
        /// </summary>
        /// <param name="connection">Подключение к БД.</param>
        /// <param name="model">Модель страницы.</param>
        /// <param name="transaction">Транзакция.</param>
        private void InsertPage(SQLiteConnection connection, PageModel model, SQLiteTransaction transaction = null)
        {
            using (var command = connection.CreateCommand())
            {
                if (transaction != null)
                    command.Transaction = transaction;

                command.CommandText =
                    @"INSERT INTO [Page] ([Title], [Text], [PublicationDate], [ChangeDate]) 
                          VALUES (@title, @text, @pubDate, @changeDate)";

                command.Parameters.Add(new SQLiteParameter("@title", DbType.String) { Value = model.Title });
                command.Parameters.Add(new SQLiteParameter("@text", DbType.String) { Value = model.Text });
                command.Parameters.Add(new SQLiteParameter("@pubDate", DbType.String) { Value = model.PublicationDate?.ToString("HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture) });
                command.Parameters.Add(new SQLiteParameter("@changeDate", DbType.String) { Value = model.LastChangeDate?.ToString("HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture) });
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Вызывает команду добавления ссылок со страницы.
        /// </summary>
        /// <param name="connection">Подключение к БД.</param>
        /// <param name="model">Модель страницы.</param>
        /// <param name="pageId">Идентификатор страницы.</param>
        /// <param name="transaction">Транзакция.</param>
        private void InsertPageLinks(SQLiteConnection connection, PageModel model, string pageId, SQLiteTransaction transaction = null)
        {
            using (var command = connection.CreateCommand())
            {
                if (transaction != null)
                    command.Transaction = transaction;

                command.CommandText = "INSERT INTO [Links] ([PageId], [Link], [TextLink]) VALUES (@pageId, @link, @text)";

                command.Parameters.Add(new SQLiteParameter("@pageId", DbType.String) { Value = pageId });

                var linkParameter = new SQLiteParameter("@link", DbType.String);
                command.Parameters.Add(linkParameter);

                var textParameter = new SQLiteParameter("@text", DbType.String);
                command.Parameters.Add(textParameter);

                foreach (var textLink in model.LinksInText)
                {
                    linkParameter.Value = textLink.Url;
                    textParameter.Value = textLink.Description;

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Вызывает команду добавления изображений со страницы.
        /// </summary>
        /// <param name="connection">Подключение к БД.</param>
        /// <param name="model">Модель страницы.</param>
        /// <param name="pageId">Идентификатор страницы.</param>
        /// <param name="transaction">Транзакция.</param>
        private void InsertPageImages(SQLiteConnection connection, PageModel model, string pageId, SQLiteTransaction transaction)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO [Images] ([PageId], [Base64]) VALUES (@pageId, @img)";

                command.Parameters.Add(new SQLiteParameter("@pageId", DbType.String) { Value = pageId });

                var imgParameter = new SQLiteParameter("@img", DbType.String);
                command.Parameters.Add(imgParameter);

                foreach (var img in model.ImagesInBase64)
                {
                    imgParameter.Value = img;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Вызывает команду получения идентификатора страницы по ее названию.
        /// </summary>
        /// <param name="connection">Подключение к БД.</param>
        /// <param name="title">Название страницы.</param>
        /// <returns>Идентификатор страницы.</returns>
        private string GetPageIdByTitle(SQLiteConnection connection, string title)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id FROM [Page] WHERE [Title] = @title ORDER BY Id DESC LIMIT 1";
                command.Parameters.Add(new SQLiteParameter("@title", DbType.String) { Value = title });

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader["Id"].ToString();
                    }
                }
            }

            throw new Exception($"Статья с названием \"{title}\" не найдена.");
        }

        /// <summary>
        /// Вызывает команду на получение изображений по идентификатору страницы.
        /// </summary>
        /// <param name="connection">Подключение к БД.</param>
        /// <param name="pageId">Идентификатор страницы.</param>
        /// <returns>Список изображений, закодированных в bse64.</returns>
        private static List<string> GetPageImages(SQLiteConnection connection, string pageId)
        {
            var result = new List<string>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM [Images] WHERE [PageId] = @pageId";
                command.Parameters.Add(new SQLiteParameter("@pageId", DbType.String) { Value = pageId });

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader["Base64"].ToString());
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Вызывает команду получения ссылок со страницы по ее идентифкатору. 
        /// </summary>
        /// <param name="connection">Подключение к БД.</param>
        /// <param name="pageId">Идентификатор страницы.</param>
        /// <returns>Список ссылок.</returns>
        private IList<LinkWithDescription> GetPageLinks(SQLiteConnection connection, string pageId)
        {
            var result = new List<LinkWithDescription>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM [Links] WHERE [PageId] = @pageId";
                command.Parameters.Add(new SQLiteParameter("@pageId", DbType.String) { Value = pageId });

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var link = new LinkWithDescription { Url = reader["Link"].ToString(), Description = reader["TextLink"].ToString() };
                        result.Add(link);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Создает подключение к БД.
        /// </summary>
        /// <returns>Подключение к БД.</returns>
        private SQLiteConnection CreateConnection() => new SQLiteConnection(_config.GetConnectionString());
    }
}