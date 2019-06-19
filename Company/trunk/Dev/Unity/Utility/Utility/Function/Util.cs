using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
#if !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Utility.Function
{
    public class Util
    {

#if !UNITY_EDITOR
//#else
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern long GetWindowLong(IntPtr hWnd, Int32 nIndex);
        [DllImport("user32.dll")]
        static extern long SetWindowLong(IntPtr hWnd, Int32 nIndex, long win);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public static void RemoveTitleOfWindow()
        {
            //프레임 제거 작업
            IntPtr w = GetForegroundWindow();
            long window = GetWindowLong(w, -16); //GWL_STYLE
                                                 //window &= ~(0x00000000 | 0x00C00000L | 0x00080000 | 0x00020000 | 0x00010000);
            window &= ~(0x00000000 | 0x00C00000L | 0x00080000 | 0x00040000L);
            SetWindowPos(w, -1, 0, 0, Display.main.systemWidth, Display.main.systemWidth, 1);
            //SetWindowPos(w, -1, 0, 0, Screen.width, Screen.height, 1);
            //SetWindowPos(w, -1, startMonitorX, startMonitorY, resolutionWidth * _monitor_count, resolutionHeight, 1); //
            //yield return new WaitForSeconds(1.0f);
            SetWindowLong(w, -16, window);

        }
#endif
        public static string GetLocalIP()
        {
            string localIP = "Not available, please check your network seetings!";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        public static string getTimeString(string secondTime)
        {
            int temp = 0;
            if (Int32.TryParse(secondTime, out temp))
            {
                return getTimeString(temp);
            }

            return null;
        }

        public static string getTimeString(int secondTime)
        {
            int second = secondTime % 60;
            int min = secondTime / 60;
            int hour = min / 60;
            min = min % 60;

            return hour + " : " + min + " : " + second;
        }

        public static bool SaveVideoData(WWW www, string filePath)
        {
            bool isTrue = (www.size == www.bytesDownloaded);
            if (isTrue)
            {
                try
                {
                    File.WriteAllBytes(filePath, www.bytes);
                }
                catch (Exception e)
                {
                    Debug.Log("e.StackTrace :: " + e.StackTrace);
                    return true;
                }
            }
            return false;
        }

        public static string getFileString(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            Debug.Log("[File is not Opend] Path : " + path);
            Log.e("[File is not Opend] Path : " + path);
            return null;
        }

        private static Sprite getSpriteFromTexture(Texture2D tex)
        {
            if (tex == null)
            {
                Log.e("texture2D is Null");
            }
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public static void ChangeImageFromTextrue2D(Texture2D tex, Image image)
        {
            image.sprite = getSpriteFromTexture(tex);
        }
        public static string getPriceWithComma(int i)
        {
            return getPriceWithComma(Convert.ToString(i));
        }

        public static string getPriceWithComma(string str)
        {
            return string.Format("{0:N0}", int.Parse(str));
        }

        public static Dictionary<string, string> Parser(string path)
        {

            var dictionary = new Dictionary<string, string>();
            string data = getFileString(path);
            if (data == null || data.Length == 0)
            {
                Debug.Log("Error data");
                Log.e("Error Data");
                return dictionary;
            }
            StringReader sr = new StringReader(data);
            string s = null;

            while ((s = sr.ReadLine()) != null)
            {
                s = s.Trim();
                string[] sList = s.Split('=');

                try
                {

                    if (sList != null)
                    {
                        dictionary.Add(sList[0], sList[1]);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
            }
            return dictionary;
        }
    }
}