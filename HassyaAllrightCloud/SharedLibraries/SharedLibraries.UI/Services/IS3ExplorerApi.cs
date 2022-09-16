using Microsoft.Extensions.Configuration;
using SharedLibraries.Utility.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using SharedLibraries.Utility.Exceptions;
using System.Threading.Tasks;
using System.Web;
using System;

namespace SharedLibraries.UI.Services
{
    public interface ISharedLibrariesApi
    {
        Task<TransferFile> DownloadFileAsync(DownloadModel model);
        Task<List<S3File>> GetFilesAsync(string folderId);
        Task<List<S3Folder>> GetFoldersAsync(string folderId);
        Task<S3File> UploadFileAsync(FileSendData model);
        Task<S3File> GetSingleFileAsync(string fileId);
        Task<List<TreeNode>> GetChild(string folderId, int tenantCd, int level);
        double ToKB(long bytes);
        double ToMB(long bytes);
        Task<List<DownloadedInfo>> GetDownloadHistory(string fileId);
        Task RenameFile(RenameFileModel model);
        Task RenameFolder(RenameFolderModel model);
        Task RemoveFile(string fileId);
        Task RemoveFolder(string folderId);
        Task MoveFile(MoveFileModel model);
        Task MoveFolder(MoveFolderModel model);
        Task<string> CreateNewFolder(FolderModel folderModel);
        Task ChangeFileStatus(UpdateFileStatusModel model);
        string GetFileIcon(string fileType);
        Task<List<TenantItem>> GetTenants();
        /// <summary>
        /// Build download file url from S3
        /// </summary>
        /// <param name="baseUri">Base uri of HOC_Kashikiri</param>
        /// <param name="encryptedFileId"></param>
        /// <returns></returns>
        string BuildDownloadUrl(string baseUri, string encryptedFileId);
        Task<S3Folder> GetFolderAsync(string folderId);
    }

    public class SharedLibrariesApi : ISharedLibrariesApi
    {
        public readonly string _url;
        public SharedLibrariesApi(IConfiguration config)
        {
            _url = config.GetSection("ServiceUrls:S3API")?.Value;
            if (string.IsNullOrEmpty(_url)) throw new System.Exception($"Please add ServiceUrls:S3API into appsetting.json");
            else
                _url += "/S3File";
        }
        public async Task<TransferFile> DownloadFileAsync(DownloadModel model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync($"{_url}/download", model);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<TransferFile>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task<List<S3File>> GetFilesAsync(string folderId)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_url}/files?folderId={HttpUtility.UrlEncode(folderId)}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<List<S3File>>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task<List<S3Folder>> GetFoldersAsync(string folderId)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_url}/folders?folderId={HttpUtility.UrlEncode(folderId)}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<List<S3Folder>>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task<S3Folder> GetFolderAsync(string folderId)
        {
            using (var client = new HttpClient())
            {
                var url = $"{_url}/folder?folderId={HttpUtility.UrlEncode(folderId)}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<S3Folder>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task<S3File> UploadFileAsync(FileSendData model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync($"{_url}/upload", model);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<S3File>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task<S3File> GetSingleFileAsync(string fileId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_url}/file?fileId={fileId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<S3File>();
                }
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task<List<TreeNode>> GetChild(string folderId, int tenantCd, int level)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_url}/subfolders?folderId={HttpUtility.UrlEncode(folderId)}&tenantCd={tenantCd}&level={level}");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<List<TreeNode>>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public double ToKB(long bytes)
        {
            return bytes * 1.0 / 1024;
        }
        public string GetFileIcon(string fileType)
        {
            switch (fileType)
            {
                case "txt":
                    return "fa fa-file-text-o";
                case "pdf":
                    return "fa fa-file-pdf-o";
                case "png":
                case "jpeg":
                    return "fa fa-file-image-o";
                case "docx":
                    return "fa fa-file-word-o";
                case "xlsx":
                    return "fa fa-file-excel-o";
                case "pptx":
                    return "fa fa-file-powerpoint-o";
                default:
                    return "fa fa-file-o";
            }
        }

        public async Task<List<DownloadedInfo>> GetDownloadHistory(string fileId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_url}/download-history?fileId={HttpUtility.UrlEncode(fileId)}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<DownloadedInfo>>();
                }
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public async Task RenameFile(RenameFileModel model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsJsonAsync($"{_url}/rename-file", model);
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public async Task RenameFolder(RenameFolderModel model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsJsonAsync($"{_url}/rename-folder", model);
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public async Task RemoveFile(string encryptedId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{_url}/file?fileId={HttpUtility.UrlEncode(encryptedId)}");
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public async Task RemoveFolder(string encryptedId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{_url}/folder?folderId={HttpUtility.UrlEncode(encryptedId)}");
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public async Task MoveFile(MoveFileModel model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsJsonAsync($"{_url}/move-file", model);
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public async Task MoveFolder(MoveFolderModel model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsJsonAsync($"{_url}/move-folder", model);
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public async Task<string> CreateNewFolder(FolderModel folderModel)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync($"{_url}/folder", folderModel);
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private void HandleExceptionResponse(HttpResponseMessage msg)
        {
            switch (msg.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new EntityNotFoundException();
                case HttpStatusCode.Conflict:
                    throw new DuplicateException();
                default:
                    throw new Exception();
            }
        }

        public async Task ChangeFileStatus(UpdateFileStatusModel model)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsJsonAsync($"{_url}/file-status", model);
                if (!response.IsSuccessStatusCode)
                    HandleExceptionResponse(response);
            }
        }

        public double ToMB(long bytes)
        {
            return ToKB(bytes) / 1024;
        }

        public async Task<List<TenantItem>> GetTenants()
        {
            using (var client = new HttpClient())
            {
                var url = $"{_url}/tenants";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<List<TenantItem>>();
                else
                {
                    HandleExceptionResponse(response);
                    return null;
                }
            }
        }

        public string BuildDownloadUrl(string baseUri, string encryptedFileId) => $"{baseUri}FileDownload?fileId={HttpUtility.UrlEncode(encryptedFileId)}";
    }
}
