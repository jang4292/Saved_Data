using UnityEngine;
using System.Collections;

public class HammerManager : MonoBehaviour
{
    public GameObject hammer;
    public MoleGameController script;

	void Update ()
    {
        hammer.transform.position = this.gameObject.transform.position;
        hammer.transform.rotation = this.gameObject.transform.rotation;
        hammer.transform.Rotate(180.0f, 0.0f, 90.0f);
	}

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.transform.localPosition.y > 0.5f)
        {
            script.points += 1;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
