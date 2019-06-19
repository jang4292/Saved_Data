namespace CommonData.Data
{
    public class URL
    {
        private const string BASE_URL = @"http://54.199.183.194/admin/";
        private const string BASE_URL_FOLDER = "mamp/attraction";

        public const string DEVELOP_URL = BASE_URL + BASE_URL_FOLDER;

        private const string DEVELOP_LOGIN = "login";
        public const string DEVELOP_LOGIN_URL = DEVELOP_URL + "/" + DEVELOP_LOGIN;

        private const string DEVELOP_BUY_VIDEO_LIST = "buyVideoList";
        public const string DEVELOP_BUY_VIDEO_LIST_URL = DEVELOP_URL + "/" + DEVELOP_BUY_VIDEO_LIST;

        private const string DEVELOP_NEW_VIDEO_LIST = "newVideoList";
        public const string DEVELOP_NEW_VIDEO_LIST_URL = DEVELOP_URL + "/" + DEVELOP_NEW_VIDEO_LIST;

        private const string DEVELOP_BUY = "buy";
        public const string DEVELOP_BUY_URL = DEVELOP_URL + "/" + DEVELOP_BUY;

        private const string DEVELOP_SEND_STRAT_MESSAGE = "startlog";
        public const string DEVELOP_SEND_STRAT_MESSAGE_URL = DEVELOP_URL + "/" + DEVELOP_SEND_STRAT_MESSAGE;

        private const string DEVELOP_SEND_END_MESSAGE = "endlog";
        public const string DEVELOP_SEND_END_MESSAGE_URL = DEVELOP_URL + "/" + DEVELOP_SEND_END_MESSAGE;
    }
}