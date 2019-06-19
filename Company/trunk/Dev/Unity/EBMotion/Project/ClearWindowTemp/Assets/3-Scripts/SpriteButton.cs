using UnityEngine;
using System.Collections;
using System;

namespace ClearWindow
{
    public delegate void OnClick();
    public interface OnClickListener
    {
        void OnClick();
    }

    public class SpriteButton : MonoBehaviour, MouseListener
    {

        public Sprite defaultSprite;
        public Sprite pressedSprite;
        private SpriteRenderer sr;
        private Bounds bounds;

        private event OnClick onClick = delegate { };
        public GameObject[] onClickListeners;

        private MouseAction _mouseAction;
        private MouseAction mouseAction
        {
            get
            {
                return _mouseAction;
            }
            set
            {
                if (_mouseAction == value) return;
                switch (value)
                {
                    case MouseAction.DOWN:
                        sr.sprite = pressedSprite;
                        break;
                    case MouseAction.UP:
                        sr.sprite = defaultSprite;
                        break;
                }
                _mouseAction = value;
            }
        }
        private bool mouseOver;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr)
            {
                if (sr.sprite) defaultSprite = sr.sprite;
                else if (defaultSprite) sr.sprite = defaultSprite;

                bounds = sr.bounds;
            }

            foreach (GameObject obj in onClickListeners)
            {
                var listener = obj.GetComponent<OnClickListener>();
                if (listener != null) onClick += listener.OnClick;
            }

            if (Hand.Instance) Hand.Instance.AddOnMouseListener(this);
        }

        void OnDestroy()
        {
            if (Hand.Instance) Hand.Instance.RemoveOnMouseListener(this);
        }

        public void OnMousePositionChanged(Vector3 position)
        {
            position.z = 0;
            mouseOver = MouseOver(position);
        }

        public void OnMouseAction(MouseAction action)
        {
            if(mouseOver)
            {
                if (MouseDown(action))
                {
                    mouseAction = MouseAction.DOWN;
                }
                else if(MouseUp(action))
                {
                    mouseAction = MouseAction.UP;
                    onClick();
                }
            }
            else
            {
                if (MouseUp(action))
                {
                    mouseAction = MouseAction.UP;
                }
            }
        }

        bool MouseOver(Vector3 position)
        {
            return bounds.Contains(position);
        }

        bool MouseDown(MouseAction action)
        {
            return action == MouseAction.DOWN && mouseAction != MouseAction.DOWN;
        }

        bool MouseUp(MouseAction action)
        {
            return action == MouseAction.UP && mouseAction == MouseAction.DOWN;
        }

        public void AddOnClickListener(OnClickListener listener)
        {
            onClick += listener.OnClick;
        }
    }
}