using UnityEngine;
using System.Collections;
//using UnityEditor;

namespace EBMotion
{
    public class Settings : MonoBehaviour
    {
        EbmDataParser ebmData;
        //CameraControl camControl;
        public int camFollowing = 0;

        string port_status = "";
        string strPort = "2";
        int baudrate = 921600;
        bool viewSensorPos = false;

        public GUIStyle e2boxLogo, sensorPosition;



        void Start()
        {
            ebmData = GameObject.Find("EbmDataParser").GetComponent("EbmDataParser") as EbmDataParser;
        }

        void OnGUI()
        {


            GUI.Label(new Rect(Screen.width - 100 - 10, Screen.height - 42 - 10, 100, 42), "", e2boxLogo);


            ///////////////////////////////////////////////////////////////
            ///////////// Receiver ////////////////////////////////////////

            GroupBoxStart(290, 330, "Settings");
            GUI.Label(new Rect(15, 37, 90, 30), "PORT");
            GUI.Label(new Rect(160, 37, 150, 30), port_status);

            if (ebmData.EbmPort.IsOpen == false)
            {
                if (GUI.Button(new Rect(60, 35, 90, 25), "Connect"))
                {
                    try
                    {
                        ebmData.EbmPort.PortName = "COM" + strPort;
                        ebmData.EbmPort.BaudRate = baudrate;
                        ebmData.EbmPort.Open();
                    }
                    catch
                    {
                    }

                    if (ebmData.EbmPort.IsOpen == true)
                    {
                        port_status = "..connected" + " [" + ebmData.EbmPort.PortName + "]";
                    }
                    else
                    {   //EditorUtility.DisplayDialog(ebmData.EbmPort.PortName,"ERROR : Port open", "OK");
                        port_status = "";
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(60, 35, 90, 25), "Disconnect"))
                {
                    ebmData.EbmPort.Close();
                    port_status = "";
                }
            }

            GUI.Label(new Rect(15, 70, 50, 25), "COM : ");
            strPort = GUI.TextField(new Rect(60, 70, 30, 25), strPort);

            GUI.Label(new Rect(100, 70, 100, 25), "BAUDRATE : ");
            baudrate = int.Parse(GUI.TextField(new Rect(185, 70, 80, 25), baudrate.ToString()));

            GUI.Label(new Rect(15, 100, 90, 25), "DATA");
            GUI.TextArea(new Rect(10, 120, 270, 80), string.Format("{0}\n{1}\n{2}\n{3}\n{4}", ebmData.sdata_preview[0], ebmData.sdata_preview[1], ebmData.sdata_preview[2], ebmData.sdata_preview[3], ebmData.sdata_preview[4]));

            ////// Calibration adjust ///////
            if (GUI.Button(new Rect(10, 220, 130, 25), "1.Reset Calibration")) //ebmData.resetCal = true;
            if (GUI.Button(new Rect(150, 220, 130, 25), "2.Pose Calibration")) //ebmData.posCal = true;

            GUI.Label(new Rect(20, 250, 130, 30), "3.Calibration Adjust");
            ebmData.OffsetAdjust = GUI.HorizontalSlider(new Rect(10, 280, 280, 15), ebmData.OffsetAdjust, -180, +180);
            /////////////////////////////////

            GUI.EndGroup();



            ///////////////////////////////////////////////////////////////
            ///////////// Sensor Position /////////////////////////////////
            if (viewSensorPos == true)
            {
                if (GUI.Button(new Rect(Screen.width - 10 - 400, 20, 400, 400), "", sensorPosition)) viewSensorPos = false;
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width - 10 - 120, 20, 120, 25), "Sensor Position")) viewSensorPos = true;
            }
            ///////////// Sensor Position /////////////////////////////////
            ///////////////////////////////////////////////////////////////

        }


        void GroupBoxStart(int width, int height, string title)
        {
            //GUI.BeginGroup (new Rect (Screen.width / 2 - (width/2), Screen.height / 2 - (height/2), width, height));
            GUI.BeginGroup(new Rect(10, 20, width, height));
            GUI.Box(new Rect(0, 0, width, height), title);
        }



    }
}
