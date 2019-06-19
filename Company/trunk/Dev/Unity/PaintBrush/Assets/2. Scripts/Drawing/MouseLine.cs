using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Public;

namespace Drawing
{
    public class MouseLine : MonoBehaviour
    {
        public enum LineState
        {
            Play,
            Stop
        }

        private LineState state;

        private LineRenderer lineRenderer = null;

        private Vector3 startPos = Vector3.zero;

        [Tooltip("선택된 색상")]
        private Color seletedColor = Color.white;
        [Tooltip("색상 상태와 Hex Color반환을 위한 스크립트")]
        private UIEvent colorState = null;
        [Tooltip("원을 그려줄 객체")]
        public GameObject dotPrefab;
        [Tooltip("프레임마다 마우스의 위치를 저장해줄 리스트 객체")]
        private List<Vector3> points;
        [Tooltip("원 배열")]
        private List<Transform> dots;
        [Tooltip("클릭 체크 해줄 논리값")]
        private bool clickCheck = true;

        /// <summary>
        /// 라인 재질
        /// </summary>
        public Material material = null;

        /// <summary>
        /// 지역에 들어와있는지 체크해줄 스크립트
        /// </summary>
        private DrawingManager drawingManager;

        /// <summary>
        /// sortingOrder
        /// </summary>
        public int sortingOrder;

        [Range(0,1)]
        public float startWidth = 0.2f;

        [Range(0,1)]
        public float endWidth = 0.2f;

        /// <summary>
        /// The minimum offset between points.
        /// </summary>
        [Range(0, 1)]
        public float offsetBetweenPoints = 0.05f;

        /// <summary>
        /// The dot width factor.
        /// </summary>
        [Range(0, 1)]
        public float dotWidthFactor = 0.42f;

        /// <summary>
        /// The dot height factor.
        /// </summary>
        [Range(0, 1)]
        public float dotHeightFactor = 0.42f;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            if (lineRenderer == null) lineRenderer = transform.GetComponent<LineRenderer>();
            if (colorState == null) colorState = GameObject.Find("EventSystem").GetComponent<UIEvent>();
            if (drawingManager == null) drawingManager = GameObject.FindObjectOfType<DrawingManager>();

            lineRenderer.material = material;
            lineRenderer.SetWidth(startWidth, endWidth);

            points = new List<Vector3>();
            dots = new List<Transform>();

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            seletedColor = colorState.SetColorState(colorState.state);

            startPos = mousePos;
            lineRenderer.SetColors(seletedColor, seletedColor);

            lineRenderer.sortingOrder = sortingOrder;

            dots.Add((CreateDot(transform, startPos)));
            dots.Add((CreateDot(transform, startPos)));
        }

        void Update()
        {
            if(state == LineState.Play)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                seletedColor = colorState.SetColorState(colorState.state);

                if (drawingManager.DrawingAreaCheck() && clickCheck)
                {
                    AddPoint(mousePos);

                    if (dots.Count > 1) dots[1].transform.position = mousePos;
                }

                // 삼성 노트 S pen 사용 시 플랫폼은 Android이나, Input은 Mouse로 받는다

                //Mobile Platform
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        PlayingLine();
                    }
                }
                //Others
                else if (Input.GetMouseButtonUp(0))
                {
                    PlayingLine();
                }
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void PlayingLine()
        {
            clickCheck = false;

            if (dots.Count > 1) dots[1].transform.position = points[points.Count - 1];

            points.Clear();
            lineRenderer.SetColors(seletedColor, seletedColor);

            state = LineState.Stop;
        }

        /// <summary>
        /// Dot Create
        /// </summary>
        /// <param name="parent">부모</param>
        /// <param name="pos">위치</param>
        /// <returns></returns>
        private Transform CreateDot(Transform parent, Vector3 pos)
        {
            GameObject dotGameObject = Instantiate(dotPrefab, pos, Quaternion.identity) as GameObject;
            dotGameObject.transform.localScale = new Vector3(startWidth * dotWidthFactor, endWidth * dotHeightFactor, 1);
            dotGameObject.transform.SetParent(parent);
            dotGameObject.name = "Dot";
            SpriteRenderer sr = dotGameObject.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.material = material;
                sr.color = seletedColor;
                sr.sortingOrder = lineRenderer.sortingOrder;
            }

            return dotGameObject.transform;
        }

        /// <summary>
        /// Add the point to the line.
        /// </summary>
        /// <param name="point">Point.</param>
        public void AddPoint(Vector3 point)
        {
            if (lineRenderer != null)
            {
                if (points.Count > 1)
                {
                    // 선분 사이의 거리에 최솟값을 주어 끊기지 않고, Vector3 값을 추가로 많이 안 만들기 위해 비교 
                    if (Vector3.Distance(point, points[points.Count - 1]) < offsetBetweenPoints)
                    {
                        return;//skip the point
                    }
                }

                points.Add(point);

                lineRenderer.SetVertexCount(points.Count);
                lineRenderer.SetPosition(points.Count - 1, point);

                BezierInterpolate();
            }
        }

        /// <summary>
        /// 곡선을 부드럽게 하기 위한 메소드
        /// </summary>
        private void BezierInterpolate()
        {
            BezierPath bezierPath = new BezierPath();
            bezierPath.Interpolate(points, 0.3f);

            List<Vector3> drawingPoints = bezierPath.GetDrawingPoints2();

            SetLinePoints(drawingPoints);
        }

        /// <summary>
        /// Set the line points.
        /// </summary>
        /// <param name="drawingPoints">Drawing points.</param>
        private void SetLinePoints(List<Vector3> drawingPoints)
        {
            lineRenderer.SetVertexCount(drawingPoints.Count);

            for (int i = 0; i < drawingPoints.Count; i++)
            {
                if (i == 0)
                {
                    dots[0].position = drawingPoints[i];
                    if (drawingPoints.Count > 1)
                        SetDotLookAt(dots[0], drawingPoints[1]);
                    dots[0].GetComponent<SpriteRenderer>().enabled = true;
                }
                else if (i == drawingPoints.Count - 1)
                {
                    dots[1].position = drawingPoints[i];
                    if (drawingPoints.Count - 2 >= 0)
                        SetDotLookAt(dots[1], drawingPoints[drawingPoints.Count - 2]);
                    dots[1].GetComponent<SpriteRenderer>().enabled = true;
                }

                lineRenderer.SetPosition(i, drawingPoints[i]);
            }
        }

        /// <summary>
        /// (임시) 주어진 점에 도트 모양(회전)을 설정합니다.
        /// </summary>
        /// <param name="dot">Dot.</param>
        /// <param name="point">Point.</param>
        private void SetDotLookAt(Transform dot, Vector3 point)
        {
            if (dot == null)
            {
                return;
            }

            Vector2 direction = dot.transform.position - point;
            dot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg + 90);
        }

        /// <summary>
        /// 포인트 갯수 반환
        /// </summary>
        /// <returns></returns>
        public int GetPointsCount()
        {
            return points.Count;
        }

        /// <summary>
        /// Set the sorting order.
        /// </summary>
        /// <param name="sortingOrder">Sorting order.</param>
        public void SetSortingOrder(int sortingOrder)
        {

            if (lineRenderer != null)
            {
                lineRenderer.sortingOrder = sortingOrder;
            }
            this.sortingOrder = sortingOrder;
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="startWidth">Start width.</param>
        /// <param name="endWidth">End width.</param>
        public void SetWidth(float startWidth, float endWidth)
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetWidth(startWidth, endWidth);
            }
            this.startWidth = startWidth;
            this.endWidth = endWidth;

        }
    }
}
