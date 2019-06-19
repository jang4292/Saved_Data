using UnityEngine;

namespace BackPress
{
    public class GameBackPress : MonoBehaviour, BackPressListener
    {
        public GameObject[] popupObjs;

        public GameObject pageLoadPopup;
        public GameObject designStorePopup;
        public GameObject designPopup;
        public GameObject homePopup;

        public void OnBackPress()
        {
            if (IsPopupOpen()) return;

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
                    if (homePopup.activeSelf)
                    {
                        homePopup.SetActive(false);
                    }
                    else
                    {
                        homePopup.SetActive(true);
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
                    obj.SetActive(false);
                    return true;
                }
            }

            return false;
        }
    }
}
