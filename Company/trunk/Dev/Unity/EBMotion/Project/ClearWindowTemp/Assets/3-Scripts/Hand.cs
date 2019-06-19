using UnityEngine;
using System.Collections;
using System;
using BodySource;

namespace ClearWindow
{
    public enum MouseAction
    {
        UP,
        DOWN,
    }

    public delegate void OnMousePositionChanged(Vector3 position);
    public delegate void OnMouseAction(MouseAction action);

    public interface MouseListener
    {
        void OnMousePositionChanged(Vector3 position);
        void OnMouseAction(MouseAction action);
    }

    public class Hand : MonoBehaviour, KinectCallback, GameStatusListener
    {
        public AudioManager audio;

        private Vector3 previousPos = Vector3.zero;

        private Rect worldRect = new Rect(-6.7f, -5.3f, 13.4f, 10.6f);

        private static Hand _Instance;
        public static Hand Instance
        {
            get
            {
                if (!_Instance) _Instance = (Hand)FindObjectOfType(typeof(Hand));
                return _Instance;
            }
        }

        [Tooltip("당김 스프라이트")]
        public Sprite pullSprite;
        [Tooltip("밀기 스프라이트")]
        public Sprite pushSprite;
        [Tooltip("플레이 시 당김 스프라이트")]
        public Sprite cleanPullSprite;
        [Tooltip("플레이 시 밀기 스프라이트")]
        public Sprite cleanPushSprite;

        private SpriteRenderer sr;

        private GameStatus _gameStatus;
        private GameStatus gameStatus
        {
            get
            {
                return _gameStatus;
            }
            set
            {
                if (_gameStatus == value) return;
                if(value == GameStatus.PLAYING)
                {
                    sr.sprite = Motion == KinectMotion.PULL ? cleanPullSprite : cleanPushSprite;
                }
                else
                {
                    sr.sprite = Motion == KinectMotion.PULL ? pullSprite : pushSprite;
                }
                _gameStatus = value;
            }
        }

        private KinectMotion _Motion;
        private KinectMotion Motion
        {
            get
            {
                return _Motion;
            }
            set
            {
                if (_Motion == value) return;
                switch (value)
                {
                    case KinectMotion.PUSH:
                        sr.sprite = gameStatus == GameStatus.PLAYING ? cleanPushSprite : pushSprite;
                        _OnMouseAction(MouseAction.DOWN);
                        break;
                    case KinectMotion.PULL:
                        sr.sprite = gameStatus == GameStatus.PLAYING ? cleanPullSprite : pullSprite;
                        _OnMouseAction(MouseAction.UP);
                        break;
                }
                _Motion = value;
            }
        }

        private event OnMousePositionChanged _OnMousePositionChanged = delegate { };
        private event OnMouseAction _OnMouseAction = delegate { };

        [Tooltip("키넥트 연동 여부")]
        public bool withKinect;
        private bool prevWithKinect;

        void Awake()
        {
            _Instance = this;

            if (withKinect)
            {
                if (BodySourceView.Instance) BodySourceView.Instance.AddKinectCallback(this);
                prevWithKinect = withKinect;
            }

            if (GameManager.Instance) GameManager.Instance.AddGameStatusChangedListener(this);

            sr = GetComponent<SpriteRenderer>();
            if (!pullSprite) pullSprite = sr.sprite;

            OnMotionChanged(KinectMotion.PUSH);
        }

        void OnDestroy()
        {
            _Instance = null;

            if (withKinect)
            {
                if (BodySourceView.Instance) BodySourceView.Instance.RemoveKinectCallback(this);
            }

            if (GameManager.Instance) GameManager.Instance.RemoveGameStatusChangedListener(this);
        }

        

        IEnumerator Start()
        {
            while (true)
            {
                if(prevWithKinect != withKinect)
                {
                    if(withKinect)
                    {
                        if (BodySourceView.Instance) BodySourceView.Instance.AddKinectCallback(this);
                    }
                    else
                    {
                        if (BodySourceView.Instance) BodySourceView.Instance.RemoveKinectCallback(this);
                    }

                    prevWithKinect = withKinect;
                }

                if (!withKinect)
                {
                    //// Test
                    //if (Input.GetMouseButtonDown(0))
                    //{
                    //    OnMotionChanged(KinectMotion.PUSH);
                    //}
                    //else if (Input.GetMouseButtonUp(0))
                    //{
                    //    OnMotionChanged(KinectMotion.PULL);
                    //}

                    //var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Vector3 position = this.transform.position;

                    if (_gameStatus == GameStatus.PLAYING && previousPos != position)
                    {
                        if(worldRect.Contains(position)) audio.StartCleanSound();
                    }

                    previousPos = position;

                    position.z = 0;
                    OnPositionChanged(position);
                }

                yield return null;
            }
        }

        public void OnMotionChanged(KinectMotion motion)
        {
            Motion = motion;
        }

        public void OnPositionChanged(Vector3 position)
        {
            transform.position = position;
            _OnMousePositionChanged(position);
        }

        public void OnGameStatusChanged(GameStatus status)
        {
            gameStatus = status;
        }

        public void AddOnMouseListener(MouseListener listener)
        {
            _OnMousePositionChanged += listener.OnMousePositionChanged;
            _OnMouseAction += listener.OnMouseAction;
        }

        public void RemoveOnMouseListener(MouseListener listener)
        {
            _OnMousePositionChanged -= listener.OnMousePositionChanged;
            _OnMouseAction -= listener.OnMouseAction;
        }
    }
}
