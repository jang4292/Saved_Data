/* The basic component of scrolling list.
 * Control the position of the list element.
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
    public class ListBox : MonoBehaviour
    {
        public int listBoxID;   // Must be unique, and count from 0

        public ListBox lastListBox;
        public ListBox nextListBox;

        private int numOfListBox;
        private int contentID;

        /// <summary>
        /// 최대값
        /// </summary>
        private Vector2 canvasMaxPos_L;
        private Vector2 unitPos_L;
        private Vector2 lowerBoundPos_L;
        private Vector2 upperBoundPos_L;
        private Vector2 rangeBoundPos_L;
        private Vector2 shiftBoundPos_L;

        private Vector3 slidingDistance_L;  // The sliding distance at each frame
        private Vector3 slidingDistanceLeft_L;

        private Vector3 originalLocalScale;

        private bool keepSliding = false;
        private int slidingFrames;

        void Start()
        {
            /// Item 길이
            numOfListBox = ListPositionCtrl.Instance.listBoxes.Length;

            /* 최소 위치이며, (0,0) 좌표 카메라의 왼쪽 하단 모서리에 있습니다
             * 최대 위치는 카메라의 오른쪽 상단 모서리에 있습니다. 사시도를 들어,
             * 우리는 계정에 캔버스 평면과 카메라 사이의 거리를 가지고해야합니다. */

            /// 월드좌표로 변환한 최대위치 - 최대위치
            /// ListPositionCtrl.Instance.canvasDistance = 100, 캔버스 평면과 카메라 사이의 거리
            canvasMaxPos_L = Camera.main.ScreenToWorldPoint(
                new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, ListPositionCtrl.Instance.canvasDistance)) -
                Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, ListPositionCtrl.Instance.canvasDistance));

            /* 캔버스 평면의 원점 캔버스 평면의 중심에 있음을 가정한다. 
             * 마지막으로, 
             * 정확한 지역의 위치를 얻기 위해 캔버스 평면의 지역 규모와 결과를 나눕니다. */
            canvasMaxPos_L /= (2.0f * ListPositionCtrl.Instance.transform.parent.localScale.x);

            unitPos_L = canvasMaxPos_L / ListPositionCtrl.Instance.divideFactor;

            lowerBoundPos_L = unitPos_L * (float)(-1 * numOfListBox / 2 - 1);
            upperBoundPos_L = unitPos_L * (float)(numOfListBox / 2 + 1);
            rangeBoundPos_L = unitPos_L * (float)numOfListBox;
            shiftBoundPos_L = unitPos_L * 0.3f;

            originalLocalScale = transform.localScale;

            initialPosition(listBoxID);
            initialContent();
        }

        /* 목록 상자의 내용을 초기화합니다.
         */
        void initialContent()
        {
            if (listBoxID == numOfListBox / 2)
                contentID = 0;
            else if (listBoxID < numOfListBox / 2)
                contentID = ListBank.Instance.getListLength() - (numOfListBox / 2 - listBoxID);
            else
                contentID = listBoxID - numOfListBox / 2;

            while (contentID < 0)
                contentID += ListBank.Instance.getListLength();
            contentID = contentID % ListBank.Instance.getListLength();
        }

        /* 델타 X 또는 Y 위치에 대한 목록 상자 슬라이드를 확인합니다.
         */
        public void setSlidingDistance(Vector3 distance)
        {
            keepSliding = true;
            slidingFrames = ListPositionCtrl.Instance.slidingFrames;

            slidingDistanceLeft_L = distance;
            slidingDistance_L = Vector3.Lerp(Vector3.zero, distance, ListPositionCtrl.Instance.slidingFactor);
        }

        /* 세계 위치 단위에 대한 목록 상자를 이동합니다.
         * "최대"에 해당하는 경우 위로 이동하거나, 다른 사람, 아래로 이동합니다.
         */
        public void unitMove(int unit, bool up_right)
        {
            Vector2 deltaPos;

            if (up_right)
                deltaPos = unitPos_L * (float)unit;
            else
                deltaPos = unitPos_L * (float)unit * -1;

            switch (ListPositionCtrl.Instance.direction)
            {
                case ListPositionCtrl.Direction.VERTICAL:
                    setSlidingDistance(new Vector3(0.0f, deltaPos.y, 0.0f));
                    break;
                case ListPositionCtrl.Direction.HORIZONTAL:
                    setSlidingDistance(new Vector3(deltaPos.x, 0.0f, 0.0f));
                    break;
            }
        }

        void Update()
        {
            if (keepSliding)
            {
                --slidingFrames;
                if (slidingFrames == 0)
                {
                    keepSliding = false;
                    // 마지막 슬라이딩 프레임에서 해당 위치로 이동합니다.
                    // 무료 이동 모드에서,이 기능을 사용할 수 없습니다.
                    if (ListPositionCtrl.Instance.alignToCenter ||
                        ListPositionCtrl.Instance.controlByButton)
                        updatePosition(slidingDistanceLeft_L);
                    return;
                }

                updatePosition(slidingDistance_L);
                slidingDistanceLeft_L -= slidingDistance_L;
                slidingDistance_L = Vector3.Lerp(Vector3.zero, slidingDistanceLeft_L, ListPositionCtrl.Instance.slidingFactor);
            }
        }

        /* 그 ID에 따라 목록 상자의 로컬 위치를 초기화합니다.
         */
        void initialPosition(int listBoxID)
        {
            switch (ListPositionCtrl.Instance.direction)
            {
                case ListPositionCtrl.Direction.VERTICAL:
                    transform.localPosition = new Vector3(0.0f,
                                                     unitPos_L.y * (float)(listBoxID * -1 + numOfListBox / 2),
                                                     0.0f);
                    updateXPosition();
                    break;
                case ListPositionCtrl.Direction.HORIZONTAL:
                    transform.localPosition = new Vector3(unitPos_L.x * (float)(listBoxID - numOfListBox / 2),
                                                     0.0f, 0.0f);
                    updateYPosition();
                    break;
            }
        }

        /* 각 프레임의 델타 위치에 따라 목록 상자의 로컬 위치를 업데이트합니다. 
         * deltaPosition 지역 공간에 있어야합니다.
         */
        public void updatePosition(Vector3 deltaPosition_L)
        {
            switch (ListPositionCtrl.Instance.direction)
            {
                case ListPositionCtrl.Direction.VERTICAL:
                    transform.localPosition += new Vector3(0.0f, deltaPosition_L.y, 0.0f);
                    updateXPosition();
                    //checkBoundaryY();
                    break;
                case ListPositionCtrl.Direction.HORIZONTAL:
                    transform.localPosition += new Vector3(deltaPosition_L.x, 0.0f, 0.0f);
                    updateYPosition();
                    //checkBoundaryX();
                    break;
            }
        }

        /* y 위치에 accroding x 위치를 계산합니다.
         * 공식 : X = max_x * 각 특성의 *의 COS (라디안 Y에 의해 제어)
         * 라디안 = (Y / upper_y) * PI / 2 라디안의 범위 / 2 -pi 원주율 발 / 2 0이고, 그래서
         * 및 해당 코사인 값은 0에서 1 0이다.
         */
        void updateXPosition()
        {
            transform.localPosition = new Vector3(
                canvasMaxPos_L.x * ListPositionCtrl.Instance.angularity * Mathf.Cos(transform.localPosition.y / upperBoundPos_L.y * Mathf.PI / 2.0f),
                transform.localPosition.y,
                transform.localPosition.z);
            updateSize(upperBoundPos_L.y, transform.localPosition.y);
        }

        /* x 위치에 따라 y 위치를 계산합니다.
         */
        void updateYPosition()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                canvasMaxPos_L.y * ListPositionCtrl.Instance.angularity * Mathf.Cos(transform.localPosition.x / upperBoundPos_L.x * Mathf.PI / 2.0f),
                transform.localPosition.z);
            updateSize(upperBoundPos_L.x, transform.localPosition.x);
        }

        /* 리스트 박스가 넘어 있는지 확인 상부 또는 하부 결합 여부.
         * 않으면, 다른쪽에리스트 박스를 이동하고 내용을 업데이트합니다.
         */
        void checkBoundaryY()
        {
            float beyondPosY_L = 0.0f;

            // 한쪽리스트 요동을 방지하기 위해 경계를 확인 후 좁힌다
            if (transform.localPosition.y < lowerBoundPos_L.y + shiftBoundPos_L.y)
            {
                beyondPosY_L = (lowerBoundPos_L.y + shiftBoundPos_L.y - transform.localPosition.y) % rangeBoundPos_L.y;
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    upperBoundPos_L.y + shiftBoundPos_L.y - unitPos_L.y - beyondPosY_L,
                    transform.localPosition.z);
                updateToLastContent();
            }
            else if (transform.localPosition.y > upperBoundPos_L.y - shiftBoundPos_L.y)
            {
                beyondPosY_L = (transform.localPosition.y - upperBoundPos_L.y + shiftBoundPos_L.y) % rangeBoundPos_L.y;
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    lowerBoundPos_L.y - shiftBoundPos_L.y + unitPos_L.y + beyondPosY_L,
                    transform.localPosition.z);
                updateToNextContent();
            }

            updateXPosition();
        }

        void checkBoundaryX()
        {
            float beyondPosX_L = 0.0f;

            // 한쪽리스트 요동을 방지하기 위해 경계를 확인 후 좁힌다
            if (transform.localPosition.x < lowerBoundPos_L.x + shiftBoundPos_L.x)
            {
                beyondPosX_L = (lowerBoundPos_L.x + shiftBoundPos_L.x - transform.localPosition.x) % rangeBoundPos_L.x;
                transform.localPosition = new Vector3(
                    upperBoundPos_L.x + shiftBoundPos_L.x - unitPos_L.x - beyondPosX_L,
                    transform.localPosition.y,
                    transform.localPosition.z);
                updateToNextContent();
            }
            else if (transform.localPosition.x > upperBoundPos_L.x - shiftBoundPos_L.x)
            {
                beyondPosX_L = (transform.localPosition.x - upperBoundPos_L.x + shiftBoundPos_L.x) % rangeBoundPos_L.x;
                transform.localPosition = new Vector3(
                    lowerBoundPos_L.x - shiftBoundPos_L.x + unitPos_L.x + beyondPosX_L,
                    transform.localPosition.y,
                    transform.localPosition.z);
                updateToLastContent();
            }

            updateYPosition();
        }

        /* 위치에 따라 목록 상자의 크기를 확장 할 수 있습니다.
         */
        void updateSize(float smallest_at, float target_value)
        {
            transform.localScale = originalLocalScale *
                (1.0f + ListPositionCtrl.Instance.scaleFactor * Mathf.InverseLerp(smallest_at, 0.0f, Mathf.Abs(target_value)));
        }

        public int getCurrentContentID()
        {
            return contentID;
        }

        /* 다음 목록 상자의 마지막 콘텐츠 업데이트
         * 리스트 박스는 카메라의 상단에 나타납니다 때.
         */
        void updateToLastContent()
        {
            contentID = nextListBox.getCurrentContentID() - 1;
            contentID = (contentID < 0) ? ListBank.Instance.getListLength() - 1 : contentID;
        }

        /* 마지막 목록 상자의 다음 콘텐츠 업데이트
         *리스트 박스는 카메라의 하단에 표시 할 때.
         */
        void updateToNextContent()
        {
            contentID = lastListBox.getCurrentContentID() + 1;
            contentID = (contentID == ListBank.Instance.getListLength()) ? 0 : contentID;
        }
    }
}
