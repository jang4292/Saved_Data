using UnityEngine;
using System.Collections;

public class BallLayerManager : MonoBehaviour {

    private const int BALL_LAYER = 8;
    private const int CANNOT_TOUCH_BALL_LAYER = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CANNOT_TOUCH_BALL_LAYER)
            other.gameObject.layer = BALL_LAYER;
    }
}
