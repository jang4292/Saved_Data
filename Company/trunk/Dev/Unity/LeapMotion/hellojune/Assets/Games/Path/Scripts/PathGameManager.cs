using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PathGameManager : AbstractGameController
{
    private string gameName = "pathfinding";
    bool isGameOver = false;

    //시간, 점수
    public Text durationText;
    public Text pointText;
    private int sumOfPlayTime = 0;
    public int points = 0;

    public GameObject pengObj;
    private float pengSpeed = 0.3f;

    public GameObject bearObj;

    public string direction = "forward";
    public GameObject leftHand;
    public GameObject rightHand;

    void Start()
    {
        StartCoroutine(ControlGameMessageUsingGameTime());
    }

    IEnumerator ControlGameMessageUsingGameTime()
    {
        while (!isGameOver)
        {
            durationText.text = sumOfPlayTime + " sec";
            pointText.text = points + " point";

            yield return new WaitForSeconds(1.0f);
            sumOfPlayTime += 1;
        }
    }

    void FixedUpdate ()
    {
        //방향 전환용 양손이 다 없으면 앞으로
        bool isUnactiveLeftAndRightHand = (leftHand.active == false) && (rightHand.active == false);
        if(isUnactiveLeftAndRightHand)
        {
            direction = "forward";
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            direction = "left";
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            direction = "right";
        }

        movePengPosition(direction);
    }

    void movePengPosition(string leftOrRight)
    {
        Vector3 localPosition = pengObj.transform.localPosition;

        if(leftOrRight == "left")
        {
            localPosition = new Vector3(localPosition.x - (0.1f * Time.deltaTime), localPosition.y, localPosition.z);
        }

        if (leftOrRight == "right")
        {
            localPosition = new Vector3(localPosition.x + (0.1f * Time.deltaTime), localPosition.y, localPosition.z);
        }

        localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z + (pengSpeed * Time.deltaTime));

        pengObj.transform.localPosition = localPosition;
    }

    override public int getTime()
    {
        return sumOfPlayTime;
    }

    override public int getPoints()
    {
        return points;
    }

    override public string getGameName()
    {
        return gameName;
    }
}
