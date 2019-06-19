using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoleGameController : AbstractGameController
{
    private string gameName = "mole";

    public Text durationText;
    public Text pointText;
    private int sumOfPlayTime = 0;
    public int points = 0;

    bool isGameOver = false;

    public GameObject[] moleModels = new GameObject[9];

    void Start()
    {
        StartCoroutine(ControlMoleUp());
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

    IEnumerator ControlMoleUp()
    {
        Debug.Log("3초뒤 시작합니다.");
        yield return new WaitForSeconds(3.0f);

        GameObject targetObj = null;
        GameObject rightBeforeTargetObj = null;

        while (true)
        {
            targetObj = moleModels[(int)Random.Range(0.0f, 8.0f)];

            if (targetObj != rightBeforeTargetObj)
            {
                targetObj.transform.localPosition = new Vector3(
                                                targetObj.transform.localPosition.x,
                                                targetObj.transform.localPosition.y + 1.0f,
                                                targetObj.transform.localPosition.z
                                                );

                targetObj.GetComponent<Rigidbody>().useGravity = false;
                yield return new WaitForSeconds(Random.Range(2.0f,3.0f));

                targetObj.GetComponent<Rigidbody>().useGravity = true;
                rightBeforeTargetObj = targetObj;
                yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
            }
        }
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
