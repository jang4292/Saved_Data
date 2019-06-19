using UnityEngine;
using System.Collections;
using EBMotion;

namespace BaseBall
{
    public class EBMouse : MonoBehaviour
    {
        private EbmDataParser ebmData;

        [Tooltip("손 오브젝트")]
        public GameObject rightHandObj = null;
        public GameObject leftHandObj = null;

        [Tooltip("처음 손 위치")]
        private Vector3 firstPos = Vector3.zero;
        [Tooltip("움직이는 손 위치")]
        private Vector3 movePos = Vector3.zero;

        void Start()
        {
            ebmData = GameObject.Find("EbmDataParser").GetComponent<EbmDataParser>();
            StartCoroutine(StartSetPos());
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
            if(GameState.directionState == GameState.Direction.Right)
            {
                movePos = rightHandObj.transform.position;
            }
            else
            {
                movePos = leftHandObj.transform.position;
            }

            movePos = firstPos - movePos;

            movePos.x = movePos.x * 25f;
            movePos.y = movePos.y * 25f;
            movePos.z = 0;

            transform.position = -movePos;
        }

        // 캘리브레이션 후 시작 포지션 변경
        IEnumerator StartSetPos()
        {
            yield return new WaitForSeconds(3f);
            if(GameState.directionState == GameState.Direction.Right)
            {
                firstPos = rightHandObj.transform.position;
            }
            else
            {
                firstPos = leftHandObj.transform.position;
            }
            
        }
    }
}
