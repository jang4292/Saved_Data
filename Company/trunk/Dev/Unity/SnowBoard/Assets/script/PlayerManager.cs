using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

    public float Speed;
    // Use this for initialization
    void Start()
    {
        Speed = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {

#if TEST
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
#endif

    }
}