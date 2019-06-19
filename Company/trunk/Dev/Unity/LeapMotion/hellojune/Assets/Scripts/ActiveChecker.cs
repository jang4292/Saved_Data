using UnityEngine;
using System.Collections;

public class ActiveChecker : MonoBehaviour
{
    public GameObject hand;
	
	void FixedUpdate()
    {
        if(hand.active == false)
        {
            this.gameObject.transform.position = new Vector3(100.0f, 100.0f, 100.0f);
        }
    }
}
