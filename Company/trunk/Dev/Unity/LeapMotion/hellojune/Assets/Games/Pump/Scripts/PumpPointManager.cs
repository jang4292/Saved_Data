using UnityEngine;
using System.Collections;

public class PumpPointManager : MonoBehaviour
{
    public PumpGameController script;

    void OnCollisionExit(Collision coll)
    {
        if(coll.gameObject.name.StartsWith("ResizedPump"))
        {
            script.points += 1;
        }
    }
}
