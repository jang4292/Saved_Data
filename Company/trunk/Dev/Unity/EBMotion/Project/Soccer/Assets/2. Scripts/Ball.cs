using UnityEngine;
using System.Collections;

namespace Soccer
{
    public enum BallState
    {
        Idle,
        Jump,
        Count,
        Stop
    }

    public class Ball : MonoBehaviour
    {
        private AudioManager audio;

        [Tooltip("처음 공 위치")]
        private Vector3 startPos = Vector3.zero;

        [Tooltip("공 축소하는 스피드")]
        private float removeSpeed = 20f;

        [Tooltip("물리적효과")]
        private new Rigidbody rigidbody = null;

        [Tooltip("공 상태")]
        [System.NonSerialized]
        public static BallState state;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            audio = GameObject.Find("Audio").GetComponent<AudioManager>();
            state = BallState.Idle;
            startPos = transform.position;
        }

        void Start()
        {
            StartCoroutine(Move());

            rigidbody = transform.GetComponent<Rigidbody>();
            rigidbody.AddForce(Vector3.left * 5, ForceMode.Impulse);
        }

        // 충돌 이벤트
        void OnCollisionEnter(Collision other)
        {
            if (other.collider.tag.Equals("Player"))
            {
                if (state != BallState.Jump)
                {
                    StartCoroutine(SetBallRemoveSize());
                    state = BallState.Jump;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Wall"))
            {
                state = BallState.Count;
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        // 공 움직임
        IEnumerator Move()
        {
            while (state == BallState.Idle)
            {
                // 공위치가 x -10이 되면 
                if (transform.position.x < -10f)
                {
                    SetStartPosition(startPos);
                }

                yield return null;
            }
        }

        // 공 축소
        IEnumerator SetBallRemoveSize()
        {
            audio.StartKickSound();

            while (transform.localScale.x > 0f)
            {
                Vector3 scale = transform.localScale;
                scale.x -= removeSpeed * Time.deltaTime;
                scale.y -= removeSpeed * Time.deltaTime;
                scale.z -= removeSpeed * Time.deltaTime;
                transform.localScale = scale;

                if(transform.lossyScale.x < 0f)
                {
                    state = BallState.Stop;

                    Destroy(gameObject);

                    GameObject ball = Resources.Load("Prefabs/Action_Ball") as GameObject;
                    GameObject shadow = Resources.Load("Prefabs/Ball_Shadow") as GameObject;

                    Instantiate(ball, ball.transform.position, ball.transform.rotation);
                    Instantiate(shadow, shadow.transform.position, shadow.transform.rotation);

                }

                yield return null;
            }
        }

        // 처음 포지션으로 
        public void SetStartPosition(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}
