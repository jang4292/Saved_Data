using CommonData.Data;
using UnityEngine;
using System.Collections;
using Utility.Function;
using System;
using UnityEngine.Networking;

public class CustomHooking : Hooking, Hooking.IHookingInterface
{

    private bool isStartedHooking = false;
    private bool isPlaying = false;

    public MessageBase Left(int key)
    {
        CustomController.Instance.indexDown();
        var msg = new MyMessage();
        msg.number = key;
        msg.title = VideoXMLDataList.lists[CustomController.Instance.Index].title;

        return (MessageBase)msg;
    }

    public MessageBase Right(int key)
    {
        CustomController.Instance.indexUp();
        var msg = new MyMessage();
        msg.number = key;
        msg.title = VideoXMLDataList.lists[CustomController.Instance.Index].title;

        return (MessageBase)msg;
    }
    public MessageBase Return(int key)
    {
        var msg = new MyMessage();
        msg.number = key;
        msg.title = VideoXMLDataList.lists[CustomController.Instance.Index].title;
        msg.delayTimeForStart = SettingData.DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE;

        ServerAPI.Instance.SendStartMessageToServer((WWW www) =>
        {
            var temp = JsonUtility.FromJson<StartLogResult>(www.text);
            UserData.MILEAGE = temp.mileage;
            CustomController.Instance.RefreshMileage();
        }
        );
        StartCoroutine(WatingTimeForSendingEndMessage());
        StopHooking();
        isPlaying = true;
        return (MessageBase)msg;
    }

    public void Start()
    {
        SetInterfaceHooking(this);
    }

    public void StartHooking()
    {
        if (!isStartedHooking && !isPlaying)
        {
            isStartedHooking = true;
            SetHook();
        }
        else
        {
            Debug.Log("SetHooking is already started [isStartedHooking] :" + isStartedHooking + " [isPlaying] : " + isPlaying);
        }

    }

    public void StopHooking()
    {
        UnHook();
        isStartedHooking = false;

    }

    private IEnumerator WatingTimeForSendingEndMessage()
    {
        var t = VideoXMLDataList.lists[CustomController.Instance.Index].playTime;
        yield return new WaitForSeconds(int.Parse(t));
        ServerAPI.Instance.SendEndMessageToServer((WWW www) =>
        {

            try
            {
                var temp = JsonUtility.FromJson<EndLogResult>(www.text);
                UserData.MILEAGE = temp.mileage;
            }
            catch (Exception e)
            {
                Debug.Log("e.StackTrace :: " + e.StackTrace);
            }
            finally
            {
                CustomController.Instance.RefreshMileage();
            }

        });
        isPlaying = false;
        Debug.Log("[Finished] WatingTimeForSendingEndMessage");
    }


}
