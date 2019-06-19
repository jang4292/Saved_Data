using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DrawingAlbum;

namespace Handler
{
    public class AlbumClickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject seletedObj;
        public RawImage seletedImage;

        private GameObject viewObj;
        private RawImage viewImage;

        private Album album;

        private ScrollRect scroll;

        /// <summary>
        /// 시작 터치 지점
        /// </summary>
        private Vector2 touchStartPos;

        /// <summary>
        /// 클릭 이벤트 범위
        /// </summary>
        private const float CLICK_EVENT_RANGE = 1f;

        void Awake()
        {
            if (scroll == null) scroll = GameObject.Find("SeletedButton").GetComponent<ScrollRect>();
            if (album == null) album = GameObject.FindObjectOfType<Album>();
            if (viewObj == null) viewObj = album.seletedObj;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (SetPointDistance(touchStartPos, transform.position) > CLICK_EVENT_RANGE || float.IsNaN(SetPointDistance(touchStartPos, transform.position))) return;

            SetPartOnClick(gameObject, seletedObj);
            SetSeletedSprite(seletedImage);
        }

        /// <summary>
        /// Album Part OnClick
        /// </summary>
        /// <param name="image"></param>
        public void SetPartOnClick(GameObject obj, GameObject image)
        {
            if (album.imageDeletedList == null) album.imageDeletedList = new List<GameObject>();

            if (!image.activeSelf)
            {
                album.imageDeletedList.Add(obj);
                image.SetActive(true);
            }
            else
            {
                album.imageDeletedList.Remove(obj);
                image.SetActive(false);
            }

            album.seletedFilePath = obj.name;
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

        /// <summary>
        /// 선택된 이미지 변경
        /// </summary>
        /// <param name="image"></param>
        public void SetSeletedSprite(RawImage image)
        {
            if (viewImage == null) viewImage = viewObj.GetComponent<RawImage>();
            viewObj.SetActive(true);
            viewImage.texture = image.texture;
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
