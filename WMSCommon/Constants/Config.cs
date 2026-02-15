using System;
using System.Collections.Generic;
using System.Text;

namespace WMSCommon.Constants
{
    public static class Config
    {
        public const string CommonFolderName = "WMSCommon";
        public const string CommonAppSettingsFilename = "common_appsettings.json";
        public const string CommonAppSettingsDevFilename = 
            "common_appsettings.Development.json";
        public const string AppSettingsFilename = "appsettings.json";
        public const string AppSettingsDevFilename = "appsettings.Development.json";

        public const string CorsPolicyName = "DevPolicy";
        public const string SiteUrl = "https://app.wms.com";

        public const string JwtIssuerKey = "JWT:Issuer";
        public const string JwtAudienceKey = "JWT:Audience";
        public const string JwtSigningKey = "JWT:SigningKey";

        public const string MQHost = "RabbitMQHost";
        public const string MQPort = "RabbitMQPort";
        public const string MQUsername = "RabbitMQUsername";
        public const string MQPassword = "RabbitMQPassword";
    }
}
