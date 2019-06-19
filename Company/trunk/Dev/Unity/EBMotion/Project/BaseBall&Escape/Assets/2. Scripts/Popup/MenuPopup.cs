using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuPopup : MonoBehaviour, PopupListener
{
    public ToggleGroup levelGroup;
    public ToggleGroup directionGroup;

    public void OnConfirm()
    {
        GetClickSound();

        SetLevelState(levelGroup.ActiveToggles().FirstOrDefault().name);
        SetDirectionState(directionGroup.ActiveToggles().FirstOrDefault().name);

        if (GameState.gameState == GameState.Game.BaseBall)
        {
            SceneFade.LoadLevel("BaseBall", 1, 1, Color.white);
        }
        else
        {
            SceneFade.LoadLevel("Escape", 1, 1, Color.white);
        }
    }

    public void OnCancel()
    {
        GetClickSound();

        gameObject.SetActive(false);
    }

    private void SetLevelState(string toggleName)
    {
        switch (toggleName)
        {
            case "Easy":
                GameState.levelState = GameState.Level.Easy;
                break;
            case "Nomal":
                GameState.levelState = GameState.Level.Nomal;
                break;
            case "Hard":
                GameState.levelState = GameState.Level.Hard;
                break;
        }
    }

    private void SetDirectionState(string toggleName)
    {
        switch (toggleName)
        {
            case "Right":
                GameState.directionState = GameState.Direction.Right;
                break;
            case "Left":
                GameState.directionState = GameState.Direction.Left;
                break;
        }
    }

    public void GetClickSound()
    {
        SoundManager.Instance.GetClickSound();
    }
}
