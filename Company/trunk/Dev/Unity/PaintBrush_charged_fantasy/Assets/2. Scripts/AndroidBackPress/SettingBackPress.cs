using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackPress
{
    public class SettingBackPress : MonoBehaviour, BackPressListener
    {
        public void OnBackPress()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
