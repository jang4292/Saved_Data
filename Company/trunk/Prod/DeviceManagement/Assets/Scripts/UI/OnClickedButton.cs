using UnityEngine;
using CommonData.Data;
using Utility;
using System.Collections.Generic;
using Utility.Function;
using System;
using UnityEngine.UI;

namespace Menu
{
    public class OnClickedButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject Panel;
        [SerializeField]
        private Text TextBox;
        [SerializeField]
        private GameObject ButtonOK;


        public void OnClickedStartButton()
        {
            Log.i("OnClickedStartButton");
            if (SettingData.IsActivedStartButton)
            {
                Log.d("OnClickedStartButton");
                CustomController.Instance.changedLoadScene(CustomController.SceneName.MAIN_VIEW);
            }
            else
            {
                Log.e("It's not set, To need going Setting");
                ShowPopup("셋팅을 먼저 해주세요");
            }
        }
        public void OnClickedSettingButton()
        {
            Log.i("OnClickedSettingButton");
            CustomController.Instance.changedLoadScene(CustomController.SceneName.SETTING);
        }

        public void IndexUp()
        {
            Log.i("IndexUp");
            CustomController.Instance.indexUp();
        }
        public void IndexDown()
        {
            Log.i("IndexDown");
            CustomController.Instance.indexDown();
        }

        public void OnClickedDownloadButton()
        {
            if (!TextControl.isShop)
            {
                download();

                /*
                ShowPopup(false, "다운로드중");
                var item = SettingData.VideoDataList.lists[CustomController.Instance.Index];
                string savePathVideoName = item.video_name;
                string thumbnail = item.video_thumbnail;

                //Debug.Log("savePathVideoName :: " + savePathVideoName);
                //Debug.Log("thumbnail :: " + thumbnail);

                var data = new Dictionary<string, string>();
                data.Add(thumbnail + "normal.png", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-normal.png");
                data.Add(thumbnail + "hard.png", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-hard.png");
                data.Add(thumbnail + "normal.bmp", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-normal.bmp");
                data.Add(thumbnail + "hard.bmp", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-hard.bmp");
                data.Add(item.motion_name, SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"sim\" + savePathVideoName + ".sim");
                data.Add(item.video_path + "wmv", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"mov\" + savePathVideoName + ".wmv");


                int count = 0;
                bool isChekced = true;
                foreach (KeyValuePair<string, string> post_arg in data)
                {
                    count++;
                    var form = new Download.DownloadForm(post_arg.Key, post_arg.Value);
                    Download.Instance.StartDownload(form, (bool isSaved) =>
                    {
                        count--;

                        if (!isSaved)
                        {
                            isChekced = false;
                        }
                        if (count < 0)
                        {
                            if (isChekced)
                            {
                                ShowPopup("다운로드 완료");
                            }
                            else
                            {
                                ShowPopup("다운로드 실패");
                            }
                        }
                    });
                }
                */

            }
            else
            {
                ShowPopup(false,"구매중");
                ServerAPI.Instance.Buy((WWW www) =>
                {

                    Debug.Log("www.text :: " + www.text);

                    var temp = JsonUtility.FromJson<ResultBuyData>(www.text);

                    UserData.MILEAGE = temp.mileage;
                    CustomController.Instance.RefreshMileage();

                    if (temp.result)
                    {
                        download();
                        /*
                        ShowPopup(false, "다운로드중");
                        var item = SettingData.VideoDataList.lists[CustomController.Instance.Index];
                        string savePathVideoName = item.video_name;
                        string thumbnail = item.video_thumbnail;

                        var data = new Dictionary<string, string>();
                        data.Add(thumbnail + "normal.png", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-normal.png");
                        data.Add(thumbnail + "hard.png", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-hard.png");
                        data.Add(thumbnail + "normal.bmp", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-normal.bmp");
                        data.Add(thumbnail + "hard.bmp", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-hard.bmp");
                        data.Add(item.motion_name, SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"sim\" + savePathVideoName + ".sim");
                        data.Add(item.video_path + "wmv", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"mov\" + savePathVideoName + ".wmv");







                        int count = 0;
                        bool isChekced = true;
                        foreach (KeyValuePair<string, string> post_arg in data)
                        {
                            count++;
                            var form = new Download.DownloadForm(post_arg.Key, post_arg.Value);
                            Download.Instance.StartDownload(form, (bool isSaved) =>
                            {

                                count--;

                                if (!isSaved)
                                {
                                    isChekced = false;
                                }
                                if (count == 0)
                                {
                                    if (isChekced)
                                    {
                                        ShowPopup("구매 및 다운로드 완료");
                                    }
                                    else
                                    {
                                        ShowPopup("다운로드 실패");
                                    }
                                }
                            });
                        }*/
                    }
                    else
                    {
                        ShowPopup(temp.resultMsg);
                        Log.e("[구매실패] :: " + temp.resultMsg);
                    }
                    CustomController.Instance.NewVideoList();
                });
            }
        }

