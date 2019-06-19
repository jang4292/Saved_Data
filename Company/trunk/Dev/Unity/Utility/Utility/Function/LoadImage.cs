using UnityEngine;
using UnityEngine.UI;

namespace Utility.Function
{
    public class LoadImage : MonoBehaviour
    {

        public Image myImageComponent;


        public void changedMainImage(WWW www)
        {
            //myImageComponent.sprite = 

            Util.ChangeImageFromTextrue2D(www.texture, myImageComponent);
            //www.LoadImageIntoTexture((Texture2D)myImageComponent.mainTexture);
        }

        //public delegate void FinishedChange(WWW www);
        /*
        public void changedMainImage(int imageNumber)
        {
            changedMainImage(imageNumber, null);
        }
        */
        //public void changedMainImage(int imageNumber, FinishedChange finishedChange)
        //public void changedMainImage(string path, FinishedChange finishedChange)
        //{
        //Debug.Log("imageNumber :: " + imageNumber);

        //string path = Data.SettingData.VideoDataList.lists[imageNumber].video_thumbnail + "normal.png";
        //Debug.Log("path :: " + path);

        //if(finishedChange != null) { 

        //ServerAPI.Instance.changedMainImage(path, finishedChange);

        //www.LoadImageIntoTexture((Texture2D)myImageComponent.mainTexture);
        //}


    }
}