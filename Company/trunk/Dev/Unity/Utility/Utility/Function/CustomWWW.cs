using UnityEngine;
using System.Collections;
using CommonData;

namespace Utility.Function
{
    public class CustomWWW
    {

        public enum Type
        {
            IMAGE,
            VIDEO,
            JSON
        }

        public delegate void ErrorHandler(string str);
        public delegate void RequestHandler(WWW www);
        public delegate void ProgressHandler(WWW www);

        public static IEnumerator LoadData(Type type, PostData data, ErrorHandler errorHandler, RequestHandler requestHandler)
        {
            while (true)
            {
                WWW www = (data.form == null) ? new WWW(data.path) : new WWW(data.path, data.form);
                yield return www;

                if (!isErrorProcess(type, www, errorHandler))
                {
                    continue;
                }

                requestHandler(www);
                break;

            }
        }

        public static IEnumerator LoadData(Type type, PostData data, ProgressHandler progressHandler, ErrorHandler errorHandler, RequestHandler requestHandler)
        {
            
            while (true)
            {
                WWW www = (data.form == null) ? new WWW(data.path) : new WWW(data.path, data.form);
                progressHandler(www);
                yield return www;

                if (!isErrorProcess(type, www, errorHandler))
                {
                    continue;
                }

                requestHandler(www);
                break;

            }
        }
        private static bool isErrorProcess(Type type, WWW www, ErrorHandler handler)
        {
            if (!www.isDone)
            {
                Debug.Log("it's not done");
                return false;
            }
            else if (www.error != null)
            {
                Debug.Log("Error : " + www.error);
                handler(www.error);
                return false;
            }

            if (Type.IMAGE.Equals(type) && www.texture == null)
            {
                Debug.Log("Error : " + www.error);
                return false;
            }
            return true;
        }
    }
}