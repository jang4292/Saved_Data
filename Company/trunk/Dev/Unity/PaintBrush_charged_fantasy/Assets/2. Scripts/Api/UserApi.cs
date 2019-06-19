﻿using UnityEngine;
using System.Collections.Generic;
using Public;

namespace Api
{
    public class UserApi : API
    {
        public UserApi(WWWHelper.HttpRequestDelegate request)
        {
            if (helper == null) helper = WWWHelper.Instance;

            mRequest = request;
        }

        public void PostAppVersionCheck(string url, int id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //data.Add("mth", "select");
            data.Add("mth", "select_ver");
            //data.Add("package", "com.mamp.paintbrush_charged1"); 
            data.Add("package", "com.mamp.paintbrush_charged_fantasy");
            
            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostUserInfoInsert(string url, int id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "new");
            data.Add("id", Variables.gmailName);
            data.Add("uuid", SystemInfo.deviceUniqueIdentifier);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostUserInfoUpdate(string url, int id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "user_update");
            data.Add("id", Variables.gmailName);
            data.Add("uuid", SystemInfo.deviceUniqueIdentifier);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostUserGCMInfoInsert(string url, int id, string gcmId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "gcm");
            data.Add("idx", Variables.userIdx + "");
            data.Add("gid", gcmId);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostUserGCMInfoUpdate(string url, int id, string gcmId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "gcm_update");
            data.Add("idx", Variables.userIdx + "");
            data.Add("gid", gcmId);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }
    }
}
