using UnityEngine;

public class Ball : MonoBehaviour {

    private SpriteRenderer mSpriteRenderer;
    private BaseBallManager manager;

    public Sprite sprGreenBall;
    public Sprite sprRedBall;

    void Awake()
    {
        manager = GameObject.FindObjectOfType<BaseBallManager>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
	    if(GameState.playingState == GameState.Playing.Playing)
        {
            transform.Rotate(Vector3.forward * 10);
            SetObjectScalePlus();
        }
	}

    void OnEnable()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 1);
        mSpriteRenderer.sprite = sprGreenBall;
    }

    void SetObjectScalePlus()
    {
        Vector3 scale = transform.localScale;

        if(scale.x > 0.7f)
        {
            mSpriteRenderer.sprite = sprRedBall;
        }

        if(scale.x > 1f)
        {
            gameObject.SetActive(false);
            manager.GetShowEffect("Miss");
        }

        scale.x += manager.speed * Time.smoothDeltaTime;
        scale.y += manager.speed * Time.smoothDeltaTime;
        transform.localScale = scale;
    }
}
