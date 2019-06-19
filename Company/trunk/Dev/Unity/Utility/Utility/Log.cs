using UnityEngine;
using System;
using System.IO;

namespace Utility
{
    public class Log
    {
        public static void d(string className, string str)
        {
            d("[" + className + "]" + str);
        }

        public static void w(string className, string str)
        {
            w("[" + className + "]" + str);
        }

        public static void e(string className, string str)
        {
            e("[" + className + "]" + str);
        }

        public static void i(string className, string str)
        {
            i("[" + className + "]" + str);
        }

        public static void d(string str)
        {
            log("[Debug] " + str); Debug.Log("[Debug] " + str);
        }

        public static void w(string str)
        {
            log("[Waring] " + str); Debug.Log("[Waring] " + str);
        }

        public static void e(string str)
        {
            log("[Error] " + str);
            Debug.Log("[Error] " + str);
        }


        public static void i(string str)
        {
            log("[Info] " + str);
            Debug.Log("[Info] " + str);
        }


        private static string GetDataTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }

        private static void log(string str)
        {

            string FilePath = Application.dataPath + @"\Logs\Log" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = Application.dataPath + @"\Logs";

            string temp;

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(DirPath);
                FileInfo fileInfo = new FileInfo(FilePath);

                if (dirInfo.Exists != true)
                {
                    Directory.CreateDirectory(DirPath);
                }

                if (fileInfo.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] : {1}", GetDataTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] : {1}", GetDataTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }
        }
    }
}