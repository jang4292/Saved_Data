using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CommonData;
using CommonData.Data;
using Utility;
using System;

public class SocketNetworkingServer : NetworkManager
{

    public delegate void DeviceInfoChagned(int count);
    public static DeviceInfoChagned CustomChangedCount;


    public delegate void LivingDevice();
    public static LivingDevice SendMessageForLivingDevice;

    public override void OnStartServer()
    {
        Log.i("[SocketNetworkingServer] OnStartServer");
        Debug.Log("OnStartServer( )");
    }

    public override void OnStartClient(NetworkClient client)
    {
        Log.i("[SocketNetworkingServer] OnStartClient : " + client.ToString());
        Debug.Log("OnStartClient( )");
    }

    public override void OnStopClient()
    {
        Log.i("[SocketNetworkingServer] OnStopClient");
        Debug.Log("OnStopClient( )");
    }

    void Start()
    {
        Log.i("[SocketNetworkingServer] Start");
        SetupServer();
    }
    public void OnDestroy()
    {
        Log.i("[SocketNetworkingServer] OnDestroy");
        StopServer();
        OnStopServer();
    }

    IEnumerator CheckingConnected()
    {
        while (true)
        {
            Log.i("[SocketNetworkingServer] CheckingConnected");
            SendMessageForLiving();
            Log.d("SettingData.DELAY_TIME_FOR_DEVICE_LIVING : " + SettingData.DELAY_TIME_FOR_DEVICE_LIVING);
            yield return new WaitForSeconds(SettingData.DELAY_TIME_FOR_DEVICE_LIVING / 1000);
        }
    }

    public void SetupServer()
    {
        try { 
            StartServer();
            int portNumber;
            Log.d("[UserData.PortNumber] : " + UserData.PortNumber);
            if (!int.TryParse(UserData.PortNumber, out portNumber))
            {
                portNumber = 4444;
            }
            Log.d("[portnumber] : " + portNumber);
            NetworkServer.Listen(portNumber);
            NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
            NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisConnected);
            NetworkServer.RegisterHandler(MyMsgType.OnMonitoring, OnMonitoringDeviceConnect);
        } catch(Exception e)
        {
            Log.e("[SetupServer] :" + e.StackTrace);
        }
    }

    public void OnMonitoringDeviceConnect(NetworkMessage netMsg)
    {
        try { 
            var msg = netMsg.ReadMessage<MyMessage>();
            Log.i("[Monitoring Device] :: " + msg.deviceNumber);
            CustomChangedCount(msg.deviceNumber);
        } catch (Exception e)
        {
            Log.e("[OnMonitoringDeviceConnect] : " + e.StackTrace);
        }
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        Log.i("[OnConnected]");
        StartCoroutine(CheckingConnected());
    }

    
    public void OnDisConnected(NetworkMessage netMsg)
    {
        Log.d("OnDisConnected netMsg.ToString :: " + netMsg.ToString());
    }
    

    private const int MAX_COUNT = 3;
    public void SendMessageForLiving()
    {
        Log.i("[SocketNetworkingServer] SendMessageForLiving");
        var msg = new MyMessage();
        short typeNumber = MyMsgType.OnMonitoring;

        NetworkServer.SendToAll(typeNumber, msg);
        SendMessageForLivingDevice();
    }
}
