using UnityEngine;
using System;
using UnityEngine.UI;
using CommonData.Data;

public class TempDropDown : MonoBehaviour {

    public interface IDropDown
    {
        void iDropDown(string text);
    }
    public void setDropDownInterface(IDropDown i)
    {
        iDropDown = i;
    }
    private IDropDown iDropDown;
    public Text text;

    public void OnValueChanged(Int32 i)
    {
        SettingData.MEMBER_COUNT = text.text;
        if (iDropDown != null)
        {
            iDropDown.iDropDown(SettingData.MEMBER_COUNT);
        }
    }
}
