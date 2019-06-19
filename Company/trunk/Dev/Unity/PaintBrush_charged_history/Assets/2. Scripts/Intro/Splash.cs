using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Intro
{
    public class Splash : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("Main");
        }

    }
}
