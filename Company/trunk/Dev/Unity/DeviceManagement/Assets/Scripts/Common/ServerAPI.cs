using UnityEngine;
using CommonData;
using CommonData.Data;
using Utility.Function;

public class ServerAPI : CommonServerAPI
{
    private const string ClassName = "ServerAPI";

    private static ServerAPI current = null; // 이 클래스의 싱글톤 객체 
    private static GameObject container = null; // 객체를 생성하기 위한 GameObject

    public static ServerAPI Instance// 싱글톤 객체 만들기
    {
        get
        {
            if (current == null)
            {
                container = new GameObject();
                container.name = ClassName;
                current = container.AddComponent(typeof(ServerAPI)) as ServerAPI;
            }
            return current as ServerAPI;
        }
    }

    public void SendStartMessageToServer(CustomWWW.RequestHandler requestHandler)
    {

        WWWForm form = new WWWForm();

        var item = SettingData.VideoDataList.lists[CustomController.Instance.Index / 2];

        form.AddField("ouid", UserData.OWNER_UID);
        form.AddField("cuid", item.uid);
        form.AddField("muid", UserData.MACHINE_UID);
        form.AddField("people", SettingData.MEMBER_COUNT);
        form.AddField("price", item.playPrice);
        form.AddField("hard", (CustomController.Instance.Index % 2 == 0) ? "1" : "0");

        StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.JSON, new PostData(URL.DEVELOP_SEND_STRAT_MESSAGE_URL, form),
                (string str) =>
                {

                }, (WWW www) =>
                {
                    var temp = JsonUtility.FromJson<StartLogResult>(www.text);
                    tempLogIdx = temp.resultMsg;
                    requestHandler(www);
                }));
    }

    private string tempLogIdx;
    public void SendEndMessageToServer(CustomWWW.RequestHandler requestHandler)
    {
        WWWForm form = new WWWForm();
        form.AddField("logidx", tempLogIdx);
        form.AddField("status", 0);
        form.AddField("mileage", UserData.PriceOfTax);
        form.AddField("ouid", UserData.OWNER_UID);

        StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.JSON, new PostData(URL.DEVELOP_SEND_END_MESSAGE_URL, form),
                        (string str) =>
                        {
                            Debug.Log("error str :: " + str);
                        }, (WWW www) =>
                        {
                            requestHandler(www);
                        }));

    }

    public void BoughtList(CustomWWW.RequestHandler requestHandler)
    {

        var form = new WWWForm();
        form.AddField("ouid", UserData.OWNER_UID);
        form.AddField("muid", UserData.MACHINE_UID);

        StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.JSON, new PostData(URL.DEVELOP_BUY_VIDEO_LIST_URL, form),
            (string str) =>
            {
                Debug.Log("str :: " + str);
            },
            (WWW www) =>
            {
                requestHandler(www);
            }));
    }

    public void NewVideoList(CustomWWW.RequestHandler requestHandler)
    {

        var form = new WWWForm();
        form.AddField("ouid", UserData.OWNER_UID);
        form.AddField("muid", UserData.MACHINE_UID);

        StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.JSON, new PostData(URL.DEVELOP_NEW_VIDEO_LIST_URL, form),
            (string str) =>
            {
                Debug.Log("str :: " + str);
            },
            (WWW www) =>
            {
                requestHandler(www);
            }));
    }

    public void Buy(CustomWWW.RequestHandler requestHandler)
    {
        var item = SettingData.VideoDataList.lists[CustomController.Instance.Index];

        var form = new WWWForm();
        form.AddField("ouid", UserData.OWNER_UID);
        form.AddField("cuid", item.uid);
        form.AddField("muid", UserData.MACHINE_UID);

        StartCoroutine(CustomWWW.LoadData(CustomWWW.Type.JSON, new PostData(URL.DEVELOP_BUY_URL, form),
            (string str) =>
            {
                Debug.Log("str :: " + str);
            },
            (WWW www) =>
            {
                requestHandler(www);

            }));
    }
}
