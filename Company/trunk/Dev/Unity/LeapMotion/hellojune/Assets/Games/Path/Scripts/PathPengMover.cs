using UnityEngine;
using System.Collections;

public class PathPengMover : MonoBehaviour
{
    public string direction;
    public PathGameManager script;

    void FixedUpdate()
    {
        script.direction = direction;
    }

    void OnDestroy()
    {

    }

}
