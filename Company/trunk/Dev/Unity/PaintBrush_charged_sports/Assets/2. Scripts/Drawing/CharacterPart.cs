using UnityEngine;
using System.Collections;

///Developed by Indie Games Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiegstudio.com
///freelance.art2014@gmail.com


/* 현재 스크립트의 네임스페이스 "Drawing"을 임의로 변경할 시 기존에 만들어진 
   에셋번들이 사용이 사용할 수 없습니다. 네임스페이스 명을 임의로 바꾸지 않을 것을 강조드립니다.

   - 만일 바꾸게 될 시 기존에 있는 에셋번들을 전부 다 새로 만들어 올리시길 바랍니다. - */
namespace Drawing
{
    [DisallowMultipleComponent]
    public class CharacterPart : MonoBehaviour
    {

        /// <summary>
        /// The initial sorting order.
        /// </summary>
        [HideInInspector]
        public int initialSortingOrder;

        /// <summary>
        /// The sprite renderer reference.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// The color lerp speed.
        /// </summary>
        private static float colorLerpSpeed = 7;

        /// <summary>
        /// The target color
        /// </summary>
        [HideInInspector]
        public Color targetColor = Color.white;

        // Use this for initialization
        void Start()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            ///Set up the initial sorting order
            initialSortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

            /////Apply the previous color on part
            //object previousColor = Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsColors[name];
            //if (previousColor != null)
            //    spriteRenderer.color = (Color)previousColor;

            //targetColor = (Color)previousColor;

            ///Apply the previous sorting order on part
            object previousSortingOrder = Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsSortingOrder[name];
            if (previousSortingOrder != null)
                spriteRenderer.sortingOrder = (int)previousSortingOrder;
        }

        void Update()
        {
            LerpToColor();
        }

        /// <summary>
        /// Lerp the target color.
        /// </summary>
        public void LerpToColor()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (targetColor == spriteRenderer.color)
            {
                return;
            }
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, colorLerpSpeed * Time.smoothDeltaTime);
        }

        /// <summary>
        /// Apply the initial sorting order.
        /// </summary>
        public void ApplyInitialSortingOrder()
        {
            GetComponent<SpriteRenderer>().sortingOrder = initialSortingOrder;
        }

        /// <summary>
        /// Set the color of the part.
        /// </summary>
        /// <param name="targetColor">Target color.</param>
        public void SetColor(Color targetColor)
        {
            this.targetColor = targetColor;
        }

        /// <summary>
        /// Apply the initial color.
        /// </summary>
        public void ApplyInitialColor()
        {
            this.targetColor = Color.white;
        }
    }

}
