using UnityEngine;
using UnityEngine.UI;
using CommonData.Data;
using Utility.Function;

public class InsertText : MonoBehaviour {

    public int index;
    public Text titleText;
    public Text durationText;
    public Text priceText;
    public Text Downloaded;

    public void UpdateText()
    {
        var item = SettingData.VideoDataList.lists[index];
        titleText.text = item.video_title;
        durationText.text = Util.getTimeString(item.video_playtime);
        priceText.text = Util.getPriceWithComma(item.price) + " 원";
        Downloaded.text = item.isUpdate;
    }

    public void OnClicked()
    {
        CustomController.Instance.indexChanged(index);
    }
}
