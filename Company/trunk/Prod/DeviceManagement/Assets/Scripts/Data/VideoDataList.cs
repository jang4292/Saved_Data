using System.Collections.Generic;

namespace CommonData.Data
{

    [System.Serializable]
    public class VideoDataList : Result
    {

        public int totalCnt;
        public List<VideoData> lists;

        public VideoDataList() : base()
        {
            this.lists = new List<VideoData>();
            this.totalCnt = -1;
        }

        [System.Serializable]
        public class VideoData : Result
        {

            public string index;
            public string uid;
            public string owner_uid;
            public string video_name;
            public string video_title;
            public string video_playtime;
            public string video_path;
            public string video_thumbnail;
            public string motion_name;
            public string motion_accept;
            public string price;
            public string price_default;
            public string tax_price;
            public string is_active;
            public string create_at;
            public string update_at;
            public string delete_at;
            public string machine_uid;
            public string playPrice;
            public string isUpdate;
            public string cpTax;
            public string apTax;
            public string resellerTax;
            public string adminTax;

            public VideoData() : base()
            {
                index = "";
                uid = "";
                owner_uid = "";
                video_name = "";
                video_title = "";
                video_playtime = "";
                video_path = "";
                video_thumbnail = "";
                motion_name = "";
                motion_accept = "";
                price = "";
                price_default = "";
                tax_price = "";
                is_active = "";
                create_at = "";
                update_at = "";
                delete_at = "";
                machine_uid = "";
                playPrice = "";
                isUpdate = "";
                cpTax = "";
                apTax = "";
                resellerTax = "";
                adminTax = "";
            }
        }
    }
}
