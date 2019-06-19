using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscapeManager : MonoBehaviour
{
    /// <summary>
    /// 0 ~ 9 
    /// </summary>
    public Sprite[] progressImages;

    public Image progress;
    public Image progressMan;

    public Text windowText;
    [System.NonSerialized]
    public float windowCount;

    public GameObject progressText;
    public GameObject endText;
    public Image tensText;
    public Image unitsText;

    private float maxProgress = 1f;
    private float maxMan = 10.2f;

    public Text timeText;
    public float timeCount;

    public GameObject gamePopup;
    public GameObject resultPopup;

    [System.NonSerialized]
    public bool result = false;

    void Start()
    {
        SetLevel(GameState.levelState);
        SoundManager.Instance.GetBgmSound();
        SoundManager.Instance.GetEscapeBurning();
    }

    private void SetLevel(GameState.Level level)
    {
        switch (level)
        {
            case GameState.Level.Easy:
                windowCount = 3;
                break;
            case GameState.Level.Nomal:
                windowCount = 5;
                break;
            case GameState.Level.Hard:
                windowCount = 7;
                break;
        }

        maxProgress = maxProgress / windowCount;
        maxMan = maxMan / windowCount;

        windowText.text = "0" + windowCount;
    }

    public void SetProgress()
    {
        progress.fillAmount += maxProgress;

        Vector3 pos = progressMan.transform.position;
        pos.x += maxMan;

        progressMan.transform.position = pos;
    }

    public void SetPercent()
    {
        int percent = (int)(progress.fillAmount * 100f);

        if(percent != 100)
        {
            tensText.sprite = progressImages[percent / 10];
            unitsText.sprite = progressImages[percent % 10];
        }
        else
        {
            progressText.SetActive(false);
            endText.SetActive(true);
        }

        windowCount--;
        windowText.text = "0" + windowCount;
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
        if (number < 10)
        {
            return "0" + number;
        }
        else
        {
            return "" + number;
        }
    }

    void Update()
    {
        if(GameState.playingState == GameState.Playing.Playing)
        {
            SetTimeTexT();

            if (timeCount < 0)
            {
                resultPopup.SetActive(true);
                SoundManager.Instance.GetFailSound();
            }
        }
    }

    public void OnPause()
    {
        GameState.playingState = GameState.Playing.Pause;
        gamePopup.SetActive(true);
    }
}
