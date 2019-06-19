using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Enlargement;

namespace Handler
{
    public class EnlargementClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public TouchEnlargement enlargementManager;
        public Text percentText;

        private const float PERCENT_ONE_HUNDRED = 5.4f;
        private const float PERCENT_TWO_HUNDRED = 3f;
        private const float PERCENT_THREE_HUNDRED = 1f;

        public void OnPointerClick(PointerEventData eventData)
        {
            // 100% -> 200%
            if(enlargementManager.imageCamera.orthographicSize == PERCENT_ONE_HUNDRED)
            {
                enlargementManager.imageCamera.orthographicSize = PERCENT_TWO_HUNDRED;
                enlargementManager.uiCamera.orthographicSize = PERCENT_TWO_HUNDRED;

                percentText.text = "200%";

                enlargementManager.enlargementButtons.SetActive(true);
                enlargementManager.mState = TouchEnlargement.EnlargementState.Start;

                enlargementManager.SetCameraMaxPos(enlargementManager.imageCamera.orthographicSize);
            }
            // 200% -> 300%
            else if (enlargementManager.imageCamera.orthographicSize == PERCENT_TWO_HUNDRED)
            {
                enlargementManager.imageCamera.orthographicSize = PERCENT_THREE_HUNDRED;
                enlargementManager.uiCamera.orthographicSize = PERCENT_THREE_HUNDRED;

                percentText.text = "300%";

                enlargementManager.SetCameraMaxPos(enlargementManager.imageCamera.orthographicSize);
            }
            // 300% -> 100%
            else if (enlargementManager.imageCamera.orthographicSize == PERCENT_THREE_HUNDRED)
            {
                enlargementManager.SetReSizing();

                percentText.text = "100%";

                enlargementManager.enlargementButtons.SetActive(false);
                enlargementManager.mState = TouchEnlargement.EnlargementState.Idle;

                enlargementManager.SetCameraMaxPos(enlargementManager.imageCamera.orthographicSize);
            }
        }
    }
}
