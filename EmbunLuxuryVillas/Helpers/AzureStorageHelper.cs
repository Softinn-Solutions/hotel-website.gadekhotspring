using System;
using System.Threading.Tasks;
using EmbunLuxuryVillas.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace EmbunLuxuryVillas.Helpers
{
    public class AzureStorageHelper
    {
        private readonly IOptions<AppConfigurations> _appConfigurations;

        public AzureStorageHelper(IOptions<AppConfigurations> config)
        {
            _appConfigurations = config;
        }

        public async Task<DatabaseIndex> GetDatabaseIndexByHotelId(int hotelId)
        {
            var storageAccount =
            CloudStorageAccount.Parse(_appConfigurations.Value.StorageConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient();
            var table =
                tableClient.GetTableReference(_appConfigurations.Value.AzureTableReference);

            TableOperation retrieveOperation = TableOperation.Retrieve<DatabaseIndex>(_appConfigurations.Value.LiteDbTablePartitionKey, hotelId.ToString());

            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                return (DatabaseIndex)retrievedResult.Result;
            }

            return null;
        }

        public async Task<bool> DownloadDatabaseFile(string fileName)
        {
            var dbPath = "newDatabase.db";
            var storageAccount =
                CloudStorageAccount.Parse(_appConfigurations.Value.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container =
                blobClient.GetContainerReference("cms-litedb-databases");

            var blockBlob = container.GetBlockBlobReference(fileName);
            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            using (var fileStream = System.IO.File.OpenWrite(dbPath))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
            }

            return ValidateAndReplaceDatabase();
        }

        public bool ValidateAndReplaceDatabase()
        {
            if (System.IO.File.Exists("database.db"))
            {
                var liteDbHelper = new LiteDbHelper();
                liteDbHelper.dbPath = "newDatabase.db";
                try
                {
                    liteDbHelper.GetFullHotelViewModel();
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete("newDatabase.db");
                    return false;
                }
                System.IO.File.Delete("database.db");
            }

            System.IO.File.Move("newDatabase.db", "database.db");

            return true;
        }
    }
}
