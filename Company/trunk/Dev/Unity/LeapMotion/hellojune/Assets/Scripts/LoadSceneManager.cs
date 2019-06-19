using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneManager : MonoBehaviour
{
    public GameObject retryUI;
    public GameObject gomainUI;
    public UnityWebManager web;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void saveLogAndCallScene(string sceneName)
    {
        web.requestPlayLog(sceneName);
    }

    public void goStop()
    {
        retryUI.SetActive(true);
        gomainUI.SetActive(true);
    }

    public void ExitScene()
    {
        Application.Quit();
    }
}
