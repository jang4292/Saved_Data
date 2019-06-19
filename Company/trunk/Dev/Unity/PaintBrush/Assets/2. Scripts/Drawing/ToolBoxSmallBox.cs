using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

namespace Drawing
{
    public class ToolBoxSmallBox : MonoBehaviour
    {
        public enum ToolState
        {
            Pattern,
            Pencil,
            Brush,
            Fill,
            Eraser
        }

        [Tooltip("Tool 상태")]
        public ToolState state;

        [Tooltip("레이캐스트를 쏠 방향 및 위치")]
        public Transform rayPos = null;

        [Tooltip("현재 스프라이트 렌더러")]
        private SpriteRenderer small_icon = null;

        private Mouse mouse;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            small_icon = this.GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            if (mouse == null) mouse = GameObject.Find("Mouse").GetComponent<Mouse>();
        }

        void Update()
        {
            if (mouse.state == Mouse.MouseState.NotClick)
            {
                //Raycasthit 2D
                Ray2D ray = new Ray2D(rayPos.position, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    mouse.state = Mouse.MouseState.Idle;

                    Sprite collider_Image = GameObject.Find(hit.collider.name).GetComponent<Image>().sprite;
                    small_icon.sprite = collider_Image;


                    switch (hit.collider.name)
                    {
                        case "Pattern":
                            SetToolState(ToolState.Pattern, hit.collider.name);
                            break;
                        case "Pencil":
                            SetToolState(ToolState.Pencil, hit.collider.name);
                            break;
                        case "Brush":
                            SetToolState(ToolState.Brush, hit.collider.name);
                            break;
                        case "Fill":
                            SetToolState(ToolState.Fill, hit.collider.name);
                            break;
                        case "Eraser":
                            SetToolState(ToolState.Eraser, hit.collider.name);
                            break;
                    }
                }
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

#if UNITY_ANALYTICS

        private string customEventName = "Tool";

        private Dictionary<string, object> data;

        /// <summary>
        /// 아날리틱스
        /// </summary>
        void LogAnalytics(string name)
        {
            if (data == null) data = new Dictionary<string, object>();
            data.Add(name, name);
            Analytics.CustomEvent(customEventName, data);
            data = null;
        }
#endif

        void SetToolState(ToolState toolState, string toolName)
        {
            state = toolState;

            LogAnalytics(toolName);
        }
    }
}
