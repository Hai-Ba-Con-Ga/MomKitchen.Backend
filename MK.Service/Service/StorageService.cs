using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using MK.Domain.Dto.Request.Storage;
using MK.Domain.Dto.Response.Storage;

namespace MK.Service.Service
{
    public class StorageService : BaseService, IStorageService
    {
        public StorageService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ResponseObject<StorageRes>> UploadFile(StorageReq storageReq)
        {
            try
            {
                //handle upload file
                var creadentials = new BasicAWSCredentials(AppConfig.AwsCredentials.AccessKey, AppConfig.AwsCredentials.SecretKey);
                var config = new AmazonS3Config()
                {
                    RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
                };
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    //upload to /foleName
                    InputStream = storageReq.File.OpenReadStream(),
                    Key = Guid.NewGuid().ToString(),
                    BucketName = AppConfig.AwsCredentials.BucketName,
                    CannedACL = S3CannedACL.PublicRead
                };
                using var client = new AmazonS3Client(creadentials, config);
                // initialise the transfer/upload tools
                var transferUtility = new TransferUtility(client);

                // initiate the file upload
                await transferUtility.UploadAsync(uploadRequest);
                var url = $"{AppConfig.AwsCredentials.BucketName}/{uploadRequest.Key}";
                return Success(new StorageRes { Url = url });
            }
            catch (Exception ex)
            {
                return BadRequest<StorageRes>(ex.Message);
            }
        }
    }
}
