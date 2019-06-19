using UnityEngine;
using System.Collections;


namespace Drawing
{
    public class ChangeColor : MonoBehaviour
    {
        /// <summary>
        /// 파레트 텍스쳐
        /// </summary>
        public Texture2D paletteTexture;

        /// <summary>
        /// 파레트 색상을 보여줄 백그라운드
        /// </summary>
        public GameObject paletteBackground;

        /// <summary>
        /// 선택된 컬러 색상
        /// </summary>
        [System.NonSerialized]
        public Color color = Color.white;

        /// <summary>
        /// 파레트 위치 및 크기
        /// </summary>
        private Rect textureRect;

        private DrawingManager drawingManager;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            textureRect = new Rect(0, 0, Screen.width, Screen.height);
            drawingManager = GameObject.FindObjectOfType<DrawingManager>();
        }

        void OnGUI()
        {
            if (Event.current.type == EventType.MouseDrag)
            {
                Vector2 mousePosition = Event.current.mousePosition;

                float textureUPosition = (mousePosition.x - textureRect.x) / textureRect.width;
                float textureVPosition = 1.0f - ((mousePosition.y - textureRect.y) / textureRect.height);

                Color textureColour = paletteTexture.GetPixelBilinear(textureUPosition, textureVPosition);

                /// 흰색이 들어오면 리턴
                if(textureColour == new Color(1,1,1,0) || textureColour == new Color(1,1,1,1))
                {
                    return;
                }

                /// 텍스쳐 밖에서 색상이 선택될 시 알파값이 0이 되므로 1로 임의로 변경
                if(textureColour.a == 0)
                {
                    textureColour.a = 1;
                }

                color = textureColour;

                drawingManager.ChangeThicknessSizeColor();

                paletteBackground.GetComponent<SpriteRenderer>().color = textureColour;
            }

            if (Event.current.type == EventType.MouseUp) 
            {
                drawingManager.PaletteDisable();
                drawingManager.drawingState = DrawingManager.DrawingState.Playing;
            }
        }
    }
}
