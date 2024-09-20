using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SchoolSystem.Web.Services.Interfaces;

namespace SchoolSystem.Web.Services;

/// <summary>
///  Service for interacting with Azure Blob Storage
/// </summary>
public class BlobStorageService : IBlobStorageService
{
    private readonly CloudBlobClient _blobClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(configuration), "AzureBlobStorage:ConnectionString is missing in app settings.json");
        }

        // Initialize BlobServiceClient
        var storageAccount = CloudStorageAccount.Parse(connectionString);
        _blobClient = storageAccount.CreateCloudBlobClient();
    }
    public async Task<Guid> UploadFileAsync(Stream fileStream, string
        containerName)
        => await UploadToBlobAsync(fileStream, containerName);

    public async Task<Guid> UploadFileAsync(IFormFile file, string containerName)
    {
        var fileStream = file.OpenReadStream();
        return await UploadToBlobAsync(fileStream, containerName);
    }

    public async Task<Guid> UploadFileAsync(byte[] fileBytes, string containerName)
    {
        var fileStream = new MemoryStream(fileBytes);
        return await UploadToBlobAsync(fileStream, containerName);
    }

    public async Task<Guid> UploadFileAsync(string filePath, string containerName)
    {
        var fileStream = File.OpenRead(filePath);
        return await UploadToBlobAsync(fileStream, containerName);
    }

    public async Task<Stream> DownloadFileAsync(Guid fileId, string containerName)
    {
        var blob = _blobClient.GetContainerReference(containerName.ToString())
                              .GetBlockBlobReference(fileId.ToString());
        var memoryStream = new MemoryStream();
        await blob.DownloadToStreamAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteFileAsync(Guid fileId, string containerName)
    {
        var blob = _blobClient.GetContainerReference(containerName.ToString())
                              .GetBlockBlobReference(fileId.ToString());
        await blob.DeleteIfExistsAsync();
    }


    private async Task<Guid> UploadToBlobAsync(Stream fileStream, string
        containerName)
    {
        var container = _blobClient.GetContainerReference(containerName.ToString());
        await container.CreateIfNotExistsAsync();
        var fileId = Guid.NewGuid();
        var blob = container.GetBlockBlobReference(fileId.ToString());
        await blob.UploadFromStreamAsync(fileStream);
        return fileId;
    }
}
