using UnityEngine;

namespace Drawing
{
    public class Mouse : MonoBehaviour
    {

        public enum MouseState
        {
            Idle,
            StartClick,
            Click,
            NotClick,
        }

        public MouseState state;

        [Tooltip("Tool 상태를 보여줄 스크립트")]
        private ToolBoxSmallBox tool;

        public Camera imageCamera;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            if (tool == null) tool = GameObject.Find("small_icon").GetComponent<ToolBoxSmallBox>();
        }

        void Update()
        {
            Vector3 mousePos = imageCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;

            SetMouseState();
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//
        
        // 마우스 상태 변경
        void SetMouseState()
        {
            // 삼성 노트 S pen 사용 시 플랫폼은 Android이나, Input은 Mouse로 받는다

            //Mobile Platform
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    state = MouseState.StartClick;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    state = MouseState.Click;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    state = MouseState.NotClick;
                }
            }
            //Others
            else if (Input.GetMouseButtonDown(0))
            {
                state = MouseState.StartClick;
            }
            else if (Input.GetMouseButton(0))
            {
                state = MouseState.Click;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                state = MouseState.NotClick;
            }
        }
    }
}


