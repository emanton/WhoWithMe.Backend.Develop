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
        private static string bucketName = "whowithmedev";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUCentral1;
        private static IAmazonS3 _s3Client;
        static S3StorageService()
        {
			string awsPublicKey = @"AKIAUL2WZ7M3CCT5K3KM";
            string awsSecretKey = @"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";
            _s3Client = new AmazonS3Client(awsPublicKey, awsSecretKey, bucketRegion);
        }

        public static async Task<string> UploadMeetingFile(MeetingPhoto userPhoto)
        {
            string key = $"images/meetings/{userPhoto.MeetingId}/{Guid.NewGuid()}";
            return await UploadFileToS3(userPhoto.FormFile, key);
        }
       
        public static async Task<string> UploadUserFile(UserPhoto userPhoto)
		{
            string key = $"images/users/{userPhoto.UserId}/{Guid.NewGuid()}";
            return await UploadFileToS3(userPhoto.FormFile, key);
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

            GetPreSignedUrlRequest expiryUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.Now.AddMinutes(5)
            };

            string fileUrl = _s3Client.GetPreSignedURL(expiryUrlRequest);
            return fileUrl;
        }
    }
}
