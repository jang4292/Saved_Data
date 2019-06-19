using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Design;
using System;

namespace Handler
{
    public class DesignPremiumClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Sprite desingSeletedSprite;
        public Sprite desingDeseletedSprite;

        public Image CharacterImage;

        private Image mImage;

        private ScrollRect scroll;

        /// <summary>
        /// 시작 터치 지점
        /// </summary>
        private Vector2 touchStartPos;

        /// <summary>
        /// 클릭 이벤트 범위
        /// </summary>
        private const float CLICK_EVENT_RANGE = 5f;

        void Start()
        {
            if (scroll == null) scroll = GameObject.Find("SeletedButton").GetComponent<ScrollRect>();
            if (mImage == null) mImage = GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            touchStartPos = transform.position;
            mImage.sprite = desingSeletedSprite;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            mImage.sprite = desingDeseletedSprite;

            if (SetPointDistance(touchStartPos, transform.position) > CLICK_EVENT_RANGE || float.IsNaN(SetPointDistance(touchStartPos, transform.position))) return;

            SetViewImage(CharacterImage);
        }

        /// <summary>
        /// 두점 사이의 길이
        /// </summary>
        /// <param name="startDot">첫번째 점</param>
        /// <param name="endDot">두번째 점</param>
        /// <returns></returns>
        public float SetPointDistance(Vector3 startDot, Vector3 endDot)
        {
            float distance = Mathf.Sqrt(Mathf.Abs(((endDot.x - startDot.x) * (endDot.x - startDot.x)) + ((endDot.y - startDot.y) + (endDot.y - startDot.y))));

            return distance;
        }

        public void SetViewImage(Image image)
        {
            DesignPopup popup = GameObject.FindObjectOfType<DesignPopup>();
            if (popup != null) popup.SeletedChangeSprite(image);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            scroll.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            scroll.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            scroll.OnEndDrag(eventData);
        }
    }
}
