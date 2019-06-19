using UnityEngine;

namespace BackPress
{
    public class StoreBackPress : MonoBehaviour, BackPressListener
    {
        public GameObject pageLoadingPopup;
        public GameObject inAppFailedPopup;
        public GameObject homePopup;
        public GameObject networkClosePopup;

        public void OnBackPress()
        {
            if (pageLoadingPopup.activeSelf || inAppFailedPopup.activeSelf || networkClosePopup.activeSelf) return;

            if (!homePopup.activeSelf) homePopup.SetActive(true);
            else homePopup.SetActive(false);
        }
    }
}
