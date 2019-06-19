using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GamePopup : MonoBehaviour, PopupListener {

    public ToggleGroup group;

    public void OnConfirm()
    {
        GetClickSound();
        if (group.ActiveToggles().FirstOrDefault().name.Equals("Restart"))
        {
            if(GameState.gameState == GameState.Game.BaseBall)
            {
                SceneFade.LoadLevel("BaseBall", 1, 1, Color.white);
            }
            else
            {
                SceneFade.LoadLevel("Escape", 1, 1, Color.white);
            }
        }
        else
        {
            SceneFade.LoadLevel("Menu", 1, 1, Color.white);
        }
    }

    public void OnCancel()
    {
        GetClickSound();
        GameState.playingState = GameState.Playing.Playing;
        gameObject.SetActive(false);
    }

    public void GetClickSound()
    {
        SoundManager.Instance.GetClickSound();
    }
}
