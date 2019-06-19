using UnityEngine;

namespace Common.Util
{
    public class String
    {
        private string packageName = null;

        public string PackageName
        {
            get
            {
                if (packageName == null)
                {
                    packageName = Application.installerName;
                }

                return packageName;
            }
        }

        public const string VERSION = @"ver.chk"; // 버전 및 패키지 정보(POST형식)
        public const string PRODUCT = @"list.go"; // 제품 정보(POST형식)
        public const string THEMA = @"thema"; // 테마정보

        public const string USER = @"user.mb"; // 사용자정보(POST형식)
        public const string POLICY_TERM = @"term.html?pk="; //개인정보보호

        public const string ANDROID_GMAIL_NAME = "GetUserGmailName"; // GMail ID
        public const string ANDROID_TOAST = "ToastMessage"; // Toast 출력
    }
}
