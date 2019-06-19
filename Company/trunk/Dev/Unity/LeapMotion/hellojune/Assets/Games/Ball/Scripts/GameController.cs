using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : AbstractGameController
{
    private string gameName = "balltaking";

    //공 프리팹
    public GameObject redBallPrefabs;
    public GameObject blueBallPrefabs;
    public GameObject yellowBallPrefabs;

    public Transform leapTransform;

    //HUD(Head Up Display)
    public Text gameGoalText;
    public Text nowGameLevelText;
    public Text durationText;
    public Text pointText;
    public GameObject gameOverUI;
    public GameObject resetUI;
    public GameObject exitUI;

    bool isGameOver = false;

    private int sumOfPlayTime = 0;
    private int points = 0;

    public int nowGameLevel = 1;

    //색깔별 공 갯수
    public int totalRedBall;
    public int totalBlueBall;
    public int totalYellowBall;

    //오른쪽 상자로 옴겨야할 공 갯수
    private int redBallsMoveToRight;
    private int blueBallsMoveToRight;
    private int yellowBallsMoveToRight;
    private int sumOfBallsMoveToRight;

	void Start ()
    {
        StartCoroutine(ControlGameMessageUsingGameTime());
        MakeBalls();
    }

    IEnumerator ControlGameMessageUsingGameTime()
    {
        yield return new WaitForSeconds(5.0f);
        MakeGoal();

        while (!isGameOver)
        {
            durationText.text = sumOfPlayTime + " sec";
            pointText.text = points + " point";
            yield return new WaitForSeconds(1.0f);
            sumOfPlayTime += 1;
        }
    }

    void MakeBalls()
    {
        int[] ballCountByColor = GetBallCountByColor(nowGameLevel);

        totalRedBall = ballCountByColor[0];
        totalBlueBall = ballCountByColor[1];
        totalYellowBall = ballCountByColor[2];

        InstantiateBalls(totalRedBall, "RedBall");
        InstantiateBalls(totalBlueBall, "BlueBall");
        InstantiateBalls(totalYellowBall, "YellowBall");
    }

    void MakeGoal()
    {
        if(nowGameLevel == 1)
        {
            redBallsMoveToRight = (int)Random.Range(1.0f, totalRedBall);
            this.gameGoalText.text = "붉은 공: " + redBallsMoveToRight + "개";
            this.gameGoalText.text += "\n푸른 공: " + 0 + "개";
            this.gameGoalText.text += "\n노란 공: " + 0 + "개";
        }
        else if(nowGameLevel == 2)
        {
            redBallsMoveToRight = (int)Random.Range(1.0f, totalRedBall);
            this.gameGoalText.text = "붉은 공: " + redBallsMoveToRight + "개";

            blueBallsMoveToRight = (int)Random.Range(1.0f, totalBlueBall);
            this.gameGoalText.text += "\n푸른 공: " + blueBallsMoveToRight + "개";

            this.gameGoalText.text += "\n노란 공: " + 0 + "개";

        }
        else if(nowGameLevel == 3)
        {
            redBallsMoveToRight = (int)Random.Range(1.0f, totalRedBall);
            this.gameGoalText.text = "붉은 공: " + redBallsMoveToRight + "개";

            blueBallsMoveToRight = (int)Random.Range(1.0f, totalBlueBall);
            this.gameGoalText.text += "\n푸른 공: " + blueBallsMoveToRight + "개";

            yellowBallsMoveToRight = (int)Random.Range(1.0f, totalYellowBall);
            this.gameGoalText.text += "\n노란 공: " + yellowBallsMoveToRight + "개";
        }
    }

    void DestroyBalls()
    {
        GameObject[] redBalls = GameObject.FindGameObjectsWithTag("RedBall");
        GameObject[] blueBalls = GameObject.FindGameObjectsWithTag("BlueBall");
        GameObject[] yellowBalls = GameObject.FindGameObjectsWithTag("YellowBall");

        foreach (GameObject obj in redBalls)
        {
            DestroyObject(obj);
        }

        foreach (GameObject obj in blueBalls)
        {
            DestroyObject(obj);
        }

        foreach (GameObject obj in yellowBalls)
        {
            DestroyObject(obj);
        }
    }

    //색상별 볼 갯수 얻기
    int[] GetBallCountByColor(int nowGameLevel)
    {
        int redBalls    = 0;
        int blueBalls   = 0;
        int yellowBalls = 0;

        if (nowGameLevel == 1)
        {
            redBalls = 10;
            blueBalls = 0;
            yellowBalls = 0;
        }
        else if(nowGameLevel == 2)
        {
            redBalls  = (int)Random.Range(2.0f, 8.0f);
            blueBalls = 10 - redBalls;
            yellowBalls = 0;
        }
        else if(nowGameLevel == 3)
        {
            redBalls    = (int)Random.Range(2.0f, 6.0f);
            blueBalls   = (int)Random.Range(2.0f, 8.0f - redBalls);
            yellowBalls = 10 - redBalls - blueBalls;
        }

        return new int[3] { redBalls, blueBalls, yellowBalls };
    }

    //색깔별 공 객체 생성
    void InstantiateBalls(int ballCount, string ballColor)
    {
        for (int i = 0; i < ballCount; i++)
        {
            InstantiateSingleBall(ballColor);
        }
    }

    public void InstantiateSingleBall(string ballColor)
    {
        GameObject targetBallPrefab;

        if (ballColor == "RedBall")
            targetBallPrefab = redBallPrefabs;
        else if (ballColor == "BlueBall")
            targetBallPrefab = blueBallPrefabs;
        else
            targetBallPrefab = yellowBallPrefabs;

        Vector3 ballPosition;

        ballPosition = new Vector3(
               Random.Range(leapTransform.position.x - 0.3f, leapTransform.position.x + 0.04f),
               Random.Range(leapTransform.position.y + 0.17f, leapTransform.position.y + 0.30f),
               Random.Range(leapTransform.position.z - 0.14f, leapTransform.position.z + 0.14f)
               );

        Instantiate(targetBallPrefab, ballPosition, Quaternion.identity);
    }

    public void CountEnteredBallToLeapMotionHandTrigger(GameObject ballObj)
    {
        if(ballObj.tag == "RedBall")
        {
            if(redBallsMoveToRight > 0)
            {
                redBallsMoveToRight -= 1;
                points += 1;
            }
            else
            {
                InstantiateSingleBall(ballObj.tag);
                Destroy(ballObj);
            }
        }
        else if(ballObj.tag == "BlueBall")
        {
            if (blueBallsMoveToRight > 0)
            {
                blueBallsMoveToRight -= 1;
                points += 1;
            }
            else
            {
                InstantiateSingleBall(ballObj.tag);
                Destroy(ballObj);
            }
        }
        else if(ballObj.tag == "YellowBall")
        {
            if (yellowBallsMoveToRight > 0)
            {
                yellowBallsMoveToRight -= 1;
                points += 1;
            }
            else
            {
                InstantiateSingleBall(ballObj.tag);
                Destroy(ballObj);
            }
        }

        sumOfBallsMoveToRight = redBallsMoveToRight + blueBallsMoveToRight + yellowBallsMoveToRight;

        if(sumOfBallsMoveToRight <= 0)
        {
            MoveToNextGameLevel(nowGameLevel + 1);
        }

    }

    public void MoveToNextGameLevel(int nowGameLevel)
    {
        if(nowGameLevel == 4)
        {
            gameOverUI.SetActive(true);
            resetUI.SetActive(true);
            exitUI.SetActive(true);
            isGameOver = true;
        }
        else
        {
            this.nowGameLevel = nowGameLevel;
            this.nowGameLevelText.text = "Level "+nowGameLevel.ToString(); 

            DestroyBalls();
            MakeBalls();
            MakeGoal();
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
