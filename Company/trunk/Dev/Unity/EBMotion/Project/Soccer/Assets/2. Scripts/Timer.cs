using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Soccer
{
    public enum TimerState
    {
        Idle,
        Start,
        Stop
    }
    public class Timer : MonoBehaviour
    {
        public AudioManager audio;

        [Tooltip("시작 시간")]
        public float startTime = 0;

        [Tooltip("시간")]
        private float time = 0;

        private Text timeText = null;

        public TimerState state;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            timeText = transform.GetComponent<Text>();

            time = startTime;
            SetTimeTextChange(time);
            state = TimerState.Start;

            StartCoroutine(RemoveTime());
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        // 시간 감소
        public IEnumerator RemoveTime()
        {
            yield return new WaitForSeconds(1f);

            while (true)
            {
                if(time < 0)
                {
                    audio.StartEndSound();
                    state = TimerState.Stop;
                    time = startTime;
                }

                time -= Time.deltaTime;

                SetTimeTextChange(time);
                
                yield return null;
            }
        }

        void SetTimeTextChange(float number)
        {
            if (time > 10)
            {
                timeText.text = "00 : " + (int)time;
            }
            else
            {
                timeText.text = "00 : 0" + (int)time;
            }
        }
    }
}
