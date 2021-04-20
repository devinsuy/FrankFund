using System;
using System.IO;
using Google.Cloud.Storage.V1;
namespace DataAccessLayer
{
    public class StorageBucketHelper
    {
        private readonly StorageClient storage;
        private readonly string bucketName;
        private readonly string baseURI;

        public StorageBucketHelper(string bucketName)
        {
            this.storage = StorageClient.Create();
            this.bucketName = bucketName;
            this.baseURI = "gs://receipt_imgs/";
        }

        // Delete a temporarily created file at the given path
        public void deleteTempFile(string localPath)
        {

        }

        public string uploadFile(string userName, string localPathToFile, string fileName) {
            // Store the file in cloud storage under the user's folder
            string objectName = $"{userName}/{fileName}";

            // Read the file and upload it to cloud storage
            using var fileStream = File.OpenRead(localPathToFile);
            this.storage.UploadObject(bucketName, objectName, null, fileStream);
            string url = this.baseURI + objectName;

            return this.baseURI + objectName;
        }

        /* Download a file from cloud storage and save it to a given local path: DataAccessLayer/tmp/download/{filename} by default
           Params:
                userName: The name of the user's folder the file is contained in
                fileName: The name of the file in the given folder
           Returns:
                The local location the file was saved
         */
        public string downloadFile(string userName, string fileName, string dstPath)
        {
            // Build output file path or use default
            if (dstPath == null)
                dstPath = $"../DataAccessLayer/tmp/download/{fileName}";
            else
                if (dstPath.EndsWith('/'))
                    dstPath += fileName;
                else
                    dstPath += $"/{fileName}";

            string objectName = $"{userName}/{fileName}";
            using var outputFile = File.OpenWrite(dstPath);
            this.storage.DownloadObject(bucketName, objectName, outputFile);

            return dstPath;
        }
        
    }
}
