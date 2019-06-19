using UnityEngine;

public class InputData : MonoBehaviour
{

    public Window window;
    public GameObject obj;
    public Shoes shoes;

    [System.NonSerialized]
    private int amount;

    private Vector3 impulse;
    private Vector3 startPos;

    void Start()
    {
        startPos = obj.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GameState.playingState == GameState.Playing.Playing)
        {
            if (collision.gameObject.name.Equals("InputKick") && shoes.state == Shoes.AnimationState.Idle)
            {
                impulse = collision.impulse / Time.fixedDeltaTime;

                if (impulse.z > 2000f)
                {
                    amount = 2000;
                }

                amount = amount < 300 ? 300 : Mathf.Abs((int)impulse.z);

                window.SetWindowPercent(amount);
                window.setTouchObject(gameObject, obj, startPos);
                shoes.startAnimation();
            }
        }
    }
}
