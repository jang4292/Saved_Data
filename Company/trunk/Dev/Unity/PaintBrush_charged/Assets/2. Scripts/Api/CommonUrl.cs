
namespace Api
{
    public class CommonUrl
    {
        // Service Server
        public static string BASE_URL = "http://211.115.127.168/paintbrush/";

        //public static string BASE_URL = "http://106.240.7.189:7198/paintbrush/";

        //public static string BASE_URL = "http://106.240.7.189:9999/paintbrush/";

        /** Service Server include SSL */
        private const string SSL_BASE_URL = "https://mampcorp.appspot.com/";

        // Test Server
        //public static string BASE_URL = "http://61.85.36.74:7198/paintbrush/";

        // Development Server
        //public static string BASE_URL = "http://61.85.36.74:9999/paintbrush/";

        // POST 형식
        // 버전 및 패키지 정보
        public static string VERSION_URL = BASE_URL + "ver.chk";

        // POST 형식
        // 제품 정보
        public static string PRODUCT_URL = BASE_URL + "list.go";

        public static string PRODUCT_THEMA_URL = BASE_URL + "thema";

        // POST 형식
        // 사용자 정보
        public static string USER_URL = BASE_URL + "user.mb";

        private const string POLICY_TERM_URL = @"term.html?pk=";

        //private const string PAKAGE_NAME = "com.mamp.paintbrush";
        //private const string PAKAGE_NAME = "com.mamp.paintbrush_charged_all";
        private const string PAKAGE_NAME = "com.mamp.paintbrush_charged_historical";

        // 개인정보보호
        public const string PERSONAL_INFORMATION_URL = SSL_BASE_URL + POLICY_TERM_URL + PAKAGE_NAME;


        //private const string PERSONAL_INFORMATION_URL = @"https://mampcorp.appspot.com/term.html?pk=com.mamp.paintbrush";

    }
}
