using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultManager : MonoBehaviour {

    // 네임텍스트
    [SerializeField]
    private GameObject nameText = null;

    // 스코어 텍스트
    [SerializeField]
    private GameObject[] scoreText = null;

    // 게이지 모음
    [SerializeField]
    private GameObject[] progress = null;

    // 게이지 마지막 모음
    [SerializeField]
    private GameObject[] lastBar = null;

    // 스코어 날짜 텍스트
    [SerializeField]
    private GameObject[] scoreDayText = null;

    // 유저 텍스트
    // 0 = 고정, 1 = 유저 이름, 2 = 최근 날짜
    [SerializeField]
    private GameObject[] userText = null;

    // 결과 점수, 결과 시간
    // 0 = 점수 , 1 = 시간
    [SerializeField]
    private GameObject[] resultText = null;

    // 스코어 날짜 모음
    private string[] scoreDayArray = new string[5];

    // 스코어 모음
    private int[] scoreArray = new int[5];

    // 불러올 데이터
    private DataSave data = null;

    // 결과 점수, 시간
    // 0 = 점수, 1 = 시간 
    public string[] resultArray = new string[2];

    // 결과창
    private GameObject result;

    private int dataScore = 0;

    private static bool startCheck = false;

    private int playerNumber;

    void Awake()
    {
        playerNumber = PlayerPrefs.GetInt("user");
        result = GameObject.Find("ResultBackground");

        if (startCheck == false)
        {
            data = GameObject.Find("Kinect").GetComponent<DataSave>();
            startCheck = true;
        }
        else
        {
            data = GameObject.Find("four").GetComponent<DataSave>();
        }

        if (!SceneManager.GetActiveScene().name.Equals("Intro"))
        {
            NameText();
        }
    }

    void Update()
    {
        if (result.activeInHierarchy && !resultArray[0].Equals("end"))
        {
            playerNumber = PlayerPrefs.GetInt("user");
            if (!SceneManager.GetActiveScene().name.Equals("Intro"))
            {
                data.saveData(playerNumber, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm"), dataScore, resultArray[1]);
            }

            for (int i = 0; i < data.sortNumberList(playerNumber).Count; i++)
            {
                switch (data.sortNumberList(playerNumber)[i])
                {
                    case "1":
                        scoreArray[i] = data.playerData[playerNumber].score_one;
                        break;
                    case "2":
                        scoreArray[i] = data.playerData[playerNumber].score_two;
                        break;
                    case "3":
                        scoreArray[i] = data.playerData[playerNumber].score_three;
                        break;
                    case "4":
                        scoreArray[i] = data.playerData[playerNumber].score_four;
                        break;
                    case "5":
                        scoreArray[i] = data.playerData[playerNumber].score_five;
                        break;
                }

                scoreDayArray[i] = data.sortDayList(playerNumber)[i];
            }

            ResultScrTime();

            ScoreText(scoreText, scoreArray, "score");
            ScoreText(progress, scoreArray, "progress");
            ScoreText(lastBar, scoreArray, "lastBar");
            ScoreDayText(scoreDayText, scoreDayArray);

            UserText();
        }
    }

    // 사용자 이름 
    private void NameText()
    {
        // 임시
        nameText.GetComponent<TextMesh>().text = data.playerData[playerNumber].name;
        nameText.GetComponent<MeshRenderer>().sortingOrder = 22;
    }

    // 그래프 , 스코어
    private void ScoreText(GameObject[] texts, int[] number,string name)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            Vector3 pos = texts[i].transform.position;
            if (name.Equals("progress")) {
                Vector3 scale = texts[i].transform.localScale;
                pos.x = -4.53f + (number[i] * 0.03f);
                scale.x = number[i] * 6;

                texts[i].transform.position = pos;
                texts[i].transform.localScale = scale;
            }
            else if (name.Equals("lastBar"))
            {
                pos.x = -4.41f + (number[i] * 0.06f);

                if (number[i] == 0)
                {
                    pos.x = -4.41f;
                }

                texts[i].transform.position = pos;
            }
            else if (name.Equals("score"))
            {
                texts[i].GetComponent<TextMesh>().text = number[i] + ",000";

                if (number[i] == 0)
                {
                    texts[i].GetComponent<TextMesh>().text = "0";
                    pos.x = -5.3f;
                    texts[i].transform.position = pos;
                }
                TextOrder(texts[i]);
            }
        }
    }

    // 스코어 날짜
    private void ScoreDayText(GameObject[] texts, string[] number)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            // 6번째 자리부터
            texts[i].GetComponent<TextMesh>().text = number[i].Substring(5,5) + "\n" + number[i].Substring(11,5);
            TextOrder(texts[i]);
        }
    }

    // 유저 이름, 날짜
    private void UserText()
    {
        userText[1].GetComponent<TextMesh>().text = data.playerData[playerNumber].name.Substring(0,3);
        userText[2].GetComponent<TextMesh>().text = System.DateTime.Now.ToString("yyyy/MM/dd");
        TextOrder(userText[0]);
        TextOrder(userText[1]);
        TextOrder(userText[2]);
    }

    // 결과 점수, 시간
    private void ResultScrTime()
    {
        TextOrder(resultText[0]);
        TextOrder(resultText[1]);

        string lastScore = "";

        if(scoreArray[0] == 0)
        {
            lastScore = "0점";
        }
        else
        {
            lastScore = scoreArray[0] + ",000점";
        }

        resultText[0].GetComponent<TextMesh>().text = lastScore;
        resultText[1].GetComponent<TextMesh>().text = data.playerData[playerNumber].time;

        Vector3 scorePos = resultText[0].transform.position;
        Vector3 timePos = resultText[1].transform.position;
        if (lastScore.Equals("0점"))
        {
            scorePos.x += 0.65f;
            resultText[0].transform.position = scorePos;
        }
        if (resultArray[1].Equals("TimeOver"))
        {
            timePos.x -= 0.45f;
            resultText[1].transform.position = timePos;
        }

        resultArray[0] = "end";
    }

    // 텍스트 order layer
    private void TextOrder(GameObject text)
    {
        text.GetComponent<MeshRenderer>().sortingOrder = 12;
    }

    // 결과 점수, 시간 변경
    public void resultRefresh(string score, string time, int number)
    {
        resultArray[0] = score;
        resultArray[1] = time;
        dataScore = number;
    }
}