        private void download()//Dictionary<string, string> data)
        {

            ShowPopup(false, "다운로드중");

            var item = SettingData.VideoDataList.lists[CustomController.Instance.Index];
            string savePathVideoName = item.video_name;
            string thumbnail = item.video_thumbnail;

            //Debug.Log("savePathVideoName :: " + savePathVideoName);
            //Debug.Log("thumbnail :: " + thumbnail);

            var data = new Dictionary<string, string>();
            data.Add(thumbnail + "normal.png", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-normal.png");
            data.Add(thumbnail + "hard.png", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-hard.png");
            data.Add(thumbnail + "normal.bmp", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-normal.bmp");
            data.Add(thumbnail + "hard.bmp", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + savePathVideoName + "-hard.bmp");
            data.Add(item.motion_name, SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"sim\" + savePathVideoName + ".sim");
            data.Add(item.video_path + "wmv", SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"mov\" + savePathVideoName + ".wmv");

            int count = 0;
            bool isChekced = true;
            
            foreach (KeyValuePair<string, string> post_arg in data)
            {
                count++;
                var form = new Download.DownloadForm(post_arg.Key, post_arg.Value);

                bool isVideo = post_arg.Key.Equals(item.video_path + "wmv");
                Download.Instance.StartDownload(form, (bool isSaved) =>
                {

                    count--;

                    //Debug.Log("count :: " + count);
                    if (!isSaved)
                    {
                        isChekced = false;
                    }
                    if (count == 0)
                    {
                        if (isChekced)
                        {
                            if (!TextControl.isShop)
                            {
                                ShowPopup("다운로드 완료");
                            } else
                            {
                                ShowPopup("구매 및 다운로드 완료");

                            }
                        }
                        else
                        {
                            ShowPopup("다운로드 실패");
                        }
                    }
                }, (int i) =>
                {
                    if(i < 99)
                    {
                        
                        if(isVideo) {
                            //Debug.Log("Button i :: " + i);
                            ShowPopup(false, "다운로드중 " + i + "%");
                        }
                    }
                });
            }
        }

        public void OnClickedBackButton()
        {
            Log.i("OnClickedBackButton");
            try
            {
                var hooking = GameObject.Find("Manager").GetComponent<CustomHooking>();
                if (hooking)
                {
                    hooking.StopHooking();
                }
            }
            catch (Exception e)
            {
                Debug.Log("e.StackTrace :: " + e.StackTrace);
            }
            CustomController.Instance.changedLoadScene(CustomController.SceneName.MENU_VIEW);
        }

        public void OnClickedGoingToShop()
        {
            Log.i("OnClickedGoingToShop [TextControl.isShop] : " + TextControl.isShop);
            if (!TextControl.isShop)
            {
                CustomController.Instance.NewVideoList();
                TextControl.isShop = true;
            }
            else
            {
                CustomController.Instance.BoughtList();
                TextControl.isShop = false;
            }
        }

        private void ShowPopup(string text)
        {
            ShowPopup(true, text);
        }

        private void ShowPopup(bool isShow, string text)
        {
            Log.d("ShowPopup [Text] : " + text);
            Panel.SetActive(true);
            TextBox.text = text;
            if(ButtonOK) { 
                ButtonOK.SetActive(isShow);
            }
        }

        private void HidePopup()
        {
            Panel.SetActive(false);

        }


        public void OnClickedPopupOKButton()
        {
            HidePopup();
        }
    }
}