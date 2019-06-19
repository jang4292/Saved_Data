using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    private const float PERCENT_POS_RIGHT = -3.3f;
    private const float PERCENT_POS_LEFT = 5.3f;

    /// <summary>
    /// 0 - 0%
    /// 1 - 30%
    /// 2 - 70%
    /// 3 - 100%
    /// </summary>
    public Sprite[] windowImages;

    /// <summary>
    /// 0 ~ 9
    /// </summary>
    public Sprite[] percentImages;

    public GameObject percentObj;

    public GameObject hundredsObj;
    public SpriteRenderer tensSprRen;
    public SpriteRenderer unitsSprRen;

    private Image windowImage;
    private float startValue;
    private Color alphaValue;
    private float windowSpeed = 0.3f;

    private int percent;
    private bool resetCheck;

    public Image doorImage;
    public GameObject[] disableObj;

    public EscapeManager manager;

    void Awake()
    {
        windowImage = GetComponent<Image>();
    }

    void Start()
    {
        if(GameState.directionState == GameState.Direction.Right)
        {
            percentObj.transform.position = new Vector3(PERCENT_POS_RIGHT, 1f, 0);
        }
        else
        {
            percentObj.transform.position = new Vector3(PERCENT_POS_LEFT, 1f, 0);
        }
    }

    public void SetWindowPercent(int amount)
    {
        if (resetCheck) return;

        percent = percent + (int)(amount / 2000f * 100);

        if (percent > 30 && percent < 70)
        {
            SoundManager.Instance.GetMiddleBreakSound();
            windowImage.sprite = windowImages[1];
        }
        else if (percent > 69 && percent < 100)
        {
            SoundManager.Instance.GetMiddleBreakSound();
            windowImage.sprite = windowImages[2];
        }
        else if (percent >= 100)
        {
            SoundManager.Instance.GetBigBreakSound();
            windowImage.sprite = windowImages[3];
            percent = 100;
            StartCoroutine(ShowWindowEffect());
            resetCheck = true;
        }
        else
        {
            SoundManager.Instance.GetSmallBreakSound();
        }

        SetPercentText(percent);
    }

    public void SetPercentText(int percent)
    {
        if (percent != 100)
        {
            tensSprRen.sprite = percentImages[percent / 10];
            unitsSprRen.sprite = percentImages[percent % 10];
        }
        else
        {
            hundredsObj.SetActive(true);
            tensSprRen.sprite = percentImages[0];
            unitsSprRen.sprite = percentImages[0];
        }

        if (!percentObj.activeInHierarchy)
        {
            StartCoroutine(ShowPercentText());
        }
    }

    public void setTouchObject(GameObject enableObj, GameObject obj, Vector3 startPos)
    {
        StartCoroutine(EanbleObject(enableObj, obj, startPos));
    }

    IEnumerator EanbleObject(GameObject enableObj, GameObject obj, Vector3 startPos)
    {
        enableObj.SetActive(false);

        while (obj.transform.position.y > startPos.y)
        {
            yield return null;
        }

        enableObj.SetActive(true);
    }

    IEnumerator ShowPercentText()
    {
        percentObj.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        percentObj.SetActive(false);
    }

    IEnumerator ShowWindowEffect()
    {
        startValue = windowImage.color.a;
        while (windowImage.color.a > 0)
        {
            alphaValue = windowImage.color;
            alphaValue.a -= windowSpeed * Time.smoothDeltaTime;
            windowImage.color = alphaValue;

            yield return null;
        }

        manager.SetProgress();
        manager.SetPercent();

        yield return new WaitForSeconds(1f);

        if(manager.windowCount > 0)
        {
            ResetWindow();
        }
        else
        {
            SetDisableObjs();

            while (doorImage.color.a < startValue)
            {
                alphaValue = doorImage.color;
                alphaValue.a += windowSpeed * Time.smoothDeltaTime;
                doorImage.color = alphaValue;

                yield return null;
            }

            SoundManager.Instance.StopBgmSound();
            SoundManager.Instance.GetShootOrSighSound();

            yield return new WaitForSeconds(2f);

            SoundManager.Instance.GetSuccessSound();

            manager.result = true;
            manager.resultPopup.SetActive(true);
        }

        resetCheck = false;
    }

    private void ResetWindow()
    {
        alphaValue = windowImage.color;
        alphaValue.a = startValue;
        windowImage.color = alphaValue;

        windowImage.sprite = windowImages[0];
        percent = 0;
        hundredsObj.SetActive(false);
    }

    private void SetDisableObjs()
    {
        foreach(GameObject obj in disableObj)
        {
            obj.SetActive(false);
        }
    }
   
}
