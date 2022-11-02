using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace FBOLinx.TableStorage
{
    public class BlobStorageService
    {
        protected readonly BlobServiceClient blobServiceClient;

        public BlobStorageService(IOptions<AzureTableStorageSettings> azureTableStorageSettings)
        {
            if (!string.IsNullOrWhiteSpace(azureTableStorageSettings?.Value?.ConnectionString))
            {
                blobServiceClient = new BlobServiceClient(azureTableStorageSettings.Value.ConnectionString);
            }
        }

        public async Task Upload<TContainer>(string directoryName, string blobName, string text)
        {
            try
            {
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(typeof(TContainer).Name.ToLowerInvariant());
                await blobContainerClient.CreateIfNotExistsAsync();
                BlobClient blobClient = blobContainerClient.GetBlobClient($"{directoryName}/{blobName}");
                await blobClient.UploadAsync(new BinaryData(text), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<string> Download<TContainer>(string directoryName, string blobName)
        {
            try
            {
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(typeof(TContainer).Name.ToLowerInvariant());
                BlobClient blobClient = blobContainerClient.GetBlobClient($"{directoryName}/{blobName}");

                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                string downloadedData = downloadResult.Content.ToString();

                return downloadedData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.Empty;
        }

        // public async Task<string> DownloadAllBlobs<TContainer>(string containerName)
        // {
        //     try
        //     {
        //         BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(typeof(TContainer).Name.ToLowerInvariant());
        //
        //         AsyncPageable<BlobItem> blobPages = blobContainerClient.GetBlobsAsync();
        //         IList<BlobItem> blobItems = new List<BlobItem>();
        //         await foreach (Page<BlobItem> page in blobPages.AsPages())
        //         {
        //             foreach (BlobItem blobItem in page.Values)
        //             {
        //                 blobItems.Add(blobItem);
        //             }
        //         }
        //
        //         foreach (BlobItem blobItem in blobItems)
        //         {
        //             blobItem.
        //         }
        //
        //         BlobClient blobClient = blobContainerClient.GetBlobClient($"{directoryName}/{blobName}");
        //         if (await blobClient.ExistsAsync())
        //         {
        //             BlobDownloadInfo download = await blobClient.DownloadAsync();
        //             byte[] result = new byte[download.ContentLength];
        //             await download.Content.ReadAsync(result, 0, (int)download.ContentLength);
        //
        //             return Encoding.UTF8.GetString(result);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex);
        //     }
        //
        //     return string.Empty;
        // }

        public async Task DeleteBlob<TContainer>(string directoryName, string blobName)
        {
            try
            {
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(typeof(TContainer).Name.ToLowerInvariant());
                BlobClient blobClient = blobContainerClient.GetBlobClient($"{directoryName}/{blobName}");
                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task DeleteContainer<TContainer>()
        {
            try
            {
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(typeof(TContainer).Name.ToLowerInvariant());
                await blobContainerClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
