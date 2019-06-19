using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    public Sprite successImage;
    public Sprite failImage;

    public Sprite successText;
    public Sprite failText;

    public Image resultImage;
    public Image resultText;

    void Start()
    {
        GameState.playingState = GameState.Playing.Pause;

        bool result = false;

        if (GameState.gameState == GameState.Game.BaseBall)
        {
            result = GameObject.FindObjectOfType<BaseBallManager>().result;
        }
        else
        {
            result = GameObject.FindObjectOfType<EscapeManager>().result;
        }

        if (result)
        {
            SetSuccess();
        }
        else
        {
            SetFail();
        }
    }

    private void SetSuccess()
    {
        // 성공
        resultImage.sprite = successImage;
        resultText.sprite = successText;
    }

    private void SetFail()
    {
        // 실패
        resultImage.sprite = failImage;
        resultText.sprite = failText;
    }

    public void Restart()
    {
        SoundManager.Instance.GetClickSound();

        if(GameState.gameState == GameState.Game.BaseBall)
        {
            SceneFade.LoadLevel("BaseBall", 1, 1, Color.white);
        }
        else
        {
            SceneFade.LoadLevel("Escape", 1, 1, Color.white);
        }
    }

    public void Home()
    {
        SoundManager.Instance.GetClickSound();

        SceneFade.LoadLevel("Menu", 1, 1, Color.white);
    }
}
