using CommonData.Data;
using UnityEngine;
using Utility.Function;

public class SettingManager : MainManager, CustomController.IList
{


    void Awake()
    {


        TextControl.isShop = false;
        CustomController.Instance.setListInterface(this);
        CustomController.Instance.setIRefresh(this);
        CustomController.Instance.indexClear();
        //Controller.Instance.indexClear();
        BoughtList();
        CustomController.Instance.RefreshMileage();
    }

    public override void refresh(int index)
    {
        if (SettingData.VideoDataList == null)
        {
            return;
        }
        var v = SettingData.VideoDataList.lists[index];

        string url = v.video_thumbnail + "normal.png";
        ServerAPI.Instance.LoadImage(url, (www) => {
            loadImage.changedMainImage(www);
            
            titleText.text = v.video_title;
            duration.text = Util.getTimeString(v.video_playtime);
            price.text = Util.getPriceWithComma(v.price) + " 원";
            
        });
    }

    public void BoughtList()
    {

        ServerAPI.Instance.BoughtList((WWW www) =>
        {
            var temp = JsonUtility.FromJson<VideoDataList>(www.text);

            SettingData.VideoDataList = temp;

            CustomXmlController.Instance.XMLwrite(temp.lists);
            CustomController.Instance.indexClear();
            
            GetComponent<CustomListView>().RefreshListViewNewItem();

            SettingData.IsActivedStartButton = true;
        });

    }

    public void NewVideoList()
    {

        ServerAPI.Instance.NewVideoList((WWW www) =>
        {
            var temp = JsonUtility.FromJson<VideoDataList>(www.text);

            SettingData.VideoDataList = temp;

            CustomXmlController.Instance.XMLwrite(temp.lists);
            CustomController.Instance.indexClear();
            
            GetComponent<CustomListView>().RefreshListViewNewItem();
        });

    }
}
