using UnityEngine;

public class Glove : MonoBehaviour {

    private const float RIGHT_ROTATION = 0f;
    private const float LEFT_ROTATION = 180f;

    public bool mouseController;

    private BaseBallManager manager;

    void Start () {

	    if(GameState.directionState == GameState.Direction.Right)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, RIGHT_ROTATION, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, LEFT_ROTATION, 0));
        }

        manager = GameObject.FindObjectOfType<BaseBallManager>();
    }

	void Update () {
        if (GameState.playingState == GameState.Playing.Playing)
        {
            if (mouseController)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                transform.position = mousePos;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        SpriteRenderer spr = other.gameObject.GetComponent<SpriteRenderer>();

        if (spr.sprite.name.Equals("ball_hit_ball"))
        {
            other.gameObject.SetActive(false);
            manager.GetShowEffect("Catch");
            manager.SetCatchText();
        }
    }
}
