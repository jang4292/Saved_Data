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

    // ������ ���� ����
	private Thread thread1;
    // �ø�����Ʈ ���� ���� �� ��ü�� ����
    public SerialPort EbmPort = new SerialPort("COM2", 921600, Parity.None, 8, StopBits.One);

    void Start () {

        QuatData = new Quaternion[4];
        DistData = new Vector3[4];

        // SerialPort ����
        EbmPort.Open();
        // �б� �۾��� ���ľ� �ϴ� ���� �ð�(�и���)�� �������ų� �����մϴ�.
        EbmPort.ReadTimeout = 3000;
		
        // ������ ��ü ����
		thread1 = new Thread(SerialDataProc);
        // ������ ����
		thread1.Start();
	}

    // ���� ���� �ɽ� ȣ�� �Ǵ� �Լ�
	void OnApplicationQuit()
	{
        // �ø��� ��Ʈ �ݱ�
		EbmPort.Close();
        // ������ �ߴ�
		thread1.Abort();
	}
	
    // ������ �Լ�
	void SerialDataProc() {
		
		string sdata=""; 
		string[] item;
		int item_counter=0;

        while(true)
        {
            // �ƹ� �ϵ� ���� �ʰ� �־��� �ð��� ����Ǳ⸸�� ��ٸ��� �޼ҵ�(static method)
		    Thread.Sleep(1);  // 1ms
		
            // �ø��� ��Ʈ�� ���� ���� ������ while�� break;
		    if(EbmPort.IsOpen != true) break;

            // sdata ��Ʈ�� ������ �Է� ���ۿ��� NewLine ������ �н��ϴ�.
            // ���ܹ߻��� continue; �������� �ѱ�
            try {  sdata = EbmPort.ReadLine(); }
				catch{	continue;	}

            // �迭�� �ִ� ���ڿ� ���� ���ڿ��� �κ� ���ڿ��� �����մϴ�.
            item = sdata.Split(',');

            // item_counter = 0 ���� �ʱ�ȭ
            item_counter = 0;

            // ���� �ν��Ͻ��� ������ ���ڿ��� ������ �ٸ� ���ڿ��� ��� �ٲ�� �� ���ڿ��� ��ȯ�մϴ�.
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

            //// item���̰� item_counter(3) + 3 ���� ũ�ų� ������
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

    // ȸ����
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

    //// �����ǰ�
    //void SetDistData(int number, string[] item, int item_counter)
    //{
    //    DistData[number].z = float.Parse(item[++item_counter]) * 10;
    //    DistData[number].x = float.Parse(item[++item_counter]) * 10;
    //    DistData[number].y = float.Parse(item[++item_counter]) * 10;
    //}
}



