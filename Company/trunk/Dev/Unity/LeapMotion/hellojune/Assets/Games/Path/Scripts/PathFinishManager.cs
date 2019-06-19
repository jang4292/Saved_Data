using UnityEngine;
using System.Collections;

public class PathFinishManager : MonoBehaviour
{
    public LoadSceneManager manager;
    public GameObject gameover;

	void OnCollisionEnter(Collision coll)
    {
        manager.goStop();
        gameover.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
    }
}
