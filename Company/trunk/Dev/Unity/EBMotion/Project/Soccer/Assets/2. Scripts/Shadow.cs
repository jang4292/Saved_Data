using UnityEngine;
using System.Collections;

namespace Soccer
{
    public class Shadow : Ball
    {
        [Tooltip("처음 오브젝트 위치")]
        private Vector3 firstPos = Vector3.zero;

        private GameObject ballObj = null;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            firstPos = transform.position;
        }

        void Start()
        {
            StartCoroutine(Move());
        }

        void Update()
        {
            if (Ball.state == BallState.Jump)
            {
                Destroy(gameObject);
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        // 그림자 움직임
        IEnumerator Move()
        {
            while (Ball.state == BallState.Idle)
            {
                // 공위치가 x -10이 되면 
                if (transform.position.x < -10f)
                {
                    SetStartPosition(firstPos);
                }
                else
                {
                    SetMovePosition();
                }

                yield return null;
            }
        }

        // 물체 이동
        void SetMovePosition()
        {
            if(ballObj == null)
            {
                ballObj = GameObject.Find("Action_Ball");
                if(ballObj == null)
                {
                    ballObj = GameObject.Find("Action_Ball(Clone)");
                }
            }

            Vector3 pos = transform.position;
            pos.x = ballObj.transform.position.x;
            transform.position = pos;
        }
    }
}
