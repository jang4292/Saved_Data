using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Api;
using Network;
using Data;

namespace Splash
{
    public class SplashTimer : MonoBehaviour
    {

        [SerializeField]
        [Range(1f, 5f)]
        private float waitTime = 1f;

        void Start()
        {

            CommonURL.ServerType = CommonURL.SERVER_TYPE.PRODCUT;

            if (!PublicClass.isVersionChecked)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    UserApi userApi = new UserApi(OnHttpRequest);
                    userApi.PostAppVersionCheck(Url.Instance.VERSION_URL, API_CODE.API_VERSION);
                }
            }

            Debug.Log("#####");

            StartCoroutine(LoadScences());



        }

        IEnumerator LoadScences()
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(Common.ScenesName.MAIN_SCENES, LoadSceneMode.Additive);

        }

        void OnHttpRequest(int id, WWW www)
        {
            //Debug.Log("OnHttpRequest : " + www.text);
            if (www.error == null)
            {
                switch (id)
                {
                    case API_CODE.API_VERSION:
                        
                        if (IsNeededUpdatedVersion(www.text))
                        {
                            //versionPopup.SetActive(true);
                            Debug.Log("NeedUpdate");
                        }
                        else
                        {
                            Debug.Log("UnNeedUpdate");
                            //plugin.GetGmailName();

                            //userApi.PostUserInfoInsert(CommonUrl.USER_URL, API.API_USER_INSERT);

                            
                        }
                        break;

                }
            } else
            {
                Debug.Log("www.error :: " + www.error);
            }
        }




        /// <summary>
        /// 버전 체크
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private bool IsNeededUpdatedVersion(string json)
        {
            try { 
            VersionData data = JsonUtility.FromJson<VersionData>(json);
                                   
            if (SetVersionParse(Application.version) < SetVersionParse(data.version))
            {
                return true;
            }
            } catch
            {
                Debug.Log("Exception");
            }

            return false;
        }

        /// <summary>
        /// 버전 문자열값을 int 값으로 변환 ex) "1.0.0" -> 100
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int SetVersionParse(string data)
        {
            return int.Parse(data.Replace(".", "")); ;
        }
    }
}