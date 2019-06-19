using UnityEngine;
using UnityEngine.UI;
using Leap;
using System;

public class MotionSpeedTesting : MonoBehaviour
{

    private int handCount;
    private int previousHandCount;
    private Controller mController;
    private Text mText;

    void Awake()
    {
        mController = new Controller();
        mController.StartConnection();

        mText = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        handCount = mController.Frame().Hands.Count;

        if (handCount != previousHandCount)
        {
            if (handCount > previousHandCount)
            {
                float framePeriod = mController.Frame(0).Timestamp - mController.Frame(1).Timestamp;

                // Timestamp(마이크로세컨드)가 100만분의 1초라서 100만을 나눠야지만 원하는 초를 가져올 수 있다.
                SetAwarenessSpeedText(framePeriod / 1000000);
            }

            previousHandCount = handCount;
        }
    }

    void OnDisable()
    {
        if (mController != null)
        {
            if (mController.IsConnected)
            {
                mController.ClearPolicy(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
            }
            mController.StopConnection();
            mController = null;
        }
    }

    // 속도측정 텍스트 변경
    private void SetAwarenessSpeedText(float second)
    {
        mText.text = "인식 속도 측정 : " + second + "초";
        Debug.Log(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") +" - "+ mText.text);
    }
}
