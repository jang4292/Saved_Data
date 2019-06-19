using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image imgBaseBall;
    public Image imgEscape;
    public Image imgBackground;

    public Sprite sprBaseBall_Selected;
    public Sprite sprBaseBall_Deselected;

    public Sprite sprEscape_Selected;
    public Sprite sprEscape_Deselected;

    public Sprite sprBackground_BaseBall;
    public Sprite sprBackground_Escape;

    public GameObject menuPopup;
    public GameObject infoPopup;
    
    void Awake()
    {
        SetDispaly();
    }

    void Start()
    {
        SoundManager.Instance.GetBgmSound();
    }

    private void SetDispaly()
    {
        float height = Screen.width / 16f * 9;
        Screen.SetResolution(Screen.width, (int)(height / 1f) + (height % 1f > 0 ? 1 : 0), true);
    }

    public void SelectedBaseBall()
    {
        SoundManager.Instance.GetClickSound();

        GameState.gameState = GameState.Game.BaseBall;

        imgBaseBall.sprite = sprBaseBall_Selected;
        imgEscape.sprite = sprEscape_Deselected;
        imgBackground.sprite = sprBackground_BaseBall;
    }

    public void SelectedEscape()
    {
        SoundManager.Instance.GetClickSound();

        GameState.gameState = GameState.Game.Escape;

        imgBaseBall.sprite = sprBaseBall_Deselected;
        imgEscape.sprite = sprEscape_Selected;
        imgBackground.sprite = sprBackground_Escape;
    }

    public void StartGame()
    {
        SoundManager.Instance.GetClickSound();
        menuPopup.SetActive(true);
    }

    public void QuitGame()
    {
        SoundManager.Instance.GetClickSound();
        Application.Quit();
    }

    public void SetInfoActive(bool active)
    {
        SoundManager.Instance.GetClickSound();
        infoPopup.SetActive(active);
    }
}
