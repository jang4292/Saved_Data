using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PumpGameController : AbstractGameController
{
    private string gameName = "pump";
    bool isGameOver = false;

    //시간, 점수
    public Text durationText;
    public Text pointText;
    private int sumOfPlayTime = 0;
    public int points = 0;

    public int pumpkinSize;
    public Transform leapTransform;
    public GameObject targetPumpkinPrefab;
    public Transform localCamera;

	void Start ()
    {
        for(int i = 0; i < pumpkinSize; i++)
        {
            MakePumpkins();
        }

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

    void MakePumpkins()
    {
        Vector3 pumpPosition = new Vector3(
               Random.Range(leapTransform.position.x - 0.3f, leapTransform.position.x + 0.04f),
               Random.Range(leapTransform.position.y + 0.17f, leapTransform.position.y + 0.30f),
               Random.Range(leapTransform.position.z - 0.14f, leapTransform.position.z + 0.14f)
               );

        Instantiate(targetPumpkinPrefab, pumpPosition, Quaternion.LookRotation(localCamera.position));
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
