using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

    [SerializeField]
    private Animator[] trees = null;

    void Awake() {

        if (trees != null)
        {
            for (int i = 0; i < trees.Length; i++)
            {
                float number = Random.Range(1, 2.6f);
                trees[i].speed = number;
            }
        }
    }
    
}
