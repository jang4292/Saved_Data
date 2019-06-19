using UnityEngine;
using System.Collections;
using EBMotion;

public class InputKick : MonoBehaviour
{
    private EbmDataParser ebmData;

    private float distance = 8f;

    [Tooltip("발 위치 오브젝트")]
    public GameObject rightFootObj = null;
    public GameObject leftFootObj = null;

    private Vector3 startPos = Vector3.zero;
    [Tooltip("처음 발 위치")]
    private Vector3 firstPos = Vector3.zero;
    [Tooltip("움직이는 발 위치")]
    private Vector3 movePos = Vector3.zero;

    void Start()
    {
        ebmData = GameObject.Find("EbmDataParser").GetComponent<EbmDataParser>();
        StartCoroutine(StartSetPos());
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (ebmData.EbmPort.IsOpen)
        {
            MovePosition();
        }
    }

    void MovePosition()
    {
        if (GameState.directionState == GameState.Direction.Right)
        {
            movePos = rightFootObj.transform.position;
        }
        else
        {
            movePos = leftFootObj.transform.position;
        }

        movePos = firstPos - movePos;

        movePos.x = startPos.x;
        movePos.y = startPos.y + movePos.z * distance;
        movePos.z = startPos.z;


        transform.position = movePos;
    }

    // 캘리브레이션 후 시작 포지션 변경
    IEnumerator StartSetPos()
    {
        yield return new WaitForSeconds(3f);
        if (GameState.directionState == GameState.Direction.Right)
        {
            firstPos = rightFootObj.transform.position;
        }
        else
        {
            firstPos = leftFootObj.transform.position;
        }

    }
}
