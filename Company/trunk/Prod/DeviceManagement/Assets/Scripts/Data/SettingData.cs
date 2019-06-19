namespace CommonData.Data
{
    public class SettingData
    {

        private static string pathForAttractionControlProgram;
        public static string PATH_FOR_ATTRACTION_CONTROL_PROGRAM
        {
            get
            {
                return pathForAttractionControlProgram;
            }
            set
            {
                pathForAttractionControlProgram = value;
            }
        }

        private static VideoDataList videoDataList;
        public static VideoDataList VideoDataList
        {
            get
            {
                return videoDataList;
            }

            set
            {
                videoDataList = value;
            }
        }

        private static string memberCount = "1";
        public static string MEMBER_COUNT
        {
            get
            {
                return memberCount;
            }

            set
            {
                memberCount = value;
            }
        }

        private static bool isActivedStartButton = false;
        public static bool IsActivedStartButton
        {
            get
            {
                return isActivedStartButton;
            }

            set
            {
                isActivedStartButton = value;
            }
        }

        private static int delayTimeForDeviceLiving;
        public static int DELAY_TIME_FOR_DEVICE_LIVING
        {
            get
            {
                return delayTimeForDeviceLiving;
            }
            set
            {
                delayTimeForDeviceLiving = value;
            }
        }

        private static string delayTimeForStartVideoAboutDevice;
        public static string DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE
        {
            get
            {
                return delayTimeForStartVideoAboutDevice;
            }
            set
            {
                delayTimeForStartVideoAboutDevice = value;
            }
        }
    }
}
