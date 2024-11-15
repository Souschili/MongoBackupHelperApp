using System.Text.Json;

public class AppConfig
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string BackupFolder { get; set; } = string.Empty;
    public string DataBaseName { get; set; } = string.Empty;  // Поле для базы данных

    public void Validate()
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new JsonException("Unable to read ConnectionString");

        if (string.IsNullOrEmpty(BackupFolder))
            throw new JsonException("Unable to read BackupFolder");

        if (string.IsNullOrEmpty(DataBaseName))
            throw new JsonException("Unable to read DataBaseName");
    }
}
