using System;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Reflection;
using RIA.Grabber.Model;

namespace RIA.Grabber.Services
{
    public class PageModelFromDbConverter
    {
        private string _pageId;
        public PageModel PageModelDb(string pageName)
        {
            var pageModel = new PageModel();
            GetPageId(pageName);
            using (var connection = new SQLiteConnection(
                $"Data Source ={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "ria_db.db")}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"SELECT * FROM [Page] WHERE [Title]=('{pageName}')";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pageModel.Title = reader["Title"].ToString();

                            pageModel.Text = reader["Text"].ToString();

                            DateTime? publicationDate = DateTime.ParseExact(reader["PublicationDate"].ToString() ?? throw new InvalidOperationException(), "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
                            pageModel.PublicationDate = publicationDate;

                            DateTime? lastChangeDate;

                            if (string.IsNullOrEmpty(reader["ChangeDate"].ToString()))
                            {
                                lastChangeDate = null;
                            }
                            else
                            {
                                lastChangeDate = DateTime.ParseExact(reader["ChangeDate"].ToString() ?? throw new InvalidOperationException(), "HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture);
                            }

                            pageModel.LastChangeDate = lastChangeDate;
                        }
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"SELECT * FROM [Links] WHERE [PageId] = '{_pageId}'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pageModel.LinksInText.Add(
                                new LinkWithDescription { Url = reader["Link"].ToString(), Description = reader["TextLink"].ToString()});
                        }
                    }
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"SELECT * FROM [Images] WHERE [PageId] = '{_pageId}'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pageModel.ImagesInBase64.Add(reader["Base64"].ToString());
                        }
                    }
                }

                connection.Close();
            }

            return pageModel;
        }

        private void GetPageId(string pageName)
        {
            using (var connection = new SQLiteConnection(
                $"Data Source ={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "ria_db.db")}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"SELECT Id FROM [Page] WHERE [Title]=('{pageName}') ORDER BY Id DESC LIMIT 1";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _pageId = reader["Id"].ToString();
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}