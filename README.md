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
    "DataBaseName": "your_database_name"
  }
}
```
