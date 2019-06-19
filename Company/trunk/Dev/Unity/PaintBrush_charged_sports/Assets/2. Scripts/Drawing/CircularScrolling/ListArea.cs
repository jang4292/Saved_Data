using UnityEngine;

namespace CircularScrolling
{
    public class ListArea : MonoBehaviour
    {
        /// <summary>
        /// 마우스
        /// </summary>
        public Transform mouse;

        /// <summary>
        /// 마우스가 들어오는지 확인할 스프라이트 렌더러
        /// </summary>
        private SpriteRenderer ren;

        public enum ListAreaState
        {
            Out,
            In
        }

        public ListAreaState state;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            ren = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            // 범위에 따른 상태 변화
            if (ren.bounds.Contains(mouse.position))
            {
                state = ListAreaState.In;
            }
            else
            {
                state = ListAreaState.Out;
            }
        }
    }
}
