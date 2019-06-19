using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XyloGameManager : AbstractGameController
{
    private string gameName = "xylo";
    bool isGameOver = false;

    //시간, 점수
    public Text durationText;
    public Text pointText;
    private int sumOfPlayTime = 0;
    public int points = 0;

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

    override
    public int getTime()
    {
        return sumOfPlayTime;
    }

    override
    public int getPoints()
    {
        return points;
    }

    override
    public string getGameName()
    {
        return gameName;
    }
}
