using UnityEngine;
using UnityEngine.UI;
using Drawing;

namespace Loading
{
    public class CharacterLoading : MonoBehaviour
    {
        public Image loadingProgressBarImage;

        private DrawingManager drawingManager;

        void Start()
        {
            drawingManager = GameObject.FindObjectOfType<DrawingManager>();
        }

        void Update()
        {
            if(drawingManager.loadingState == DrawingManager.CharacterLoading.Start)
            {
                if (drawingManager.bundleLoadRequest != null)
                {
                    if (!drawingManager.bundleLoadRequest.isDone)
                    {
                        loadingProgressBarImage.fillAmount = drawingManager.bundleLoadRequest.progress;
                    }
                    else
                    {
                        loadingProgressBarImage.fillAmount = 1;
                        gameObject.SetActive(false);
                        drawingManager.loadingState = DrawingManager.CharacterLoading.Stop;
                    }
                }
            }
        }
    }
}
