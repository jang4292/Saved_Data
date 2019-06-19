using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
//using UnityEditor;

namespace EBMotion
{
    public class EbmDataParser : MonoBehaviour
    {
        /*
        public Quaternion[] QuatData = new Quaternion[30];
        public float[] SensorBat = new float[30];
        public float OffsetAdjust = 0;
        public string[] sdata_preview = new string[5];
        public bool posCal = false;
        public bool resetCal = false;
        */

        public Vector3[] correction = new Vector3[30];
        public Quaternion[] QuatData = new Quaternion[30];
        public string[] sdata_preview = new string[5];
        public float OffsetAdjust = 0;

        Quaternion quat;
        int id;
        public int channel = 0;

        Thread thread1;

        public SerialPort EbmPort = null;

        private int portNumber = -1;

        void Awake()
        {
            //스크린 비율 16:9
            float _height = Screen.width / 16f * 9;

            Screen.SetResolution(Screen.width, (_height % 1f > 0) ? Mathf.RoundToInt(_height) : (int)_height, true);
        }

        void Start()
        {
            /*
            PortNumberCheck();

            posCal = true;

            EbmPort.ReadTimeout = 3000;
            thread1 = new Thread(SerialDataProc);
            //		thread1.IsBackground = true;
            thread1.Start();
            */
            PortNumberCheck();

            ResetCal();
            PosCal();

            EbmPort.ReadTimeout = 3000;
            thread1 = new Thread(SerialDataProc);
            thread1.Start();
        }

        void PortNumberCheck()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                EbmPort = new SerialPort("\\\\.\\" + portName, 921600, Parity.None, 8, StopBits.One);

                try
                {
                    EbmPort.Open();
                    break;
                }
                catch (System.IO.IOException e)
                {
                    //print("실패이유 : " + e.Message);
                }
            }
        }

        /*
        void OnApplicationQuit()
        {
            if(EbmPort.IsOpen) ResetCal();
            EbmPort.Close();
            thread1.Abort();
        }
        */

        void OnDestroy()
        {
            EbmPort.Close();
            thread1.Abort();
        }

        void SerialDataProc()
        {
            string sdata = "";
            string[] ch_id;
            string[] item;

            while (true)
            {
                Thread.Sleep(1);  // 1ms
                                  //for(int n=0;n<15;n++)
                                  /*
                for (int n = 0; n < 5; n++)
                {
                    if (EbmPort.IsOpen)
                    {
                        try
                        {
                            sdata = EbmPort.ReadLine();

                            sdata_preview[n] = sdata;
                            item = sdata.Split(',');

                            if (item.Length == 6)  // chid,x,y,z,w,batt
                            {
                                ch_id = item[0].Split('-');
                                channel = int.Parse(ch_id[0]);
                                id = int.Parse(ch_id[1]);
                                if (id < 30)
                                {
                                    quat.y = float.Parse(item[1]);
                                    quat.x = float.Parse(item[2]);
                                    quat.z = float.Parse(item[3]);
                                    quat.w = float.Parse(item[4]);
                                    SensorBat[id] = float.Parse(item[5]);
                                }
                                QuatData[id] = quat * Quaternion.Euler(new Vector3(0, OffsetAdjust, 0));
                            }
                        }
                        catch
                        {
                            continue; 
                        }

                        if (resetCal)
                        {
                            ResetCal();
                        }

                        if (posCal)
                        {
                            PosCal();
                        }
                    }
                }
                */


                for (int n = 0; n < sdata_preview.Length; n++)
                {
                    if (EbmPort.IsOpen)
                    {
                        try
                        {
                            sdata = EbmPort.ReadLine();

                            sdata_preview[n] = sdata;
                            item = sdata.Split(',');

                            if (item.Length >= 5)  // chid,x,y,z,w,batt
                            {
                                ch_id = item[0].Split('-');
                                channel = int.Parse(ch_id[0]);
                                id = int.Parse(ch_id[1]);
                                if (id < 30)
                                {
                                    quat.y = float.Parse(item[1]);
                                    quat.x = float.Parse(item[2]);
                                    quat.z = float.Parse(item[3]);
                                    quat.w = float.Parse(item[4]);

                                    correction[id].x = float.Parse(item[5]);
                                    correction[id].y = float.Parse(item[6]);
                                    correction[id].z = float.Parse(item[7]);
                                    //if (correction[id] == Vector3.zero)
                                    //{
                                    //    correction[id].x = float.Parse(item[5]);
                                    //    correction[id].y = float.Parse(item[6]);
                                    //    correction[id].z = float.Parse(item[7]);
                                    //}
                                }

                                QuatData[id] = quat * Quaternion.Euler(new Vector3(0, OffsetAdjust, 0));
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        /*
        void ResetCal()
        {
            EbmPort.Write("<??cmco>");
            resetCal = false;
        }

        void PosCal()
        {
            EbmPort.Write("<??cmo>");
            posCal = false;
        }

        void OnGUI()
        {

        }
        */

        void ResetCal()
        {
            EbmPort.Write("<??cmco>");
        }

        void PosCal()
        {
            EbmPort.Write("<??cmo>");
        }
    }

}
