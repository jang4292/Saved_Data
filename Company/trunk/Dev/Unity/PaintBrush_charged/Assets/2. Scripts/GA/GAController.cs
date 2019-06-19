using UnityEngine;
using UnityEngine.SceneManagement;
using Public;

namespace GA
{
    public class GAController : MonoBehaviour
    {
        public GoogleAnalyticsV4 googleAnalytics;

        void Start()
        {
            Instantiate(googleAnalytics);
            googleAnalytics.StartSession();
        }

        void OnApplicationQuit()
        {
            googleAnalytics.StopSession();
        }

        /// <summary>
        /// 서버 연동 실패 시 - Main
        /// </summary>
        public void PostServerNonConnect(string reason)
        {
            googleAnalytics.LogScreen(SceneManager.GetActiveScene().name);

            EventHitBuilder builder = new EventHitBuilder();
            builder.SetEventCategory("ServerNonConnect");
            builder.SetEventAction(Variables.gmailName + " / " + System.DateTime.Now);
            builder.SetEventLabel(reason);

            // 종류 - 사용자ID / 날짜 - 이유
            googleAnalytics.LogEvent(builder);
        }

        /// <summary>
        /// 인앱 결제 후 서버연동 실패 시 - Store
        /// </summary>
        public void PostInAppServerNonConnect(string reason, long money)
        {
            googleAnalytics.LogScreen(SceneManager.GetActiveScene().name);
            // 종류 - 사용자ID / 날짜 - 이유 - 가격
            googleAnalytics.LogEvent("InAppNonConnect", Variables.gmailName + " / " + System.DateTime.Now, reason, money);
        }
    }
}
