using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionManager : MonoBehaviour
{

    private enum GrabState
    {
        Idle,
        Hold
    }

    private GrabState grabState;

    private GameObject handMouse;
    private HandMouseSource mouse;

    private Vector3 handBasicPos;
    private Vector3 handPos;
    private float depthSize = 30;

    private LeapProvider provider;

    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    void Update()
    {
        Frame frame = provider.CurrentFrame;
        if (frame.Hands.Count > 0)
        {
            foreach (Hand hand in frame.Hands)
            {
                if (handMouse == null)
                {
                    handMouse = GameObject.Find("HandMouse");
                    mouse = handMouse.GetComponent<HandMouseSource>();
                }

                /// 주먹을 쥐거나 피거나 할 때 움직임을 잠시 막아 흔들림을 보정
                if (grabState == GrabState.Idle && (hand.GrabAngle > 1f && hand.GrabAngle < 2.5f))
                {
                    grabState = GrabState.Hold;
                }
                else if(grabState == GrabState.Hold && (hand.GrabAngle <= 1f || hand.GrabAngle >= 2.5f))
                {
                    grabState = GrabState.Idle;
                }

                if(grabState == GrabState.Idle)
                {
                    if (handBasicPos == Vector3.zero)
                    {
                        handBasicPos = hand.PalmPosition.ToVector3() * depthSize;
                    }

                    handPos.x = (hand.PalmPosition.x * depthSize) - handBasicPos.x;
                    handPos.y = (hand.PalmPosition.y * depthSize) - handBasicPos.y;
                    handPos.z = 0f;

                    handMouse.transform.position = handPos;
                }

                if (hand.GrabAngle > 3f)
                {
                    mouse.OnStatusChanged(HandMouseSource.HandMouseStatus.CLICK);
                }
                else if (hand.GrabAngle < 1f)
                {
                    mouse.OnStatusChanged(HandMouseSource.HandMouseStatus.IDLE);
                }
            }
        }
    }
}
