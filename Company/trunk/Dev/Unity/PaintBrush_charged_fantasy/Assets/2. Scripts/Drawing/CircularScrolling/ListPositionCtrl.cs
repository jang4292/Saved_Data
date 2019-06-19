/* Calculate and assign the final position for each ListBoxes.
 *
 * There are three controling modes:
 * 1. Free moving: Control the listBoxes with finger or mouse.
 *    You don't know where the ListBox would stop at.
 * 2. Align to center: It's the same as free moving
 *    but there always has a listBox positioning at the center.
 * 3. Control by button: Control the listBoxes by button on the screen.
 *    There always has a listBox positioning at the center.
 *
 * Author: LanKuDot <airlanser@gmail.com>
 *
 * As long as you retain this notice you can do whatever you want with this stuff.
 * If we meet some day, and you think this stuff is worth it,
 * you can buy me a coffee in return. LanKuDot
 */
using UnityEngine;
using UnityEngine.UI;

namespace CircularScrolling
{
    public class ListPositionCtrl : MonoBehaviour
    {
        public enum Direction
        {
            VERTICAL,
            HORIZONTAL
        };

        public static ListPositionCtrl Instance;
        /* 초기 설정.
         * 모드 controlByButton alignToCenter
         * ------------------------------------------------- -
         * 무료 거짓 거짓 이동
         * 허위 사실의 중심을 맞 춥니 다
         * 진정한 BTN에 의한 제어는 상관 없어
         */
        public bool controlByButton = false;
        public bool alignToCenter = false;

        public ListBox[] listBoxes;
        public Vector2 centerPos;

        public Button[] buttons;

        public Direction direction = Direction.VERTICAL;
        // 3D 카메라를 들어, 캔버스 평면과 카메라 사이의 거리.
        public float canvasDistance = 100.0f;
        // 각 목록 상자 사이의 거리를 설정합니다. 가까이, 더 큰.
        public float divideFactor = 2.0f;
        // 슬라이딩 기간을 설정합니다. 길수록 크게.
        public int slidingFrames = 35;
        // 슬라이딩 속도를 설정합니다. 빨리, 더 큰.
        [Range(0.0f, 1.0f)]
        public float slidingFactor = 0.2f;
        // 오른쪽으로 왼쪽으로 이동 목록 커브를 설정하거나 위 / 아래 수평 모드입니다.
        // 긍정적 : (최대) 오른쪽에 곡선; 음 : 곡선 (아래) 왼쪽으로.
        [Range(-1.0f, 1.0f)]
        public float angularity = 0.2f;
        // 중앙 목록 상자의 크기의 양을 설정합니다.
        public float scaleFactor = 0.05f;

        private bool isTouchingDevice;

        private Vector3 lastInputWorldPos;
        private Vector3 currentInputWorldPos;
        private Vector3 deltaInputWorldPos;

        /// <summary>
        /// 클릭 체크를 표시해줄 논리값
        /// </summary>
        private bool clickCheck = false;

        /// <summary>
        /// 범위 체크
        /// </summary>
        public ListArea area;

        public Camera toolCamera;

        void Awake()
        {
            Instance = this;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    //isTouchingDevice = false;
                    isTouchingDevice = false;
                    break;
                case RuntimePlatform.Android:
                    //isTouchingDevice = true;
                    isTouchingDevice = false;
                    break;
            }
        }

        void Start()
        {
            if (!controlByButton)
                foreach (Button button in buttons)
                    button.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!controlByButton)
            {
                if (!isTouchingDevice)
                    storeMousePosition();
                else
                    storeFingerPosition();
            }
        }

        /* 플레이어가 마우스 왼쪽 버튼을 클릭 할 때 마우스의 위치를 저장합니다.
         */
        void storeMousePosition()
        {
            if (area.state == ListArea.ListAreaState.In)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastInputWorldPos = toolCamera.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvasDistance));

                    clickCheck = true;
                }

                if (clickCheck && Input.GetMouseButton(0))
                {
                    currentInputWorldPos = toolCamera.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvasDistance));
                    deltaInputWorldPos = currentInputWorldPos - lastInputWorldPos;
                    foreach (ListBox listbox in listBoxes)
                        listbox.updatePosition(deltaInputWorldPos / transform.parent.localScale.x);

                    lastInputWorldPos = currentInputWorldPos;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                setSlidingEffect();
                clickCheck = false;
            }
        }

        /* 모바일에 감동의 위치를 저장합니다.
         */
        void storeFingerPosition()
        {
            if (Input.touchCount > 0)
            {
                if (area.state == ListArea.ListAreaState.In)
                {

                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        lastInputWorldPos = toolCamera.ScreenToWorldPoint(
                            new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, canvasDistance));

                        clickCheck = true;
                    }
                    if (clickCheck && Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        currentInputWorldPos = toolCamera.ScreenToWorldPoint(
                            new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, canvasDistance));
                        deltaInputWorldPos = currentInputWorldPos - lastInputWorldPos;
                        foreach (ListBox listbox in listBoxes)
                            listbox.updatePosition(deltaInputWorldPos / transform.parent.localScale.x);

                        lastInputWorldPos = currentInputWorldPos;
                    }
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    setSlidingEffect();

                    clickCheck = false;
                }
            }
        }

        /* 감동적인이 종료되면, 밀어 거리를 계산하고 
         * 목록 상자에 할당합니다.
         */
        void setSlidingEffect()
        {
            Vector3 deltaPos = deltaInputWorldPos / transform.parent.localScale.x;

            if (alignToCenter)
                deltaPos = findDeltaPositionToCenter();

            foreach (ListBox listbox in listBoxes)
                listbox.setSlidingDistance(deltaPos);
        }

        /* 중앙 위치에 가장 가까운 목록 상자 찾기
         * 그리고 그들 사이의 X 또는 Y의 델타 위치를 계산한다.
         */
        Vector3 findDeltaPositionToCenter()
        {
            float minDeltaPos = 99999.9f;
            float deltaPos;

            switch (direction)
            {
                case Direction.VERTICAL:
                    foreach (ListBox listBox in listBoxes)
                    {
                        deltaPos = centerPos.y - listBox.transform.localPosition.y;
                        if (Mathf.Abs(deltaPos) < Mathf.Abs(minDeltaPos))
                            minDeltaPos = deltaPos;
                    }

                    return new Vector3(0.0f, minDeltaPos, 0.0f);

                case Direction.HORIZONTAL:
                    foreach (ListBox listBox in listBoxes)
                    {
                        deltaPos = centerPos.x - listBox.transform.localPosition.x;
                        if (Mathf.Abs(deltaPos) < Mathf.Abs(minDeltaPos))
                            minDeltaPos = deltaPos;
                    }

                    return new Vector3(minDeltaPos, 0.0f, 0.0f);

                default:
                    return Vector3.zero;
            }
        }

        /* 버튼 컨트롤을 사용할 수 있습니다! 
         * 다음 내용 버튼을 누르면, 
         * 모든 목록 상자 1 단위를 위로 이동합니다.
         */
        public void nextContent()
        {
            foreach (ListBox listbox in listBoxes)
                listbox.unitMove(1, true);
        }

        /* 버튼 컨트롤을 사용할 수 있습니다! 
         * 마지막 내용 버튼을 누르면, 
         * 모든 목록 상자 1 장치를 아래로 이동합니다.
         */
        public void lastContent()
        {
            foreach (ListBox listbox in listBoxes)
                listbox.unitMove(1, false);
        }
    }
}
