using UnityEngine;
using System.Collections;
using Leap.Unity;

public class XyloStickManager : MonoBehaviour {

    public GameObject stickObj;
    public XyloGameManager script;

    void Update ()
    {
        stickObj.transform.position = this.gameObject.transform.position;
        stickObj.transform.rotation = this.gameObject.transform.rotation;
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "XyloBar")
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            script.points += 1;
        }
    }
}
