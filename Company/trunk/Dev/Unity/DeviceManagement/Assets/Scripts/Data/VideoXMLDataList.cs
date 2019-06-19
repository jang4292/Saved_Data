using System.Collections.Generic;

namespace CommonData.Data
{
    public class VideoXMLDataList
    {

        public static List<XMLData> lists = new List<XMLData>();

        public class XMLData
        {
            public string title;
            public string picturePath;
            public string moviePath;
            public string simPath;
            public string playTime;

            public XMLData(string t, string p, string m, string s, string d)
            {
                title = t;
                picturePath = p;
                moviePath = m;
                simPath = s;
                playTime = d;

            }
        }
    }
}