using UnityEngine;

namespace BackPress
{
    public class MainBackPress : MonoBehaviour, BackPressListener
    {
        public GameObject[] popupObjs;

        public GameObject dataLoadingPopup;
        public GameObject versionCheckPopup;
        public GameObject userInfoUpdatePopup;
        public GameObject designStorePopup;
        public GameObject designPopup;
        public GameObject exitPopup;

        public void OnBackPress()
        {
            if (IsPopupOpen()) return;

            if (dataLoadingPopup != null && dataLoadingPopup.activeSelf) return;

            if (versionCheckPopup.activeSelf)
            {
                Application.Quit();
            }
            else if (!userInfoUpdatePopup.activeSelf)
            {
                if (designStorePopup.activeSelf)
                {
                    designStorePopup.SetActive(false);
                }
                else
                {
                    if (designPopup.activeSelf)
                    {
                        designPopup.SetActive(false);
                    }
                    else
                    {
                        if (exitPopup.activeSelf)
                        {
                            exitPopup.SetActive(false);
                        }
                        else
                        {
                            exitPopup.SetActive(true);
                        }
                    }
                }
            }
        }

        private bool IsPopupOpen()
        {
            foreach(GameObject obj in popupObjs)
            {
                if (obj.activeSelf)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
