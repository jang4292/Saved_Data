using UnityEngine;
using Drawing;

namespace Enlargement
{
    public class TouchEnlargement : MonoBehaviour
    {
        public enum EnlargementState
        {
            Idle,
            Start
        }

        public enum MoveState
        {
            Idle,
            Left,
            Right,
            Up,
            Down
        }

        public EnlargementState mState;

        private MoveState moveState;

        public DrawingManager drawingManager; 

        private const float CAMERA_BASIC_SIZE = 5.4f;
        private Vector3 CAMERA_BASIC_POSITION { get { return new Vector3(0, 0, -10); } }

        private const float CAMERA_MAX_LEFT = -8f;
        private const float CAMERA_MAX_RIGHT = 5f;
        private const float CAMERA_MAX_UP = 5.1f;
        private const float CAMERA_MAX_DOWN = -5.05f;

        private float maxLeft = -8f;
        private float maxRight = 5f;
        private float maxUp = 5.1f;
        private float maxDown = -5.05f;

        public Camera imageCamera;
        public Camera uiCamera;
        public Camera toolCamera;

        public GameObject outlineCamera;

        public GameObject enlargementButtons;

        private Vector3 mPos;
        private float cameraPosSpeed = 0.25f;

        private float orthoZoomSpeed = 0.5f;        // 직교 모드에서 시야의 변화율

        void Start()
        {
            outlineCamera.SetActive(false);
            outlineCamera.SetActive(true);

            mPos = CAMERA_BASIC_POSITION;
        }

        void Update()
        {
            if(moveState != MoveState.Idle)
            {
                switch (moveState)
                {
                    case MoveState.Left:
                        if(mPos.x <= maxLeft)
                        {
                            moveState = MoveState.Idle;
                            return;
                        }
                        mPos.x -= cameraPosSpeed;
                        break;
                    case MoveState.Right:
                        if (mPos.x >= maxRight)
                        {
                            moveState = MoveState.Idle;
                            return;
                        }
                        mPos.x += cameraPosSpeed;
                        break;
                    case MoveState.Up:
                        if (mPos.y >= maxUp)
                        {
                            moveState = MoveState.Idle;
                            return;
                        }
                        mPos.y += cameraPosSpeed;
                        break;
                    case MoveState.Down:
                        if (mPos.y <= maxDown)
                        {
                            moveState = MoveState.Idle;
                            return;
                        }
                        mPos.y -= cameraPosSpeed;
                        break;
                }

                SetCameraPosition();
            }
        }
        
        public void SetIdleMoveState()
        {
            drawingManager.drawingState = DrawingManager.DrawingState.Playing;
            moveState = MoveState.Idle;
        }

        public void SetLeftMoveState()
        {
            drawingManager.drawingState = DrawingManager.DrawingState.Stop;
            moveState = MoveState.Left;
        }

        public void SetRightMoveState()
        {
            drawingManager.drawingState = DrawingManager.DrawingState.Stop;
            moveState = MoveState.Right;
        }

        public void SetUpMoveState()
        {
            drawingManager.drawingState = DrawingManager.DrawingState.Stop;
            moveState = MoveState.Up;
        }

        public void SetDownMoveState()
        {
            drawingManager.drawingState = DrawingManager.DrawingState.Stop;
            moveState = MoveState.Down;
        }

        public void SetCameraMaxPos(float cameraSize)
        {
            float distance = CAMERA_BASIC_SIZE - cameraSize;
            float percent = distance / CAMERA_BASIC_SIZE * 100;

            maxLeft = CAMERA_MAX_LEFT * percent / 100;
            maxRight = CAMERA_MAX_RIGHT * percent / 100;
            maxUp = CAMERA_MAX_UP * percent / 100;
            maxDown = CAMERA_MAX_DOWN * percent / 100;
        }

        private void SetCameraPosition()
        {
            imageCamera.transform.position = mPos;
            uiCamera.transform.position = mPos;
        }

        /// <summary>
        /// 처음 크기로 크기 조정
        /// </summary>
        public void SetReSizing()
        {
            mPos = CAMERA_BASIC_POSITION;
            imageCamera.transform.position = mPos;
            uiCamera.transform.position = mPos;
            imageCamera.orthographicSize = CAMERA_BASIC_SIZE;
            uiCamera.orthographicSize = CAMERA_BASIC_SIZE;
        }
    }
}
