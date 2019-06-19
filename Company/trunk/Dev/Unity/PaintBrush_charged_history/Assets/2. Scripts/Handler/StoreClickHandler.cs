using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Store;
using System;

namespace Handler
{
    public class StoreClickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Sprite singlePartSeletedSprite;
        public Sprite singlePartDeseletedSprite;

        private const int SINGLE_PART_BUY_SELETED_ITEM_MAX = 5;

        /// <summary>
        /// 클릭 이벤트 범위
        /// </summary>
        private const float CLICK_EVENT_RANGE = 1.5f;

        private StoreManager storeManager;

        private Image mImage;

        private ScrollRect scroll;

        /// <summary>
        /// 시작 터치 지점
        /// </summary>
        private Vector2 touchStartPos;

        void Start()
        {
            if (scroll == null) scroll = GameObject.Find("SingleScroll").GetComponent<ScrollRect>();
            if (mImage == null) mImage = GetComponent<Image>();
            if (storeManager == null) storeManager = GameObject.FindObjectOfType<StoreManager>();
        }

        /// <summary>
        /// 오브젝트가 비활성화 될때
        /// </summary>
        void OnDisable()
        {
            mImage.sprite = singlePartDeseletedSprite;
        }

        /// <summary>
        /// Click Event
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (SetPointDistance(touchStartPos, transform.position) > CLICK_EVENT_RANGE || float.IsNaN(SetPointDistance(touchStartPos, transform.position))) return;

            if (storeManager.userBuyidxList == null) storeManager.userBuyidxList = new List<string>();

            // Non-Select -> Select 
            if (mImage.sprite == singlePartDeseletedSprite)
            {
                // Drawing Select Max 5
                if (storeManager.userBuyidxList.Count < SINGLE_PART_BUY_SELETED_ITEM_MAX)
                {
                    mImage.sprite = singlePartSeletedSprite;
                    storeManager.userBuyidxList.Add(name);
                }
            }
            // Selected -> Non-Select
            else
            {
                mImage.sprite = singlePartDeseletedSprite;
                storeManager.userBuyidxList.Remove(name);
            }

            storeManager.SetSelectedCount();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            touchStartPos = transform.position;
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
