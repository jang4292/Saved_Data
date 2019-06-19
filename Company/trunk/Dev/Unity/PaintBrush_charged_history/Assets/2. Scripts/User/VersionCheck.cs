//#define Test

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Api;
using AndroidPlugins;
using Vo;
using GoogleCloudMessage;
using GA;
using Public;

namespace User
{
    public class VersionCheck : MonoBehaviour
    {
        /// <summary>
        /// 버전체크 팝업
        /// </summary>
        public GameObject versionPopup;

        /// <summary>
        /// 유저 정보 업데이트 팝업
        /// </summary>
        public GameObject userInfoUpdatePopup;

        /// <summary>
        /// 서버에서 받은 앱 패키지네임
        /// </summary>
        private string appPackgeName = "";

        private int version;

        private WWWHelper helper;

        public AndroidPluginManager plugin;

        private UserApi userApi;

        private UserInfoVo userVo;

        // Project Number on Google API Console
        private string[] SENDER_IDS = { "386948255202" };

        public GameObject networkPopup;
        public Text networkText;

        public GAController gAController;

        public GameObject dataLoadingPopup;
        public Text dataLoadingProgress;
        private bool dataLoading;
        private float delaySpeed = .5f;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
#if Test
            Variables.versionCheck = true;
#endif
            if (Variables.versionCheck)
            {
                Destroy(gameObject);
            }
            else
            {
                /// 네트웍이 연결되어있으면
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    if (!Variables.versionCheck)
                    {
                        dataLoadingPopup.SetActive(true);
                        StartCoroutine(GetDataLoading());

                        /* TODO GCM 처리
                        GCM.Initialize();
                        GCM.Register(SENDER_IDS);
                        */

                        userApi = new UserApi(OnHttpRequest);
                        userApi.PostAppVersionCheck(CommonUrl.VERSION_URL, API.API_VERSION);

                        Variables.versionCheck = true;
                    }
                }
                else
                {
                    networkPopup.SetActive(true);
                }
            }

        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        IEnumerator GetDataLoading()
        {
            while (!dataLoading)
            {
                dataLoadingProgress.text = "....";

                yield return new WaitForSeconds(delaySpeed);

                dataLoadingProgress.text = "...";

                yield return new WaitForSeconds(delaySpeed);

                dataLoadingProgress.text = "..";

                yield return new WaitForSeconds(delaySpeed);

                dataLoadingProgress.text = ".";

                yield return new WaitForSeconds(delaySpeed);

                yield return null;
            }
        }

        private void SetServerConnectPopup(string error)
        {
            dataLoadingPopup.SetActive(false);
            dataLoading = true;

            networkText.text = "서버 점검 중 입니다.";
            networkText.fontSize = 65;

            networkPopup.SetActive(true);

            gAController.PostServerNonConnect(error);
        }

        void OnHttpRequest(int id, WWW www)
        {
            //Debug.Log("www.text11 :: " + www.text);
            if (Variables.serverConnectFailed)
            {
                SetServerConnectPopup("ServerError : TimeOut");
            }
            else
            {
                if (www.error == null)
                {
                    switch (id)
                    {
                        case API.API_VERSION:
                            dataLoadingPopup.SetActive(false);
                            dataLoading = true;

                            /// 버전 낮으면
                            /// 
                            //Debug.Log("www.text :: " + www.text);
                            if (!IsVersion(www.text))
                            {
                                versionPopup.SetActive(true);
                            }
                            else
                            {
                                plugin.GetGmailName();

                                userApi.PostUserInfoInsert(CommonUrl.USER_URL, API.API_USER_INSERT);
                            }
                            break;
                        case API.API_USER_INSERT:
                            if (IsDeviceId(www.text))
                            {
                                Variables.userIdx = userVo.idx;
                                Variables.userPoint = userVo.point;
                            }
                            else 
                            {
                                userInfoUpdatePopup.SetActive(true);
                            }

                            /* TODO GCM 처리
                            /// 다르면 Update
                            if (IsGCMId(www.text) == 1)
                            {
                                userApi.PostUserGCMInfoUpdate(CommonUrl.USER_URL, API.API_USER_GCM_ID_UPDATE, GCM.GetRegistrationId());
                            }
                            /// 빈값이면 Insert
                            else if (IsGCMId(www.text) == 2)
                            {
                                userApi.PostUserGCMInfoInsert(CommonUrl.USER_URL, API.API_USER_GCM_ID_INSERT, GCM.GetRegistrationId());
                            }
                            */
                            break;
                        case API.API_USER_UPDATE:
                            userVo = JsonUtility.FromJson<UserInfoVo>(www.text);

                            if (userVo.result)
                            {
                                Variables.userIdx = userVo.idx;
                                Variables.userPoint = userVo.point;
                                plugin.GetToast("현재 기기로 등록이 완료 되었습니다.");
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(userVo.resultMsg) && !userVo.resultMsg.Contains("Exception"))
                                {
                                    plugin.GetToast(userVo.resultMsg);
                                }
                                else
                                {
                                    plugin.GetToast("현재 기기로 등록이 실패 되었습니다.");
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Debug.Log("[Version & User Check] : " + www.error);

                    SetServerConnectPopup(www.error);

                }
                userApi.MinusRequest();
            }
        }

        /// <summary>
        /// 버전 체크
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        bool IsVersion(string json)
        {
            VersionVo data = JsonUtility.FromJson<VersionVo>(json);

            //Debug.Log("data : : " + data);
            //Debug.Log("data.version : : " + data.version);
            //Debug.Log("data.packageName : : " + data.packageName);

            appPackgeName = data.packageName;

            /// 버전이 낮으면
            if (SetVersionParse(Application.version) < SetVersionParse(data.version))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 버전 문자열값을 int 값으로 변환 ex) "1.0.0" -> 100
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int SetVersionParse(string data)
        {
            version = int.Parse(data.Replace(".", ""));
            return version;
        }

        bool IsDeviceId(string json)
        {
            userVo = JsonUtility.FromJson<UserInfoVo>(json);

            if (userVo.uuid.Equals(userVo.requuid) || string.IsNullOrEmpty(userVo.requuid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 0 = 같음, 1 = 다름 (Update), 2 = 없음 (Insert)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// 

            /* TODO GCM 처리
        int IsGCMId(string json)
        {
            userVo = JsonUtility.FromJson<UserInfoVo>(json);

            /// GCM ID가 같으면
            if(userVo.gid.Equals(GCM.GetRegistrationId()))
            {
                return 0;

            }
            /// GCM ID가 다르면 Update
            else if (userVo.gid.Equals(GCM.GetRegistrationId()))
            {
                return 1;
            }
            /// GCM ID가 없으면 Insert
            else if (string.IsNullOrEmpty(userVo.gid))
            {
                return 2;
            }

            return -1;
        }
        */
        /// <summary>
        /// 유저 uuid 정보 변경
        /// </summary>
        public void UserInfoUpdate()
        {
            userApi.PostUserInfoUpdate(CommonUrl.USER_URL, API.API_USER_UPDATE);
        }

        /// <summary>
        /// 마켓으로 바로 가기
        /// </summary>
        public void GoogleMarketGoing()
        {
            Application.OpenURL("market://details?id=" + appPackgeName);
        }
    }

}
