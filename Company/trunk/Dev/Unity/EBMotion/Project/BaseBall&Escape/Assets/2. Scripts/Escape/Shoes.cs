using UnityEngine;
using System.Collections;
using EBMotion;

public class Shoes : MonoBehaviour {

    public enum AnimationState
    {
        Idle,
        Start
    }


    public AnimationState state;

    private float speed = 0.05f;

    private Vector3 rightPos = new Vector3(4.28f, -1.5f, 0);
    private Vector3 leftPos = new Vector3(-4.28f, -1.5f, 0);

    private const float RIGHT_ROTATION = 0f;
    private const float LEFT_ROTATION = 180f;

    void Awake()
    {
        if (GameState.directionState == GameState.Direction.Right)
        {
            transform.position = rightPos;
            transform.rotation = Quaternion.Euler(new Vector3(0, RIGHT_ROTATION, 0));
        }
        else
        {
            transform.position = leftPos;
            transform.rotation = Quaternion.Euler(new Vector3(0, LEFT_ROTATION, 0));
        }
    }

    private EbmDataParser ebmData;

    [Tooltip("손 오브젝트")]
    public GameObject rightHandObj = null;
    public GameObject leftHandObj = null;

    private Vector3 startPos = Vector3.zero;
    [Tooltip("처음 손 위치")]
    private Vector3 firstPos = Vector3.zero;
    [Tooltip("움직이는 손 위치")]
    private Vector3 movePos = Vector3.zero;

    void Start()
    {
        ebmData = GameObject.Find("EbmDataParser").GetComponent<EbmDataParser>();
        StartCoroutine(StartSetPos());
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (state == AnimationState.Idle && ebmData.EbmPort.IsOpen)
        {
            MovePosition();
        }
    }

    void MovePosition()
    {
        if (GameState.directionState == GameState.Direction.Right)
        {
            movePos = rightHandObj.transform.position;
        }
        else
        {
            movePos = leftHandObj.transform.position;
        }

        movePos = firstPos - movePos;

        movePos.x = startPos.x + (GameState.directionState == GameState.Direction.Right ? -movePos.z * 15f : movePos.z * 15f);
        movePos.y = -1.5f;
        movePos.z = 0;

        transform.position = movePos;
    }

    // 캘리브레이션 후 시작 포지션 변경
    IEnumerator StartSetPos()
    {
        yield return new WaitForSeconds(3f);
        if (GameState.directionState == GameState.Direction.Right)
        {
            firstPos = rightHandObj.transform.position;
        }
        else
        {
            firstPos = leftHandObj.transform.position;
        }
    }

    public void startAnimation()
    {
        state = AnimationState.Start;

        if (GameState.directionState == GameState.Direction.Right)
        {
            StartCoroutine(KickRightAnimation());
        }
        else
        {
            StartCoroutine(KickLeftAnimation());
        }
    }

    IEnumerator KickLeftAnimation()
    {
        while(transform.position.x > startPos.x)
        {
            Vector3 pos = transform.position;
            pos.x -= speed;
            transform.position = pos;
            yield return null;
        }

        while (!(movePos.x < startPos.x + 0.3f && movePos.x > startPos.x - 0.3f))
        {
            if (GameState.directionState == GameState.Direction.Right)
            {
                movePos = rightHandObj.transform.position;
            }
            else
            {
                movePos = leftHandObj.transform.position;
            }

            movePos = firstPos - movePos;

            movePos.x = startPos.x + movePos.z * 10f;
            yield return null;
        }

        state = AnimationState.Idle;
    }

    IEnumerator KickRightAnimation()
    {
        while (transform.position.x < startPos.x)
        {
            Vector3 pos = transform.position;
            pos.x += speed;
            transform.position = pos;
            yield return null;
        }

        while (!(movePos.x < startPos.x + 0.3f && movePos.x > startPos.x - 0.3f))
        {
            if (GameState.directionState == GameState.Direction.Right)
            {
                movePos = rightHandObj.transform.position;
            }
            else
            {
                movePos = leftHandObj.transform.position;
            }

            movePos = firstPos - movePos;

            movePos.x = startPos.x + movePos.z * 10f;
            yield return null;
        }

        state = AnimationState.Idle;
    }
}
