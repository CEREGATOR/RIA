namespace RIA.Grabber.Data
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Предоставляет конфигурацию подключения к БД.
    /// </summary>
    public class DbConfiguration
    {
        private string _dataSource;

        /// <summary>
        /// Получает или задает параметр DataSource строки подключения к БД.
        /// </summary>
        public string DataSource
        {
            get => _dataSource;
            
            set
            {
                if (Path.IsPathRooted(value))
                {
                    _dataSource = value;
                    return;
                }

                _dataSource = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), value);
            }
        }

        public string GetConnectionString() => $"Data Source={DataSource ?? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), "ria_db.db")}";
    }
}