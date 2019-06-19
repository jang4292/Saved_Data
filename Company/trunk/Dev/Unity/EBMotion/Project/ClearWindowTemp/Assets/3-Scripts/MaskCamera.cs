using UnityEngine;
using System.Collections.Generic;
using ClearWindow;
using System;

namespace ClearWindow
{
    public delegate void OnClearWindow(int clear);
    public interface OnClearWindowListener
    {
        void OnClearWindow(int clear);
    }

    public class MaskCamera : MonoBehaviour, GameStatusListener, MouseListener
    {
        private static MaskCamera _Instance;
        public static MaskCamera Instance
        {
            get
            {
                if (!_Instance) _Instance = (MaskCamera)FindObjectOfType(typeof(MaskCamera));
                return _Instance;
            }
        }

        public Material EraserMaterial;
        private bool firstFrame;
        private Vector2? newHolePosition;

        private GameStatus gameStatus;

        private MouseAction mouseAction;
        private Vector3 mousePosition;

        private Rect worldRect = new Rect(-6.7f, -5.3f, 13.4f, 10.6f);

        public SpriteRenderer dirtyWindow;
        private Bounds dirtyBounds;
        private Vector2 factor;
        private int xCnt, yCnt;
        private bool[,] checker;
        private float clearPercentage
        {
            get
            {
                var cnt = 0;
                for (var x = 0; x < xCnt; x++)
                {
                    for (var y = 0; y < yCnt; y++)
                    {
                        if (checker[x, y]) cnt++;
                    }
                }
                return cnt * 100f / checker.Length;
            }
        }

        private event OnClearWindow _OnClearWindow = delegate { };

        void Awake()
        {
            _Instance = this;

            if (GameManager.Instance) GameManager.Instance.AddGameStatusChangedListener(this);

            if (Hand.Instance) Hand.Instance.AddOnMouseListener(this);

            if (dirtyWindow)
            {
                dirtyBounds = dirtyWindow.bounds;

                factor = new Vector2(Mathf.Abs(dirtyBounds.min.x), Math.Abs(dirtyBounds.min.y));
                var max = dirtyBounds.max;
                xCnt = (int)((max.x + factor.x) * 10);
                yCnt = (int)((max.y + factor.y) * 10);
                checker = new bool[xCnt, yCnt];
            }
        }

        void OnDestroy()
        {
            if (GameManager.Instance) GameManager.Instance.RemoveGameStatusChangedListener(this);

            if (Hand.Instance) Hand.Instance.RemoveOnMouseListener(this);

            _Instance = null;
        }

        public void Start()
        {
            firstFrame = true;
        }

        public void Update()
        {
            newHolePosition = null;
            mouseAction = MouseAction.DOWN;
            if (gameStatus == GameStatus.PLAYING && mouseAction == MouseAction.DOWN)
            {
                //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (worldRect.Contains(mousePosition))
                    newHolePosition = new Vector2(1201f * (mousePosition.x - worldRect.xMin) / worldRect.width, 947f * (mousePosition.y - worldRect.yMin) / worldRect.height);
            }
        }

        public void OnPostRender()
        {
            if (firstFrame)
            {
                firstFrame = false;
                GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
            }
            if (newHolePosition != null)
                CutHole(new Vector2(1201f, 947f), newHolePosition.Value);
        }

        private void CutHole(Vector2 imageSize, Vector2 imageLocalPosition)
        {
            Rect textureRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            Rect positionRect = new Rect(
                (imageLocalPosition.x - 0.5f * EraserMaterial.mainTexture.width) / imageSize.x,
                (imageLocalPosition.y - 0.5f * EraserMaterial.mainTexture.height) / imageSize.y,
                EraserMaterial.mainTexture.width / imageSize.x,
                EraserMaterial.mainTexture.height / imageSize.y
            );

            ClearStatus(positionRect);

            GL.PushMatrix();
            GL.LoadOrtho();
            for (int i = 0; i < EraserMaterial.passCount; i++)
            {
                EraserMaterial.SetPass(i);
                GL.Begin(GL.QUADS);
                GL.Color(Color.white);
                GL.TexCoord2(textureRect.xMin, textureRect.yMax);
                GL.Vertex3(positionRect.xMin, positionRect.yMax, 0.0f);
                GL.TexCoord2(textureRect.xMax, textureRect.yMax);
                GL.Vertex3(positionRect.xMax, positionRect.yMax, 0.0f);
                GL.TexCoord2(textureRect.xMax, textureRect.yMin);
                GL.Vertex3(positionRect.xMax, positionRect.yMin, 0.0f);
                GL.TexCoord2(textureRect.xMin, textureRect.yMin);
                GL.Vertex3(positionRect.xMin, positionRect.yMin, 0.0f);
                GL.End();
            }
            GL.PopMatrix();
        }

        public void OnGameStatusChanged(GameStatus status)
        {
            gameStatus = status;

            if (status == GameStatus.PLAYING)
            {
                checker = new bool[xCnt, yCnt];
                firstFrame = true;
            } 
        }

        public void OnMousePositionChanged(Vector3 position)
        {
            mousePosition = position;
        }

        public void OnMouseAction(MouseAction action)
        {
            mouseAction = action;
        }

        void ClearStatus(Rect rect)
        {
            if (!dirtyWindow) return;

            if (dirtyBounds.Contains(mousePosition))
            {
                var x = (int)((mousePosition.x + factor.x) * 10);
                var y = (int)((mousePosition.y + factor.y) * 10);

                for (var i = x - 6; i <= x + 6; i++)
                {
                    for (var j = y - 6; j <= y + 6; j++)
                    {
                        if (i < 0 || i >= xCnt || j < 0 || j >= yCnt) continue;
                        checker[i, j] = true;
                    }
                }

                var percentage = clearPercentage;

                if (percentage > 99.999)
                {
                    _OnClearWindow((int)percentage);
                }
            }
        }

        public void AddOnClearWindowListener(OnClearWindowListener listener)
        {
            _OnClearWindow += listener.OnClearWindow;
        }

        public void RemoveOnClearWindowListener(OnClearWindowListener listener)
        {
            _OnClearWindow -= listener.OnClearWindow;
        }
    }
}