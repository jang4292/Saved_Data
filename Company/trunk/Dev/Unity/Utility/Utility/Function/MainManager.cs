using UnityEngine;
using UnityEngine.UI;

namespace Utility.Function
{

    public abstract class MainManager : MonoBehaviour, Controller.IRefresh
    {

        [SerializeField]
        protected LoadImage loadImage;

        [SerializeField]
        protected Text titleText;
        [SerializeField]
        protected Text duration;
        [SerializeField]
        protected Text price;


        public abstract void refresh(int index);

        public virtual void Start()
        {
            //CustomController.Instance.setIRefresh(this);
            //CustomController.Instance.indexClear();
        }
    }
}