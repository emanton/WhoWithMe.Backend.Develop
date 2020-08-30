using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Services.SpecificModels;

namespace WhoWithMe.Services.Helpers
{
	public static class S3StorageService
	{
        private static string baseUrl = "https://whowithmedev.s3.eu-central-1.amazonaws.com/";
        private static string bucketName = "whowithmedev";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUCentral1;
        private static IAmazonS3 _s3Client;
        static S3StorageService()
        {
			string awsPublicKey = @"AKIAUL2WZ7M3CCT5K3KM";
            string awsSecretKey = @"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";
            _s3Client = new AmazonS3Client(awsPublicKey, awsSecretKey, bucketRegion);
        }

        public static async Task<string> UploadMeetingFile(long meetingId, IFormFile formFile)
        {
            string key = $"images/meetings/{meetingId}/{Guid.NewGuid()}";
            return await UploadFileToS3(formFile, key);
        }
       
        public static async Task<string> UploadUserFile(long userId, IFormFile formFile)
        {
            string key = $"images/users/{userId}/{Guid.NewGuid()}";
            return await UploadFileToS3(formFile, key);
        }

        private static async Task<string> UploadFileToS3(IFormFile file, string key)
		{
            using (var newMemoryStream = new MemoryStream())
			{
                file.CopyTo(newMemoryStream);

				var uploadRequest = new TransferUtilityUploadRequest
				{
                    BucketName = bucketName,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.
                    Key = key,
                    InputStream = newMemoryStream,
					CannedACL = S3CannedACL.PublicRead
				};

				var fileTransferUtility = new TransferUtility(_s3Client);
				await fileTransferUtility.UploadAsync(uploadRequest);
			}

            return baseUrl + key;
        }
    }
}
