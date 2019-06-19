using UnityEngine;
using DrawingAlbum;
using Drawing;
using Public;
using Store;

namespace AndroidPlugins
{
    public class AndroidPluginManager : MonoBehaviour
    {

#if UNITY_ANDROID
        /// <summary>
        /// 안드로이드 오브젝트
        /// </summary>
        private AndroidJavaObject androidPluginObject;

        /// <summary>
        /// 안드로이드 패키지 이름 (유니티 패키지도 동일)
        /// </summary>
        private const string ANDROID_PACKAGE_NAME = "com.unity3d.player.UnityPlayer";

        /// <summary>
        /// 안드로이드 오브젝트에 접근하여 가져올 액티비티 클래스 이름
        /// </summary>
        private const string ANDROID_ACTIVITY_NAME = "currentActivity";

        /// <summary>
        /// TTS 함수 이름
        /// </summary>
        private const string ANDROID_TTS = "ReadingText";

        /// <summary>
        /// 카카오톡 공유 함수 이름
        /// </summary>
        private const string ANDROID_KAKAO_SHARE = "GetDrawingShareSNS";

        /// <summary>
        /// 지메일아이디
        /// </summary>
        private const string ANDROID_GMAIL_NAME = "GetUserGmailName";

        /// <summary>
        /// 토스트
        /// </summary>
        private const string ANDROID_TOAST = "ToastMessage";

        /// <summary>
        /// 네이티브웹뷰출력
        /// </summary>
        private const string OPEN_NATIVE_WEB_VIEW = "OpenNativeWebView";
        
        /// <summary>
        /// 스크린샷 후 갤러리 정보 업데이트
        /// </summary>
        private const string ANDROID_GALLERY_REFRESH = "GetFileGalleryRefresh";

        /// <summary>
        /// InApp 아이템 구매
        /// </summary>
        private const string ANDROID_IN_APP_BUY_ITEM = "PutInAppBuyItem";

        /// <summary>
        /// 인앱 결제 실패 시 문의 메일을 보내기 
        /// </summary>
        private const string ANDROID_IN_APP_FAILED_GMAIL_POST = "PostQuestionGmail";

        /// <summary>
        /// 그림 그려줄 매니저 스크립트 ( 그림 이름을 알아내기 위해 )
        /// </summary>
        private DrawingManager drawingManager;

        private Album album;

        private StoreManager storeManager;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            Init();
        }

        void Destroy()
        {
            /// androidObject가 null이 아니라면 제거
            if (androidPluginObject != null) androidPluginObject.Dispose();
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        private bool Init()
        {
            /// 안드로이드 오브젝트 객체에 접근
            AndroidJavaClass androidJC = new AndroidJavaClass(ANDROID_PACKAGE_NAME);

            if (androidJC != null)
            {
                /// 접근한 오브젝트 객체에서 클래스를 가져온다
                androidPluginObject = androidJC.GetStatic<AndroidJavaObject>(ANDROID_ACTIVITY_NAME);

                /// 안드로이드 오브젝트 객체 제거
                androidJC.Dispose();

                if (androidPluginObject == null)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public void ShowNativeWebView()
        {
            string url = "";


        }


        /// <summary>
        /// 웹뷰출력
        /// </summary>
        /// <param name="path"></param>
        public void OpenNativeWebView(string url)
        {
            if (androidPluginObject != null)
            {
                androidPluginObject.Call(OPEN_NATIVE_WEB_VIEW, url);
            }
        }

        /// <summary>
        /// 텍스트 읽어주기
        /// </summary>
        public void TextToSpeech()
        {
            if (drawingManager == null)
            {
                drawingManager = GameObject.FindObjectOfType<DrawingManager>();
            }

            if (androidPluginObject != null)
            {
                androidPluginObject.Call(ANDROID_TTS, drawingManager.characterPrefab.name);
            }
        }

        /// <summary>
        /// SNS 공유
        /// </summary>
        public void GetDrawingShareSNS()
        {
            if (album == null)
            {
                album = GameObject.FindObjectOfType<Album>();
            }

            if (string.IsNullOrEmpty(album.seletedFilePath))
            {
                return;
            }

            if (androidPluginObject != null)
            {
                androidPluginObject.Call(ANDROID_KAKAO_SHARE, Variables.moveFolderPath + album.seletedFilePath);
            }

        }

        /// <summary>
        /// Android 토스트 메시지
        /// </summary>
        /// <param name="message"></param>
        public void GetToast(string message)
        {
            if(androidPluginObject != null)
            {
                androidPluginObject.Call(ANDROID_TOAST, message);
            }
        }

        /// <summary>
        /// 갤러리에 파일 리플레쉬
        /// </summary>
        /// <param name="path"></param>
        public void GetGalleryFileRefresh(string path)
        {
            if (androidPluginObject != null)
            {
                androidPluginObject.Call(ANDROID_GALLERY_REFRESH, path);
            }
        }

        public void GetGmailName()
        {
            if (androidPluginObject != null)
            {
                Variables.gmailName = androidPluginObject.Call<string>(ANDROID_GMAIL_NAME);
            }
        }

        public void PutInAppBuyItem(GameObject item)
        {
            if(androidPluginObject != null)
            {
                if (storeManager == null) storeManager = GameObject.FindObjectOfType<StoreManager>();
                storeManager.SetStoreButtonsShow(false);
                androidPluginObject.Call(ANDROID_IN_APP_BUY_ITEM, item.name, Variables.gmailName);
            }
        }

        public void PostInAppFailedGmail()
        {
            if(androidPluginObject != null)
            {
                androidPluginObject.Call(ANDROID_IN_APP_FAILED_GMAIL_POST);
            }
        }

#endif // UNITY_ANDROID
    }
}
