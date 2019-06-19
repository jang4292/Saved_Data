
namespace Network
{
    public abstract class CommonURL
    {

        protected abstract string BaseURL { get; }

        public enum SERVER_TYPE { PRODCUT, TEST, DEVELOP, NONE };

        private static SERVER_TYPE serverType = SERVER_TYPE.NONE;
        public static SERVER_TYPE ServerType
        {
            set
            {
                serverType = value;
            }
        }


        private const string PRODUCT_SERVER = @"http://211.115.127.168";
        private const string TEST_SERVER = @"http://106.240.7.189";
        /** Service Server include SSL */
        private const string SSL_BASE_URL = "https://mampcorp.appspot.com/";

        private const string TEST_PORT = @":7198";
        private const string DEVELOP_PORT = @":9999";

        // 버전 및 패키지 정보 (POST형식)
        public string VERSION_URL
        {
            get
            {
                return BaseURL + Common.Util.String.VERSION;
            }
        }

        protected string ServerAddress
        {
            get
            {
                switch (serverType)
                {
                    case SERVER_TYPE.PRODCUT:
                        return PRODUCT_SERVER;


                    case SERVER_TYPE.TEST:
                        return TEST_SERVER + ":" + TEST_PORT;

                    case SERVER_TYPE.DEVELOP:
                        return TEST_SERVER + ":" + DEVELOP_PORT;

                    default:
                        return "Error";
                        //TODO To need to add Exception Logic.
                }
            }
        }





        /*
        // POST 형식
        // 제품 정보
        public static string PRODUCT_URL = BASE_URL + "list.go";

        public static string PRODUCT_THEMA_URL = BASE_URL + "thema";

        // POST 형식
        // 사용자 정보
        public static string USER_URL = BASE_URL + "user.mb";

        private const string POLICY_TERM_URL = @"term.html?pk=";
        */



    }
}