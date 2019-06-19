using Utility.Function;
using CommonData.Data;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ViewManager : MainManager, XMLController.IFinishedReadXML, TempDropDown.IDropDown
{
    [SerializeField]
    private Text indexText;
    [SerializeField]
    private Text TaxPriceText;

    [SerializeField]
    private Text PayPriceText;


    public override void Start()
    {
        //base.Start();
        CustomController.Instance.setIRefresh(this);
        CustomController.Instance.indexClear();

        GameObject.Find("Dropdown").GetComponent<TempDropDown>().setDropDownInterface(this);
        CustomXmlController.Instance.setIFinishedReadXML(this);
        CustomXmlController.Instance.xmlReader();
        CustomController.Instance.RefreshMileage();
    }
    public override void refresh(int index)
    {
        
        var v = VideoXMLDataList.lists[index];
        var s = SettingData.VideoDataList.lists[index / 2];
        string path = @"file://" + SettingData.PATH_FOR_ATTRACTION_CONTROL_PROGRAM + @"img\" + v.title + ".png";
        ServerAPI.Instance.LoadImage(path, (www) => {
            loadImage.changedMainImage(www);
            titleText.text = v.title;
            duration.text = Util.getTimeString(s.video_playtime);
            //price.text = Util.getPriceWithComma(s.playPrice) + " 원";
            indexText.text = "No." + index;

            PriceControll(s.playPrice);
        });
    }

    private string playPrice;
    public void PriceControll(string playPrice )
    {
        this.playPrice = playPrice;
        PriceControll();
    }

    public void PriceControll()
    {
        price.text = Util.getPriceWithComma(playPrice) + " 원";
        var p = int.Parse(playPrice);
        var c = int.Parse(SettingData.MEMBER_COUNT);
        var sumPrice = p * c;
        UserData.PriceOfTax = Convert.ToString(sumPrice / 100 * 20);
        TaxPriceText.text = Util.getPriceWithComma(UserData.PriceOfTax) + " 원";
        PayPriceText.text = Util.getPriceWithComma(sumPrice) + " 원";
    }

    public void FinishedReadXML()
    {
        refresh(0);
        //throw new NotImplementedException();
    }

    public void iDropDown(string text)
    {
        PriceControll();
    }
}
