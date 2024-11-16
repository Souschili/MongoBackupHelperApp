using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBackupHelperApp.Services
{
    internal class MongoUploaderService
    {
        private readonly FileManagerService _fileManagerService;

        public MongoUploaderService(FileManagerService fileManagerService)
        {
            _fileManagerService = fileManagerService;
        }

        public void StartUpload()
        {
            //1 получили имена файлов
            var files=_fileManagerService.GetFiles();

         

            
            Console.WriteLine();



           
        }

    }
}
