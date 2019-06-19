using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {

    public GroundManager GroundM;

    public GameObject Gondola_A;
    public GameObject Gondola_B;
    public GameObject[] Gondolas;
	// Use this for initialization
	void Start () {
        GroundM.moveSpeed = 12f;
	}
	
	// Update is called once per frame
	void Update () {
        Gondola_move();
    }
    void Gondola_move() {
        Gondola_A.transform.position += new Vector3(0, GroundM.moveSpeed / 3.6f * Time.deltaTime, -GroundM.moveSpeed * Time.deltaTime);
        Gondola_B.transform.position += new Vector3(0, GroundM.moveSpeed / 3.6f * Time.deltaTime, -GroundM.moveSpeed * Time.deltaTime);
        if (Gondola_B.transform.position.z < -100f)
        {
            Destroy(Gondola_A);
            Gondola_A = Gondola_B;
            Godola_repeat();
        }
    }
    void Godola_repeat() {
        int A = Random.Range(0, Gondolas.Length);
        Gondola_B = Instantiate(Gondolas[A], new Vector3(-17.5f, -44f, 209f), transform.rotation) as GameObject;
        Gondola_B.transform.Rotate(-0.8f, 1.3f, 0);
    }
}
