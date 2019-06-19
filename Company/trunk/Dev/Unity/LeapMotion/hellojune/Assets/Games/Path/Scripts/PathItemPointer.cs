using UnityEngine;
using System.Collections;

public class PathItemPointer : MonoBehaviour
{
    public PathGameManager script;

	void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name.StartsWith("Blue"))
        {
            script.points += 2;
        }
        else
        {
            script.points += 5;
        }

        gameObject.GetComponent<AudioSource>().Play();
        Destroy(coll.gameObject);
    }
}
