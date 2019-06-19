using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
//using UnityEditor;


public class EbmDataParser : MonoBehaviour {

    public Quaternion[] QuatData;
    public Vector3[] DistData;

    private int id = 0;
    public int ID  { get{ return id; } }

	private Quaternion quat;
	private Vector3    euler;
	private Vector3    dist;

    // 쓰레드 변수 생성
	private Thread thread1;
    // 시리얼포트 변수 생성 및 객체를 생성
    public SerialPort EbmPort = new SerialPort("COM2", 921600, Parity.None, 8, StopBits.One);

    void Start () {

        QuatData = new Quaternion[4];
        DistData = new Vector3[4];

        // SerialPort 열기
        EbmPort.Open();
        // 읽기 작업을 마쳐야 하는 제한 시간(밀리초)을 가져오거나 설정합니다.
        EbmPort.ReadTimeout = 3000;
		
        // 쓰레드 객체 생성
		thread1 = new Thread(SerialDataProc);
        // 쓰레드 시작
		thread1.Start();
	}

    // 앱이 종료 될시 호출 되는 함수
	void OnApplicationQuit()
	{
        // 시리얼 포트 닫기
		EbmPort.Close();
        // 쓰레드 중단
		thread1.Abort();
	}
	
    // 쓰레드 함수
	void SerialDataProc() {
		
		string sdata=""; 
		string[] item;
		int item_counter=0;

        while(true)
        {
            // 아무 일도 하지 않고 주어진 시간이 경과되기만을 기다리는 메소드(static method)
		    Thread.Sleep(1);  // 1ms
		
            // 시리얼 포트가 열려 있지 않으면 while문 break;
		    if(EbmPort.IsOpen != true) break;

            // sdata 스트링 변수에 입력 버퍼에서 NewLine 값까지 읽습니다.
            // 예외발생시 continue; 다음으로 넘김
            try {  sdata = EbmPort.ReadLine(); }
				catch{	continue;	}

            // 배열에 있는 문자에 따라 문자열을 부분 문자열로 분할합니다.
            item = sdata.Split(',');

            // item_counter = 0 으로 초기화
            item_counter = 0;

            // 현재 인스턴스의 지정된 문자열이 지정된 다른 문자열로 모두 바뀌는 새 문자열을 반환합니다.
            item[0] = item[0].Replace("-", "");

            id = int.Parse(item[0].Substring(3));

            if (item.Length >= item_counter + 3)   // X,Y,Z,W
            {
                switch (id)
                {
                    case 0:
                        SetQuatData(0, item, item_counter);
                        break;
                    case 1:
                        SetQuatData(1, item, item_counter);
                        break;
                    case 2:
                        SetQuatData(2, item, item_counter);
                        break;
                    case 3:
                        SetQuatData(3, item, item_counter);
                        break;
                }
            }

            //// item길이가 item_counter(3) + 3 보다 크거나 같으면
            //if (item.Length >= item_counter + 3)   // distance
            //{
            //    switch (id)
            //    {
            //        case 0:
            //            SetDistData(0, item, item_counter);
            //            break;
            //        case 1:
            //            SetDistData(1, item, item_counter);
            //            break;
            //        case 2:
            //            SetDistData(2, item, item_counter);
            //            break;
            //        case 3:
            //            SetDistData(3, item, item_counter);
            //            break;
            //    }
            //}
        }
	}

    // 회전값
    void SetQuatData(int number, string[] item, int item_counter)
    {
        QuatData[number].w = float.Parse(item[++item_counter]);
        QuatData[number].z = float.Parse(item[++item_counter]);
        QuatData[number].y = float.Parse(item[++item_counter]);
        QuatData[number].x = float.Parse(item[++item_counter]);

        //euler.x = float.Parse(item[++item_counter]);
        //euler.y = float.Parse(item[++item_counter]);
        //euler.z = float.Parse(item[++item_counter]);
        //QuatData[number].eulerAngles = euler;
        //DistData[number] = euler;
    }

    //// 포지션값
    //void SetDistData(int number, string[] item, int item_counter)
    //{
    //    DistData[number].z = float.Parse(item[++item_counter]) * 10;
    //    DistData[number].x = float.Parse(item[++item_counter]) * 10;
    //    DistData[number].y = float.Parse(item[++item_counter]) * 10;
    //}
}



