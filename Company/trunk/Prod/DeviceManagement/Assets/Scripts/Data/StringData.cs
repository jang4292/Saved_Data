namespace CommonData.Data
{
    public class StringData : CommonData.StringData
    {
        public const string ACCESS_NODE_PATH = "/configuration/video";

        /*settings file*/
        public const string PAHT_FOR_ATTRACTION_CONTROL_PROGRAM = "path";
        public const string MACHINE_UID = "machineUID";
        public const string DELAY_TIME_FOR_CHECKING_ABOUT_LIVING_DEVICE = "device_check_time";
        public const string DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE = "delay_video_time";
        public const string PORT_NUMBER = "portNumber";

        public const string XML_FILE_NAME = "configuration.xml";
        public const string IMG_FOLER_NAME = @"img\";

        public class ConfigurationString
        {
            public const string VIDEO = "video";
            public const string PICTURE = "picpath";
            public const string MOVIE = "movpath";
            public const string SIMPATH = "simpath";
            public const string MOVIE_TIME = "movtime";
            public const string PRICE = "price";
        }
    }
}
