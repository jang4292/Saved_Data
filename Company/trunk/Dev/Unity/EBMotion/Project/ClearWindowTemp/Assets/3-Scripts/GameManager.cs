using UnityEngine;
using System.Collections;
using System;

namespace ClearWindow
{
    public enum GameStatus
    {
        NONE,
        PLAYING,
        END_SUCCESS,
        END_FAIL,
    }

    public delegate void OnGameStatusChanged(GameStatus status);

    public interface GameStatusListener
    {
        void OnGameStatusChanged(GameStatus status);
    }

    public class GameManager : MonoBehaviour, TimeListener, OnClearWindowListener
    {
        public enum MotionState
        {
            Right,
            Left
        }

        [Tooltip("모션 구분")]
        public MotionState state;

        public static GameManager _Instance;
        public static GameManager Instance
        {
            get
            {
                if (!_Instance)
                {
                    _Instance = (GameManager)FindObjectOfType(typeof(GameManager));
                }

                return _Instance;
            }
        }

        private event OnGameStatusChanged _OnGameStatusChanged = delegate { };

        private GameStatus _Status;
        public GameStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (_Status == value) return;
                _OnGameStatusChanged(value);
                switch (value)
                {
                    case GameStatus.END_SUCCESS:
                        Instantiate(resultPopupPrefab);
                        break;
                    case GameStatus.END_FAIL:
                        Instantiate(resultPopupPrefab).GetComponent<ResultPopup>().SetSuccess(false);
                        break;
                }
                _Status = value;
            }
        }

        public GameObject resultPopupPrefab;

        void Awake()
        {
            _Instance = this;

            float height = Screen.width / 16f * 9;
            Screen.SetResolution(Screen.width, (int)(height / 1f) + (height % 1f > 0 ? 1 : 0), true);

            if (MaskCamera.Instance) MaskCamera.Instance.AddOnClearWindowListener(this);
            if (Timer.Instance) Timer.Instance.AddTimeListener(this);
        }

        void Start()
        {
            StartCoroutine(StartGame());
        }

        void OnDestroy()
        {
            if (MaskCamera.Instance) MaskCamera.Instance.RemoveOnClearWindowListener(this);
            if (Timer.Instance) Timer.Instance.RemoveTimeListener(this);

            _Instance = null;
        }

        public void AddGameStatusChangedListener(GameStatusListener listener)
        {
            _OnGameStatusChanged += listener.OnGameStatusChanged;
        }

        public void RemoveGameStatusChangedListener(GameStatusListener listener)
        {
            _OnGameStatusChanged -= listener.OnGameStatusChanged;
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(2f);
            Status = GameStatus.PLAYING;
        }

        public void OnTimeOut()
        {
            Status = GameStatus.NONE;
            Status = GameStatus.PLAYING;
        }

        public void OnClearWindow(int clear)
        {
            //Status = GameStatus.END_SUCCESS;
        }
    }
}