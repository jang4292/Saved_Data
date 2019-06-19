using UnityEngine;
using System.Collections;

public class GroundManager : MonoBehaviour {

    public int GroundPoolsize = 10;
    public GameObject[] Startobject;
    public GameObject[] MountainBlock;
    public GameObject A_Zone;
    public GameObject B_Zone;
    public GameObject C_Zone;
    public GameObject D_Zone;
    public GameObject E_Zone;
    public GameObject F_Zone;
    public GameObject G_Zone;
    public GameObject H_Zone;

    public float moveSpeed;
    // Use this for initialization
    void Start () {

        Startobject = GameObject.FindGameObjectsWithTag("StartGround");
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }
    void Move() {
        A_Zone.transform.position += new Vector3(0, moveSpeed/3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        B_Zone.transform.position += new Vector3(0, moveSpeed/3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        if (B_Zone.transform.position.z < -100f ) {
            Destroy(A_Zone);
            A_Zone = B_Zone;
            Mountain_BZone();
            Debug.Log("Bzone");
        }
        C_Zone.transform.position += new Vector3(0, moveSpeed / 3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        D_Zone.transform.position += new Vector3(0, moveSpeed / 3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        if (D_Zone.transform.position.z < -100f)  {
            Destroy(C_Zone);
            C_Zone = D_Zone;
            Mountain_DZone();
            Debug.Log("Dzone");
        }
        E_Zone.transform.position += new Vector3(0, moveSpeed / 3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        F_Zone.transform.position += new Vector3(0, moveSpeed / 3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        if (F_Zone.transform.position.z < -100f)  {
            Destroy(E_Zone);
            E_Zone = F_Zone;
            Mountain_FZone();
            Debug.Log("Fzone");
        }
        G_Zone.transform.position += new Vector3(0, moveSpeed / 3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        H_Zone.transform.position += new Vector3(0, moveSpeed / 3.6f * Time.deltaTime, -moveSpeed * Time.deltaTime);
        if (H_Zone.transform.position.z < -100f)
        {
            Destroy(G_Zone);
            G_Zone = H_Zone;
            Mountain_HZone();
            Debug.Log("Hzone");
        }
    }
    void Mountain_BZone()
    {
        int A = Random.Range(0, MountainBlock.Length);
        B_Zone = Instantiate(MountainBlock[A], new Vector3(0, 7.60f, 36.3f), transform.rotation) as GameObject;
        B_Zone.transform.Rotate(-0.9f, 0, 0);
    }
    void Mountain_DZone()
    {
        int A = Random.Range(0, MountainBlock.Length);
        D_Zone = Instantiate(MountainBlock[A], new Vector3(0, 7.61f, 36.3f), transform.rotation) as GameObject;
        D_Zone.transform.Rotate(-0.608f, 0, 0);
    }
    void Mountain_FZone()
    {
        int A = Random.Range(0, MountainBlock.Length);
        F_Zone = Instantiate(MountainBlock[A], new Vector3(0, 7.60f, 36.3f), transform.rotation) as GameObject;
        F_Zone.transform.Rotate(-0.9f, 0, 0);
    }
    void Mountain_HZone()
    {
        int A = Random.Range(0, MountainBlock.Length);
        H_Zone = Instantiate(MountainBlock[A], new Vector3(0, 7.61f, 36.3f), transform.rotation) as GameObject;
        H_Zone.transform.Rotate (- 0.608f, 0, 0);
    }
}
