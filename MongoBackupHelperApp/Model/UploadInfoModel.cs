using MongoDB.Bson;

namespace MongoBackupHelperApp.Model
{
    internal class UploadInfoModel
    {
        public string CollectionName { get; set; } = string.Empty;
        public IEnumerable<BsonDocument>? Documents { get; set; }
    }
}
