using MongoDB.Bson;

namespace MongoBackupHelperApp.Services
{
    internal interface IFileManagerService
    {
        Task<Dictionary<string, IEnumerable<BsonDocument>>> GetUploadInfoAndData();
    }
}