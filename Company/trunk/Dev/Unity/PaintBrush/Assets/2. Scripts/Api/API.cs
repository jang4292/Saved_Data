
namespace Api
{
    public class API
    {
        public WWWHelper helper;

        public WWWHelper.HttpRequestDelegate mRequest;

        public void MinusRequest()
        {
            if (helper != null) helper.OnHttpRequest -= mRequest;
        }

        public const int API_VERSION = 10;

        public const int API_USER_INSERT = 11;
        public const int API_USER_UPDATE = 12;

        public const int API_USER_GCM_ID_INSERT = 13;
        public const int API_USER_GCM_ID_UPDATE = 14;

        public const int API_PRODUCT_DESIGN_NEW_LIST = 20;
        public const int API_PRODUCT_DESING_BUY_LIST = 21;

        public const int API_PRODUCT_SINGLE_SEARCH = 29;
        public const int API_PRODUCT_SINGLE_STORE_LIST = 30;
        public const int API_PRODUCT_SINGLE_STORE_REFRESH_LIST = 31;
        public const int API_PRODUCT_SINGLE_STORE_LIST_PAGEING = 32;

        public const int API_USER_SINGLE_BUY_PRODUCT = 40;
        public const int API_USER_IN_APP_CASH = 45;
    }
}
