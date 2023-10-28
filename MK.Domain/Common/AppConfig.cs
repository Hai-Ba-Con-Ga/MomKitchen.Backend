using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Common
{
    public class AppConfig
    {
        public static ConnectionStrings ConnectionStrings { get; set; }
        public static FirebaseConfig FirebaseConfig { get; set; }
        public static JwtSetting JwtSetting { get; set; }
        public static AwsCredentials AwsCredentials { get; set; }
        public static VnpayConfig VnpayConfig { get; set; }
        public static CacheConfig CacheConfig { get; set; }
    }
    public class FirebaseConfig
    {
        public string Path { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
    public class JwtSetting
    {
        public bool ValidateIssuerSigningKey { get; set; }
        public string IssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; } = true;
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; } = true;
        public string ValidAudience { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; } = true;
    }
    public class AwsCredentials
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
    }
    public class VnpayConfig
    {
        public static string VnpayConfigString = "VnpayConfig";
        public string ReturnUrl { get; set; }
        public string Url { get; set; }
        public string HashSecret { get; set; }
        public string TmnCode { get; set; }
        public string FrontendCallBack { get; set; }
    }
    public class CacheConfig
    {
        public string RedisConnectionString { get; set; } = null!;
        public string RedisInstanceName { get; set; } = null!;
        public TimeSpan? AbsoluteExpTime { get; set; } = null!;
        public TimeSpan? UnusedExpTime { get; set; } = null!;
    }
}
