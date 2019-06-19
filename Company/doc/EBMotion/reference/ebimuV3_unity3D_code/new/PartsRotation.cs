using UnityEngine;
using System.Collections;

public class PartsRotation : MonoBehaviour {
	
	public int ID=0;
	EbmDataParser ebmData;
	
	Quaternion quat;
	// Use this for initialization
	void Start () {
		ebmData = GameObject.Find("EbmDataParser").GetComponent("EbmDataParser") as EbmDataParser;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		quat = ebmData.QuatData[ID];
		transform.rotation = quat;
	}
}
