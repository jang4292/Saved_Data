using UnityEngine;

namespace Utility.Function
{
    public abstract class ListView : MonoBehaviour
    {

        public abstract int ItemCount
        {
            get;
        }

        public abstract int StartYPosition
        {
            get;
        }
        public abstract int ItemHeight
        {
            get;
        }

        public abstract void getItem(int index, GameObject obj);


        [SerializeField]
        protected GameObject item;
        [SerializeField]
        protected GameObject container;

        public void Clear()
        {
            foreach (Transform item in container.transform)
            {
                if (item.name.EndsWith("(Clone)"))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        public void RefreshListViewNewItem()
        {
            Clear();
            var rectTransform = container.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, ItemHeight * ItemCount);
            for (int i = 0; i < ItemCount; i++)
            {

                var v = Instantiate(item);
                v.transform.SetParent(rectTransform);
                v.SetActive(true);

                var rt = v.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0, (StartYPosition - (ItemHeight * i)));
                rt.sizeDelta = new Vector2(0, ItemHeight);

                getItem(i, v);
            }
        }
    }
}