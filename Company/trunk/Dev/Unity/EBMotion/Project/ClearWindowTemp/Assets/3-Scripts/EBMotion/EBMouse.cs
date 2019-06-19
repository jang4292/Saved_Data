using UnityEngine;
using ClearWindow;
using System.Collections;

namespace EBMotion
{
    public class EBMouse : MonoBehaviour
    {
        private EbmDataParser ebmData;

        [Tooltip("손 오브젝트")]
        public GameObject rightHandObj = null;
        public GameObject leftHandObj = null;

        [Tooltip("Motion 구분")]
        private GameManager gameManager = null;

        [Tooltip("처음 손 위치")]
        private Vector3 firstPos = Vector3.zero;

        [Tooltip("처음 핸드 위치")]
        private Vector3 startPos = Vector3.zero;

        [Tooltip("움직이는 손 위치")]
        private Vector3 movePos = Vector3.zero;

        /// <summary>
        /// Y값 마이너스
        /// </summary>
        public bool isMinus = false;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            ebmData = GameObject.Find("EbmDataParser").GetComponent("EbmDataParser") as EbmDataParser;
            StartCoroutine(StartSetPos());
            startPos = transform.position;
        }

        void Update()
        {
            if (ebmData.EbmPort.IsOpen)
            {
                MovePosition();
            }
        }

        void MovePosition()
        {
            if(gameManager.state == GameManager.MotionState.Right)
            {
                movePos = rightHandObj.transform.position;
            }
            else
            {
                movePos = leftHandObj.transform.position;
            }

            
            movePos = firstPos - movePos;
            /*

            //movePos.x = -3 + (movePos.x * 25f);
            //movePos.y = -4 + (movePos.y * 25f);

            //movePos.x = movePos.x * 25f;

            movePos.x = movePos.x * 10f;

            //if (isMinus)
            //{
            //  movePos.y = movePos.y * -25f;
            // }
            //else
            //{
            //movePos.y = movePos.y * 25f;
            //}

            movePos.y = movePos.y * -1.5f;


            movePos.z = 0;

            Debug.Log("movePos :: " + movePos);
            //transform.position = -movePos;
            transform.position = -movePos;
            */


            /*
            movePos = firstPos - movePos;

            movePos.x = movePos.x * 25f;
            movePos.y = movePos.y * 25f;
            movePos.z = 0;

            transform.position = -movePos;
            */


            Debug.Log("movePos :: " + movePos);

            movePos.x = startPos.x - movePos.x * 1.5f;
            Debug.Log("startPos.y :: " + startPos.y);
            Debug.Log("movePos.y :: " + movePos.y);

            movePos.y = startPos.y + movePos.y;// * distance;
            movePos.z = 0;

            //Debug.Log("movePost : " + movePos);
            transform.position = movePos;
            //firstPos = movePos;


        }

        // 캘리브레이션 후 시작 포지션 변경
        IEnumerator StartSetPos()
        {
            yield return new WaitForSeconds(1.5f);
            if(gameManager.state == GameManager.MotionState.Right)
            {
                firstPos = rightHandObj.transform.position;
                Debug.Log("firstPos :: " + firstPos);
                //transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                firstPos = leftHandObj.transform.position;
                //transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            
        }
    }
}
