using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {

    // 움직일 조각
    public GameObject[] parts = null;
    // EbmDataParaser Scripts
    private EbmDataParser ebmData = null;

	void Start () {
        ebmData = GameObject.Find("DataParser").GetComponent<EbmDataParser>();
    }

    void FixedUpdate()
    {
        if(ebmData.QuatData.Length > 0)
        {
            Rotation(parts[0], 0);
            //Rotation(parts[1],1);

            //Rotation(parts[2],2);
            //Rotation(parts[3], 3);

            // Sqrt = 루트, Pow = (a , a의 몇승)

            //float test1 = Mathf.Sqrt(Mathf.Pow(ebmData.QuatData[0].x - ebmData.QuatData[1].x, 2))
            //    + Mathf.Sqrt(Mathf.Pow(ebmData.QuatData[0].y - ebmData.QuatData[1].y, 2))
            //    + Mathf.Sqrt(Mathf.Pow(ebmData.QuatData[0].z - ebmData.QuatData[1].z, 2))
            //    + Mathf.Sqrt(Mathf.Pow(ebmData.QuatData[0].w - ebmData.QuatData[1].w, 2));

            //print(test1);

            //  float test1 = Mathf.Sqrt(Mathf.Pow(parts[2].transform.position.x - parts[0].transform.position.x, 2))
            //+ Mathf.Sqrt(Mathf.Pow(parts[2].transform.position.y - parts[0].transform.position.y, 2))
            //+ Mathf.Sqrt(Mathf.Pow(parts[2].transform.position.z - parts[0].transform.position.z, 2));

            //  print(test1);
        }
    }

    // Obj 회전
    void Rotation(GameObject obj, int id)
    {
        Quaternion dataRotation = ebmData.QuatData[id];

        obj.transform.rotation = dataRotation;
        //obj.transform.position = ebmData.DistData[id];
    }
}
