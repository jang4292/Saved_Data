using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
//using UnityEditor;


public class EbmDataParser : MonoBehaviour {
	
	
	public Quaternion[] QuatData = new Quaternion[30];  
	public float[] SensorBat = new float[30];  
	public float OffsetAdjust = 0;
	public string[] sdata_preview = new string[5];
	public bool posCal=false;
	public bool resetCal=false;
	
	Quaternion quat;
	int id;
	public int channel=0;
	
	Thread thread1;
	public SerialPort EbmPort = new SerialPort("COM1",921600,Parity.None,8,StopBits.One); 	
	
	void Start () {
		EbmPort.ReadTimeout = 3000;
		thread1 = new Thread(SerialDataProc);
//		thread1.IsBackground = true;
		thread1.Start();
		
	}

	void OnApplicationQuit()
	{
		EbmPort.Close();
		thread1.Abort();
	}
	
		  
	void SerialDataProc() {
		
		string sdata=""; 
		string[] ch_id;
		string[] item;
		
		while(true)
		{
			Thread.Sleep(1);  // 1ms
				//for(int n=0;n<15;n++)
		    for(int n=0;n<5;n++)
	 		{
			   if(EbmPort.IsOpen == true) 
			    {
		          try
		          {
						sdata = EbmPort.ReadLine(); 
						
						sdata_preview[n] = sdata;
						item = sdata.Split(','); 
	        			if(item.Length == 6)  // chid,x,y,z,w,batt
	        			{
							ch_id = item[0].Split('-');
			   				channel = int.Parse(ch_id[0]);
			   				id      = int.Parse(ch_id[1]);
							if(id<30)
							{
			   					quat.y = float.Parse(item[1]);
			   					quat.x = float.Parse(item[2]);
			   					quat.z = float.Parse(item[3]);
			   					quat.w = float.Parse(item[4]);
								SensorBat[id] = float.Parse(item[5]);
							}
							    QuatData[id] = quat * Quaternion.Euler(new Vector3(0,OffsetAdjust,0));
							    
					    }
				  }
				  catch
				  {
	     			continue;
				  }
						
	        			
	        			if(posCal==true) 
	        			{ EbmPort.Write("<??cmo>");
	        			  posCal=false;
	        			}
	        			
	        			if(resetCal==true)
	        			{ EbmPort.Write("<??cmco>");
	        			  resetCal=false;
	        			}
       
				}
				
			}
		}
	}

	
	
	void OnGUI () {
		
	}

}



