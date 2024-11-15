using System.Text.Json;

namespace MongoBackupHelperApp.Config
{
    internal class AppConfig
    {
        // по умолчанию подключаемся к локальной базе
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string BackupFolder { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new JsonException("Unable to read ConnectionString");

            if (string.IsNullOrEmpty(BackupFolder))
                throw new JsonException("Unable to read BackupFolder");
        }
    }
}
