using UnityEngine;
using System;
using UnityEngine.UI;
using Utility;

public class NetworkHelper : MonoBehaviour
{
    private const int MAX_RESPONSE_COUNT = 5;

    public Sprite PlayOn;
    public Sprite PlayOff;

    [SerializeField]
    private DeviceInfo[] deviceInfo;

    [Serializable]
    private class DeviceInfo
    {
        public Image item;
        public bool isConnected;
        public int sendCount = 0;
    }

    void Start()
    {
        Log.i("NetworkHelper Start()");
        SocketNetworkingServer.CustomChangedCount += changed;
        SocketNetworkingServer.SendMessageForLivingDevice += SendMessageForLivingDevice;
    }

    void OnGUI()
    {
        foreach (DeviceInfo info in deviceInfo)
        {

            
            if (!info.item)
            {
                Log.i("[info.item] doesn't have any object");
            }

            if (info.isConnected)
            {
                Log.i("[info.isConnected] PlayOn");
                info.item.sprite = PlayOn;
            }
            else
            {
                info.item.sprite = PlayOff;
            }
        }
    }

    public void changed(int i)
    {
        Log.d("[changed] : " + i);
        deviceInfo[i].sendCount = 0;
        deviceInfo[i].isConnected = true;
    }

    public void SendMessageForLivingDevice()
    {
        Log.i("SendMessageForLivingDevice()");
        foreach (DeviceInfo info in deviceInfo)
        {
            info.sendCount++;
            if (info.sendCount > MAX_RESPONSE_COUNT)
            {
                info.isConnected = false;
            }
        }
    }
}
