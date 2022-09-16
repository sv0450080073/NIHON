using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HassyaAllrightCloud.Domain.Dto.FileSharing.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IAwsS3FileManager
    {
        Task<string> UploadFileAsync(string fileName, Stream file);
        Task<TransferFile> DownloadFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
        Task<string> CreateFolderAsync(string folderPath);
    }
    public class AwsS3FileManager : IAwsS3FileManager
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucket;

        public AwsS3FileManager(
            IAmazonS3 client)
        {
            _client = client;
            _bucket = "testholder";
        }

        public async Task<string> CreateFolderAsync(string folderPath)
        {
            var findFolderRequest = new ListObjectsV2Request();
            findFolderRequest.BucketName = _bucket;
            findFolderRequest.Prefix = folderPath;
            ListObjectsV2Response findFolderResponse = await _client.ListObjectsV2Async(findFolderRequest);

            if (findFolderResponse.S3Objects.Count > 0)
            {
                return "";
            }
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = _bucket,
                StorageClass = S3StorageClass.Standard,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.None,
                Key = folderPath,
                ContentBody = string.Empty
            };
            // add try catch in case you have exceptions shield/handling here
            try
            {
                PutObjectResponse response = await _client.PutObjectAsync(request);
                return folderPath;
            }
            catch (AmazonS3Exception)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadFileAsync(string fileName, Stream file)
        {
            var filestream = new MemoryStream();
            await file.CopyToAsync(filestream);

            var s3FileName = fileName;

            var transferRequest = new TransferUtilityUploadRequest()
            {
                ContentType = "application/octet-stream",
                InputStream = filestream,
                BucketName = _bucket,
                Key = s3FileName
            };
            transferRequest.Metadata.Add("x-amz-meta-title", fileName);

            var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(transferRequest);

            return s3FileName;
        }

        public async Task<TransferFile> DownloadFileAsync(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucket,
                Key = fileName
            };

            using (var objectResponse = await _client.GetObjectAsync(request))
            {
                if (objectResponse.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Could not find file.");
                }

                using (var responseStream = objectResponse.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var result = new MemoryStream();
                    responseStream.CopyTo(result);
                    return new TransferFile
                    {
                        Name = fileName,
                        Content = result.ToArray()
                    };
                }
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucket,
                    Key = fileName
                };

                var result = await _client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
