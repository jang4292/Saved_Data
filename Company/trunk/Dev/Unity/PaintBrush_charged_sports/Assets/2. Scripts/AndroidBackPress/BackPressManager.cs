using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackPress
{
    public class BackPressManager : MonoBehaviour
    {
        private BackPressListener backPress;

        void Awake()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Main":
                        backPress = GetComponent<MainBackPress>();
                        break;
                    case "Game":
                        backPress = GetComponent<GameBackPress>(); 
                        break;
                    case "Store":
                        backPress = GetComponent<StoreBackPress>();
                        break;
                    case "Album":
                        backPress = GetComponent<AlbumBackPress>();
                        break;
                    case "Setting":
                        backPress = GetComponent<SettingBackPress>();
                        break;
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                backPress.OnBackPress();
            }
        }
    }
}
