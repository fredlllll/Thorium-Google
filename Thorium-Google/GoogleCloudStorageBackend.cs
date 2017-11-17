using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Google.Cloud.Storage.V1;
using Thorium_Storage_Service;
using Google.Apis.Auth.OAuth2;
using System.Reflection;
using NLog;

namespace Thorium_Google
{
    public class GoogleCloudStorageBackend : IStorageBackend
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        string projectId;

        StorageClient storageClient;

        public GoogleCloudStorageBackend()
        {
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string credentialsFile = Path.Combine(fi.Directory.FullName, "googlecredentials.json");

            logger.Info("credentials file: " + credentialsFile);

            GoogleCredential gc = GoogleCredential.FromJson(File.ReadAllText(credentialsFile));

            projectId = GoogleCloudStorageBackendConfig.ProjectId;

            logger.Info("project id: " + projectId);

            storageClient = StorageClient.Create(gc);
        }

        string GetBucketName(string dataPackage)
        {
            return dataPackage + "_thoriumdatapackage";
        }

        public void CreateDataPackage(string id)
        {
            storageClient.CreateBucket(projectId, GetBucketName(id));
        }

        public void CreateFile(string dataPackage, string key, string sourcefile)
        {
            using(FileStream fs = new FileStream(sourcefile, FileMode.Open, FileAccess.Read))
            {
                storageClient.UploadObject(GetBucketName(dataPackage), key, null, fs);
            }
        }

        public void DeleteDataPackage(string id)
        {
            storageClient.DeleteBucket(GetBucketName(id));
        }

        public void DeleteFile(string dataPackage, string key)
        {
            storageClient.DeleteObject(GetBucketName(dataPackage), key);
        }

        public IEnumerable<string> GetDataPackageKeys(string id)
        {
            var objs = storageClient.ListObjects(GetBucketName(id));
            return objs.Select((x) => { return x.Name; });
        }

        public void MakeFileAvailable(string dataPackage, string key, string destinationFile)
        {
            using(FileStream fs = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                storageClient.DownloadObject(GetBucketName(dataPackage), key, fs);
            }
        }
    }
}
