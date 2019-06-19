using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
//using UnityEditor;


public class EbmDataParser : MonoBehaviour {

  public Quaternion QuatData;
  public Vector3 DistData;
	
	Quaternion quat;
	Vector3    euler;
	Vector3    dist;

	Thread thread1;
	public SerialPort EbmPort = new SerialPort("COM3",115200,Parity.None,8,StopBits.One); 	
	
	
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
		string[] item;
		int item_counter=0;
		
			
    while(true)
    {
		    Thread.Sleep(1);  // 1ms
		
		    if(EbmPort.IsOpen != true) break;
		    					    
		    try{  sdata = EbmPort.ReadLine(); }
				catch{	continue;	}
				
				item = sdata.Split(','); 
				
				item[0] = item[0].Replace("*", "");
					
			  item_counter=0;
			  
		    if (item.Length >= item_counter + 3)  // z,x,y
		      {
		          euler.z = float.Parse(item[item_counter++]);
		          euler.x = float.Parse(item[item_counter++]);
		          euler.y = float.Parse(item[item_counter++]);
		          quat.eulerAngles = euler;
		      }
		
		    QuatData = quat;
		    
		    if (item.Length >= item_counter + 3)   // distance
		    {
		        dist.z = float.Parse(item[item_counter++]) * 10;
		        dist.x = float.Parse(item[item_counter++]) * 10;
		        dist.y = float.Parse(item[item_counter++]) * 10;
		    }
		    
		    DistData = dist;
			
		}
	}


}



