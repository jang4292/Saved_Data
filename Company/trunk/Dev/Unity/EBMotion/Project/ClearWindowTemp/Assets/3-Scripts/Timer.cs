using UnityEngine;
using System;
using System.Collections;

namespace ClearWindow
{
    public delegate void OnTimeOut();

    public interface TimeListener
    {
        void OnTimeOut();
    }

    public class Timer : MonoBehaviour, GameStatusListener
    {
        public AudioManager audio;

        private static Timer _Instance;
        public static Timer Instance
        {
            get
            {
                if (!_Instance) _Instance = (Timer)FindObjectOfType(typeof(Timer));
                return _Instance;
            }
        }

        [Tooltip("숫자(0~9) 스프라이트")]
        public Sprite[] numberSprites;

        [Tooltip("분(십의 자리)")]
        private SpriteRenderer minuteTens;
        [Tooltip("분(일의 자리)")]
        private SpriteRenderer minuteUnits;
        [Tooltip("초(십의 자리)")]
        private SpriteRenderer secondTens;
        [Tooltip("초(일의 자리")]
        private SpriteRenderer secondUnits;

        [Tooltip("게임 시간(초)")]
        public int maxTime;
        private int curTime;

        private IEnumerator tickTock;

        private event OnTimeOut _OnTimeOut = delegate { };

        void Awake()
        {
            _Instance = this;

            var child = transform.GetChild(0);
            minuteTens = child.FindChild("MinuteTens").GetComponent<SpriteRenderer>();
            minuteUnits = child.FindChild("MinuteUnits").GetComponent<SpriteRenderer>();
            secondTens = child.FindChild("SecondTens").GetComponent<SpriteRenderer>();
            secondUnits = child.FindChild("SecondUnits").GetComponent<SpriteRenderer>();

            if (GameManager.Instance) GameManager.Instance.AddGameStatusChangedListener(this);
        }

        void OnDestroy()
        {
            if (GameManager.Instance) GameManager.Instance.RemoveGameStatusChangedListener(this);

            _Instance = null;
        }

        IEnumerator TickTock()
        {
            audio.StartEndSound();
            curTime = maxTime < 0 ? 0 : maxTime;

            do
            {
                var time = TimeSpan.FromSeconds(curTime);
                minuteTens.sprite = numberSprites[time.Minutes / 10];
                minuteUnits.sprite = numberSprites[time.Minutes % 10];
                secondTens.sprite = numberSprites[time.Seconds / 10];
                secondUnits.sprite = numberSprites[time.Seconds % 10];

                curTime -= 1;

                yield return new WaitForSeconds(1f);
            }
            while (curTime >= 0);

            _OnTimeOut();
        }

        public void OnGameStatusChanged(GameStatus status)
        {
            if (status == GameStatus.PLAYING) StartCoroutine(tickTock = TickTock());
            else if (status == GameStatus.END_SUCCESS) StopCoroutine(tickTock);
        }

        public void AddTimeListener(TimeListener listener)
        {
            _OnTimeOut += listener.OnTimeOut;
        }

        public void RemoveTimeListener(TimeListener listener)
        {
            _OnTimeOut -= listener.OnTimeOut;
        }
    }
}
