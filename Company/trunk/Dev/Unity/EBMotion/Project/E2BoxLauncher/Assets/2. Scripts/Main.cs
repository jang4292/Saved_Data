using UnityEngine;
using System.Collections;

namespace Launcher
{
    public class Main : MonoBehaviour, OnClickListener
    {
        private string nowDirectory = "";

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            // 현재 실행한 프로그램의 디렉토리 반환
            nowDirectory = System.Environment.CurrentDirectory;
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Reference Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        public void OnClick()
        {
            try
            {
                System.Diagnostics.Process.Start(nowDirectory + "/" + name + "/" + name + ".exe");
                Application.Quit();
            }
            catch
            {
                Debug.Log("지정된 파일을 찾을 수 없습니다.");
            }
        }

    }
}

