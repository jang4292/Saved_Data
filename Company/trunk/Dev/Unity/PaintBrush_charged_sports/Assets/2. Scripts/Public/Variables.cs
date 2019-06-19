using UnityEngine;
using System.Collections.Generic;

namespace Public
{
    public class Variables
    {
        public static string name;

        public static bool versionCheck;

        public static string moveFolderPath = "/mnt/sdcard/Mamp/PaintBrush/";

        public static string assetBundlePath = "/mnt/sdcard/Mamp/PaintBrush/Asset/";

        public static string gmailName = "";

        public static int userIdx = -1;

        public static int userPoint = 0;

        public static bool serverConnectFailed;

        /// <summary>
        /// 캐릭터 이름들
        /// </summary>
        [System.NonSerialized]
        public static List<string> characterNames = new List<string>() { "cat", "mouse", "chicken", "squirrel", "lion", "bull", "rabbit", "teddy", "bear", "sheep", "pig", "peinguin", "dog", "monkey", "fox", "gorilla" };
    }
}
