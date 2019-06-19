using CommonData.Data;
using UnityEngine;
using UnityEngine.UI;
using Utility.Function;

public class CustomListView : ListView {

    private int DisplayItem = 3;
    [SerializeField]
    private ScrollRect scrollRect;

    public void Start()
    {
        scrollRect.vertical = ItemCount > DisplayItem;
    }

    public override int ItemCount
    {
        get
        {
            int count = 0;
            if (SettingData.VideoDataList != null)
            {
                if(SettingData.VideoDataList.lists != null)
                { 
                    count = SettingData.VideoDataList.lists.Count;
                }
            }
            return count;
        }
    }

    public override int ItemHeight
    {
        get
        {
            return 60;
        }
    }

    public override int StartYPosition
    {
        get
        {
            return -30;
        }
    }

    public override void getItem(int i, GameObject obj)
    {
        var text = obj.GetComponent<InsertText>();
        text.index = i;
        text.UpdateText();
    }
}
