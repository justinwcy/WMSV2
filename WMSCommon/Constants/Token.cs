namespace WMSCommon.Constants
{
    public static class Token
    {
        public const string AccessToken = "WMSAccessToken";
        public const string RefreshToken = "WMSRefreshToken";

        // expiry time is in seconds
        public const double AccessTokenExpiryTime = 5 * 60;
        public const double RefreshTokenExpiryTime = 7 * 24 * 60 * 60;
    }
}
