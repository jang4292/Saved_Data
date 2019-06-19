using UnityEngine;
using System.IO.Ports;
using System.Threading;

namespace EBMotion
{
    public class EbmDataParser : MonoBehaviour
    {
        public Vector3[] correction = new Vector3[30];
        public Quaternion[] QuatData = new Quaternion[30];
        public string[] sdata_preview = new string[5];
        public float OffsetAdjust = 0;

        private Quaternion quat;
        private int id;
        public int channel = 0;

        private Thread thread1;

        public SerialPort EbmPort = null;

        private int portNumber = -1;

        void Start()
        {
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
                    //print("IOException : " + e.Message);
                }
            }
        }

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
                Thread.Sleep(1);  

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
