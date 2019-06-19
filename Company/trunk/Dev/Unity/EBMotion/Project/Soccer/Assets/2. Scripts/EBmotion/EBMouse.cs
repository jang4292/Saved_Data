using UnityEngine;
using System.Collections;
using Soccer;

namespace EBMotion
{
    public class EBMouse : MonoBehaviour
    {
        private EbmDataParser ebmData;

        private float distance = 8;

        [Tooltip("오른쪽 발 오브젝트")]
        public GameObject rightObj = null;
        [Tooltip("왼쪽 발 오브젝트")]
        public GameObject leftObj = null;
        [Tooltip("게임 모션 상태 체크해줄 스크립트")]
        private GameManager gameManager = null;
        [Tooltip("처음 공 위치")]
        private Vector3 startPos = Vector3.zero;
        [Tooltip("발 위치")]
        private Vector3 firstPos = Vector3.zero;
        [Tooltip("움직이는 발 위치")]
        private Vector3 movePos = Vector3.zero;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            ebmData = GameObject.Find("EbmDataParser").GetComponent("EbmDataParser") as EbmDataParser;
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
            if(gameManager.state == GameManager.GameMotionState.Right)
            {
                movePos = rightObj.transform.position;
            }
            else
            {
                movePos = leftObj.transform.position;
            }

            
            movePos = firstPos - movePos;

            movePos.x = startPos.x;
            //Debug.Log("startPos.y :: " + startPos.y);
            //Debug.Log("movePos.y :: " + movePos.y);

            movePos.y = startPos.y + movePos.y;// * distance;
            movePos.z = startPos.z;

            //Debug.Log("movePost : " + movePos);
            transform.position = movePos;
            
            /*
            movePos = firstPos - movePos;

            movePos.x = movePos.x * 25f;
            movePos.y = movePos.y * 25f;
            //movePos.y = -movePos.y;// * distance;
            movePos.z = 0;

            Debug.Log("movePos :: " + movePos);
            transform.position = -movePos;
            */
        }

        // 캘리브레이션 후 시작 포지션 변경
        IEnumerator StartSetPos()
        {
            yield return new WaitForSeconds(1.5f);
            if(gameManager.state == GameManager.GameMotionState.Right)
            {
                firstPos = rightObj.transform.position;
                Debug.Log("firstPos :: " + firstPos);
            }
            else
            {
                firstPos = leftObj.transform.position;
            }
        }

    }
}
