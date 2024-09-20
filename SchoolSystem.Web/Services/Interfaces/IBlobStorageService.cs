namespace SchoolSystem.Web.Services.Interfaces;

/// <summary>
/// Service for interacting with Azure Blob Storage
/// </summary>
public interface IBlobStorageService
{
  Task<Guid> UploadFileAsync(Stream fileStream, string containerName);
  Task<Guid> UploadFileAsync(IFormFile file, string containerName);
  Task<Guid> UploadFileAsync(byte[] fileBytes, string containerName);
  Task<Guid> UploadFileAsync(string filePath, string containerName);
  Task<Stream> DownloadFileAsync(Guid fileId, string containerName);
  Task DeleteFileAsync(Guid fileId, string containerName);
}
