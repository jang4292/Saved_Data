﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CommonData.Data;
using Utility.Function;
using System;
using Utility;

namespace Login
{
    public class Login : MonoBehaviour
    {

        [SerializeField]
        private InputField idField;
        [SerializeField]
        private InputField pwField;

        private void Awake()
        {
            var parsingData = Util.Parser(CommonData.StringData.INIT_FILE_NAME);

            foreach (var item in parsingData)
            {
                switch (item.Key)
                {
                    case StringData.PAHT_FOR_ATTRACTION_CONTROL_PROGRAM:
                        SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM = item.Value;
                        break;

                    case StringData.DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE:
                        SettingData.DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE = item.Value;
                        break;

                    case StringData.DELAY_TIME_FOR_CHECKING_ABOUT_LIVING_DEVICE:
                        SettingData.DELAY_TIME_FOR_DEVICE_LIVING = Int32.Parse(item.Value);
                        break;

                    case StringData.MACHINE_UID:
                        UserData.MACHINE_UID = item.Value;
                        break;
                    case StringData.PORT_NUMBER:
                        UserData.PortNumber = item.Value;
                        break;
                }
            }
        }

        public void OnClickedLoginButton()
        {
            string id = idField.text;
            string pw = pwField.text;


            if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(pw))
            {
                ShowNotification("아이디와 비밀번호를 확인해주세요");
            }
            else
            {
                var data = new Dictionary<string, string>();
                data.Add("user_id", idField.text);
                data.Add("user_password", pwField.text);

                ServerAPI.Instance.Login(data, (WWW www) =>
                {
                    var temp = JsonUtility.FromJson<LoginInfoList>(www.text);

                    if (temp.result)
                    {

                        Log.i("Login Success");
                        
                        UserData.UserName = temp.name;
                        UserData.MILEAGE = temp.mileage;

                        var lists = temp.lists;
                        foreach (var item in lists)
                        {

                            if (item.machineUID.Equals(UserData.MACHINE_UID))
                            {
                                UserData.OWNER_UID = item.ownerUID;
                                break;
                            }
                        }

                        CustomController.Instance.changedLoadScene(CustomController.SceneName.MENU_VIEW);
                    }
                    else
                    {
                        ShowNotification(temp.resultMsg);
                        Log.e("Login Fail");
                    }
                }, URL.DEVELOP_LOGIN_URL);
            }
        }

        [SerializeField]
        private Text idText;
        [SerializeField]
        private Text pwText;

        [SerializeField]
        private Text noticationText;

        [SerializeField]
        private GameObject noticationPanel;




        private void ShowNotification(string text)
        {

            noticationPanel.SetActive(true);
            noticationText.text = text;

            noticationText.color = new Color(1, 0.32f, 0.32f);
            idText.color = new Color(0, 0, 0);
            pwText.color = new Color(0, 0, 0);

        }

    }
}