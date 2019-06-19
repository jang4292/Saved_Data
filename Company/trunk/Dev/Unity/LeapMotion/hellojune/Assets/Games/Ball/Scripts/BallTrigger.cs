using UnityEngine;
using System.Collections;

public class BallTrigger : MonoBehaviour {

    public GameObject gameController;

    private GameObject touchedBall;
    private const int BALL_LAYER = 8;
    private const int CANNOT_TOUCH_BALL_LAYER = 0;
	
	void Update ()
    {
        if (touchedBall != null)
            touchedBall.transform.position = this.transform.position;
	}

    void OnTriggerEnter(Collider other)
    {
        if(touchedBall == null)
        {
            if (other.gameObject.layer == BALL_LAYER)
            {
                touchedBall = other.gameObject;
            }
        }
        else
        {
            if(other.gameObject.tag == "DropBoxTrigger")
            {
                touchedBall.layer = CANNOT_TOUCH_BALL_LAYER;
                gameController.GetComponent<GameController>().CountEnteredBallToLeapMotionHandTrigger(touchedBall);
                touchedBall = null;
            }
        }
    }
    
}
