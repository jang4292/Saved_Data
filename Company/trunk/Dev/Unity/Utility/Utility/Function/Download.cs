using CommonData;
using System.Collections;
using UnityEngine;
namespace Utility.Function
{
    public class Download : MonoBehaviour
    {
        private const string ClassName = "Download";

        private static Download current = null; // 이 클래스의 싱글톤 객체 
        private static GameObject container = null; // 객체를 생성하기 위한 GameObject

        public static Download Instance// 싱글톤 객체 만들기
        {
            get
            {
                if (current == null)
                {
                    container = new GameObject();
                    container.name = ClassName;
                    current = container.AddComponent(typeof(Download)) as Download;
                }
                return current as Download;
            }
        }

        public class DownloadForm
        {
            public string downloadURL;
            public string savePath;

            public DownloadForm(string downloadURL, string savePath)
            {
                this.downloadURL = downloadURL;
                this.savePath = savePath;
            }
        }

        public delegate void Finished(bool isSaved);
        public delegate void Progress(int i);
        public void StartDownload(DownloadForm form, Finished finished, Progress progress)
        {
            StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.VIDEO, new PostData(form.downloadURL), (WWW www) =>
            {
                StartCoroutine(ProgressView(www, progress));
            },
                (string str) =>
                {
                    Debug.Log("error str :: " + str);
                }, (WWW www) =>
                {
                    if (!Util.SaveVideoData(www, form.savePath))
                    {
                        //Debug.Log("SaveCorrect");
                        finished(true);
                    } else
                    {
                        Log.e("[Download] Save Faile");
                        //Debug.Log("Save fail");
                        finished(false);
                    }
                }));
        }

        private IEnumerator ProgressView(WWW www, Progress progress)
        {
            while (true)
            {
                //yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(1);

                //Debug.Log("www.progress :: " + www.progress * 100);
                int nProgress = (int)(www.progress * 100);

                //Debug.Log("nProgress :: " + nProgress);

                progress(nProgress);
                if (nProgress >= 99) break;
            }
        }
    }
}