using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseBallManager : MonoBehaviour
{
    [System.NonSerialized]
    public float speed = 0.3f;
    private float progressSpeed = 0.25f;
    public Text speedText;
    public Image progress;

    public GameObject[] ballObjs;

    public GameObject effectObj;
    public Sprite sprCatchEffect;
    public Sprite sprMissEffect;

    public Text throwBallText;
    private int throwCount = 0;

    public Text catchBallText;
    private int catchCount = 0;

    public Text targetText;
    private int targetCount = 0;

    [System.NonSerialized]
    public bool result = false;

    public Text timeText;
    public float timeCount;

    public GameObject gamePopup;
    public GameObject resultPopup;

    void Start()
    {
        SetLevel(GameState.levelState);
        SoundManager.Instance.GetBgmSound();
    }

    void Update()
    {
        if (GameState.playingState == GameState.Playing.Playing)
        {
            SetProgress();
            SetTimeTexT();

            if(timeCount < 0)
            {
                SoundManager.Instance.GetFailSound();
                resultPopup.SetActive(true);
            }
        }
    }

    private void SetLevel(GameState.Level level)
    {
        switch (level)
        {
            case GameState.Level.Easy:
                speed = 0.3f;
                progressSpeed = 0.25f;
                speedText.text = "100km";

                targetCount = 10;
                //targetCount = 20;
                break;
            case GameState.Level.Nomal:
                speed = 0.5f;
                progressSpeed = 0.45f;
                speedText.text = "125km";

                targetCount = 10;
                //targetCount = 30;
                break;
            case GameState.Level.Hard:
                speed = 0.7f;
                progressSpeed = 0.65f;
                speedText.text = "150km";

                targetCount = 10;
                //targetCount = 40;
                break;
        }

        targetText.text = "/" + targetCount;
    }

    private void SetProgress()
    {
        if (progress.fillAmount < 1)
        {
            progress.fillAmount += progressSpeed * Time.smoothDeltaTime;
        }
        else
        {
            SoundManager.Instance.GetShootOrSighSound();

            throwCount += 1;
            SetBallCountText(throwCount, throwBallText);

            progress.fillAmount = 0;
            ballObjs[Random.Range(0, 6)].SetActive(true);
        }
    }

    private void SetBallCountText(int count, Text countText)
    {
        if(count > 9)
        {
            countText.text = count + "";
        }
        else
        {
            countText.text = "0" + count;
        }
    }

    private void SetTimeTexT()
    {
        timeCount -= Time.smoothDeltaTime;

        if (timeCount > 60)
        {
            int minute = (int)timeCount / 60;
            int second = (int)timeCount - (minute * 60);

            timeText.text = GetTimeString(minute) + ":" + GetTimeString(second);
        }
        else
        {
            timeText.text = "00:" + GetTimeString((int)timeCount);
        }
    }

    private string GetTimeString(int number)
    {
        if(number < 10)
        {
            return "0" + number;
        }
        else
        {
            return "" + number;
        }
    }

    public void SetCatchText()
    {
        catchCount += 1;
        SetBallCountText(catchCount, catchBallText);

        if(catchCount >= targetCount)
        {
            SoundManager.Instance.GetSuccessSound();
            result = true;
            resultPopup.SetActive(true);
        }
    }

    public void GetShowEffect(string effect)
    {
        switch (effect)
        {
            case "Catch":
                SoundManager.Instance.GetCatchSound();
                effectObj.GetComponent<SpriteRenderer>().sprite = sprCatchEffect;
                break;
            case "Miss":
                SoundManager.Instance.GetMissSound();
                effectObj.GetComponent<SpriteRenderer>().sprite = sprMissEffect;
                break;
        }

        StartCoroutine(ShowEffect());
    }

    IEnumerator ShowEffect()
    {
        effectObj.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        effectObj.SetActive(false);
    }

    public void OnPause()
    {
        SoundManager.Instance.GetClickSound();
        GameState.playingState = GameState.Playing.Pause;
        gamePopup.SetActive(true);
    }
}
