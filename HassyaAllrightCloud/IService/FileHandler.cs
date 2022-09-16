
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.FileSharing.Models;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IFileHandler
    {
        Task<TransferFile> DownloadFileAsync(string fileName);
        Task UploadFileAsync(FileSendData fileSendData);
        Task CreateFoldersAsync(string path);
    }

    public class FileHandler : IFileHandler
    {
        private readonly KobodbContext _context;
        private readonly IAwsS3FileManager _s3FileManager;


        public FileHandler(
            KobodbContext context,
            IAwsS3FileManager s3FileManager
            )
        {
            _context = context;
            _s3FileManager = s3FileManager;
        }

        public async Task CreateFoldersAsync(string path)
        {
            var folder = await _s3FileManager.CreateFolderAsync(path);

        }
        public async Task<TransferFile> DownloadFileAsync(string fileName)
        {
            var file = await _s3FileManager.DownloadFileAsync(fileName);
            return file;
        }
        public async Task UploadFileAsync(FileSendData fileSendData)
        {
            var s3fileName = await _s3FileManager.UploadFileAsync(fileSendData.File.Name, fileSendData.File.Data);
            var fileStorage = new FileStorageData
            {
                FileSendDataId = fileSendData.Id,
                FileName = s3fileName
            };
        }
    }
}

