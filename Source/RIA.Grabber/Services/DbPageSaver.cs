using System.Globalization;
using System.IO;
using System.Reflection;

namespace RIA.Grabber.Services
{
    using System.Data.SQLite;

    using Model;

    public class DbPageSaver : IPageSaverDb
    {
        private string _pageId;
        public void SavePageModelDb(PageModel pageModel)
        {
            using (var connection = new SQLiteConnection(@"Data Source=D:\DbFileCreator\ria_db.db"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"INSERT INTO [Page] ([Title], [Text], [PublicationDate], [ChangeDate]) VALUES ('{pageModel.Title}', '{pageModel.Text}','{pageModel.PublicationDate?.ToString("HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture)}', '{pageModel.LastChangeDate?.ToString("HH:mm dd.MM.yyyy", CultureInfo.InvariantCulture)}')";

                    command.ExecuteNonQuery();

                    GetPageId(pageModel);

                    SQLiteTransaction transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    foreach (var textLink in pageModel.LinksInText) 
                    {
                        command.CommandText =
                            $"INSERT INTO [Links] ([PageId], [Link], [TextLink]) VALUES ('{_pageId}' ,'{textLink.Url}', '{textLink.Description}')";
                        command.ExecuteNonQuery();
                    }

                    foreach (var imgBase64 in pageModel.ImagesInBase64)
                    {
                        command.CommandText =
                            $"INSERT INTO [Images] ([PageId], [Base64]) VALUES ('{_pageId}' ,'{imgBase64}')";
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }

                connection.Close();
            }
        }

        private void GetPageId(PageModel pageModel)
        {
            using (var connection = new SQLiteConnection(@"Data Source=D:\DbFileCreator\ria_db.db"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"SELECT Id FROM [Page] WHERE [Title]=('{pageModel.Title}') ORDER BY Id DESC LIMIT 1";

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

