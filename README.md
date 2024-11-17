# Mongo Backup Helper App

This is a console application designed to upload backup data to MongoDB efficiently. It retrieves files from a file manager service, processes them, and uploads them to specific MongoDB collections in parallel or asynchronously.

## Features

- **Parallel Uploads**: Handles bulk file uploads to MongoDB collections with limited parallelism to avoid overloading the database.
- **Asynchronous Execution**: Uses `Task.WhenAll()` for asynchronous execution of file uploads, ensuring that all tasks are completed before moving forward.
- **Bulk Insert with MongoDB**: Uses MongoDB's bulk insert functionality for efficient data insertion.
- **Error Handling**: Basic error handling is included to catch and log exceptions during the upload process.
- **Chunking Data**: Optionally, data can be chunked into smaller parts for easier processing and faster uploads.

## Requirements

- .NET 6.0 or later
- MongoDB instance
- MongoDB .NET Driver
- Configuration file (`options.json`) with MongoDB connection details

## Setup

### 1. Clone the repository
```bash
git clone <repository_url>
cd MongoBackupHelperApp
```
### 2. Configuration
In the root of the project, create an options.json file with your MongoDB connection details:
```bash
{
  "Options": {
    "ConnectionString": "mongodb://your_connection_string",
    "BackupFolder": "you_backup_folder",
    "DataBaseName": "your_database_name"
  }
}
```
### 3. Build the project
To build the project, run:
```bash
dotnet build
```
### 4. Run the application
After building the application, run it using:
```bash
dotnet run
```
## Usage

### Uploading Backup

The `UploadBackupAsync()` method in the `MongoUploaderService` handles uploading files to MongoDB. Files are processed and uploaded in parallel, ensuring optimal performance without chunking.

### Code Example
```csharp
public async Task UploadBackupAsync()
{
    // 1. Retrieve file data
    var filesData = await _fileManagerService.GetUploadInfoAndData();
    
    if (filesData == null)
        throw new ArgumentNullException("No data to upload", nameof(filesData));

    // Asynchronous execution with task completion
    var uploadTask = filesData.Select(x => UploadToCollection(x));
    await Task.WhenAll(uploadTask);
    
    // Parallel execution without result (commented out for your choice)
    // await Parallel.ForEachAsync(filesData, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (file, _) =>
    // {
    //     await UploadToCollection(file);
    // });
}
```
