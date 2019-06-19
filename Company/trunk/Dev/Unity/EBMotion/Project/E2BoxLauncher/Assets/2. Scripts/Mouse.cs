using UnityEngine;
using System.Collections;
using System;

namespace Launcher
{
    public enum MouseState
    {
        Idle,
        Down,
        Up
    }

    public class Mouse : MonoBehaviour
    {
        private SpriteRenderer spriteRen = null;

        [SerializeField]
        private Sprite clickSprite = null;
        [SerializeField]
        private Sprite notClickSprite = null;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            if (spriteRen == null) spriteRen = transform.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;

            if (Input.GetMouseButtonDown(0))
            {
                SetSpriteChange(clickSprite);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                SetSpriteChange(notClickSprite);
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void SetSpriteChange(Sprite sprite)
        {
            spriteRen.sprite = sprite;
        }
    }

}
