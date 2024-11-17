
namespace MongoBackupHelperApp.Services
{
    internal interface IMongoUploaderService
    {
        Task UploadBackupAsync();
    }
}