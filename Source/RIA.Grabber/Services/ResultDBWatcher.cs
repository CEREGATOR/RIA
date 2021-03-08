using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace RIA.Grabber.Services
{
    public class ResultDbWatcher
    {
        public List<string> ListDbUpdate()
        {
            List<string> pageTitleList = new List<string>();

            using (var connection = new SQLiteConnection(
                $"Data Source ={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "ria_db.db")}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT [Title] FROM [Page]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var titleOrderInRow = reader.GetOrdinal("Title");
                            var title = reader.GetString(titleOrderInRow);
                            pageTitleList.Add(title);
                        }
                    }
                }

                connection.Close();
            }

            return pageTitleList;
        }
    }
}