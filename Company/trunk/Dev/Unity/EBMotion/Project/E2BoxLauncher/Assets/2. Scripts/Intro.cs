using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace Launcher
{
    interface OnClickListener
    {
        void OnClick();
    }

    public class Intro : MonoBehaviour, OnClickListener
    {
        
        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            // 스크린 비율 16:9
            float _height = Screen.width / 16f * 9;

            Screen.SetResolution(Screen.width, (_height % 1f > 0) ? Mathf.RoundToInt(_height) : (int)_height, true);

            // 실행시 마우스 커서 숨기기
            Cursor.visible = false;
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Reference Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        public void OnClick()
        {
            SceneManager.LoadScene("Main");
        }
    }

}
