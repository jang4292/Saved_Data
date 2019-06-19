using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackPress
{
    public class AlbumBackPress : MonoBehaviour, BackPressListener
    {
        public void OnBackPress()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
