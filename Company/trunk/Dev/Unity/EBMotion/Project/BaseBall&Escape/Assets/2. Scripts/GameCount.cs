using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCount : MonoBehaviour {

    public Text countText;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        countText.text = "2";

        yield return new WaitForSeconds(1f);

        countText.text = "1";

        yield return new WaitForSeconds(1f);

        countText.text = "0";

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
        GameState.playingState = GameState.Playing.Playing;
    }
}
