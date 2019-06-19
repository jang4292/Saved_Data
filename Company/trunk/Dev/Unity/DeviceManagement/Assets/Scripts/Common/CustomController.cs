using UnityEngine.SceneManagement;
using CommonData.Data;
using Utility.Function;
using UnityEngine.UI;
using UnityEngine;
using Utility;

public class CustomController : Controller
{

    public interface IMILEAGE
    {
        Text RefreshMileage();
    }
    private IMILEAGE iMILEAGE;
    public void setIMILEAGE(IMILEAGE i)
    {
        iMILEAGE = i;
    }
    public interface IList
    {
        void NewVideoList();
        void BoughtList();
    }

    private IList iList;
    public void setListInterface(IList i)
    {
        iList = i;
    }

    public void NewVideoList()
    {
        if (iList != null)
        {
            iList.NewVideoList();
        }
    }

    public void BoughtList()
    {
        if (iList != null)
        {
            iList.BoughtList();
        }
    }

    private static CustomController instance = null;
    public static CustomController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CustomController();
            }
            return instance;
        }
    }

    public override int Count
    {
        get
        {
            if(VideoXMLDataList.lists == null)
            {
                return 0;
            }
            return VideoXMLDataList.lists.Count;
        }
    }

    public enum SceneName
    {
        LOGIN,
        MENU_VIEW,
        MAIN_VIEW,
        SETTING,
        UPDATE_VIEW
    }

    public void changedLoadScene(SceneName name)
    {
        Log.i("[Changed Scene] : " + name);
        switch (name)
        {
            case SceneName.LOGIN:
                SceneManager.LoadScene("00.Login", LoadSceneMode.Single);
                break;
            case SceneName.MENU_VIEW:
                SceneManager.LoadScene("01.Menu", LoadSceneMode.Single);
                break;
            case SceneName.MAIN_VIEW:
                SceneManager.LoadScene("02.MainView", LoadSceneMode.Single);
                break;
            case SceneName.SETTING:
                SceneManager.LoadScene("02.Setting", LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }

    public void RefreshMileage()
    {
        if(iMILEAGE != null)
        {
            Text text = iMILEAGE.RefreshMileage();
            text.text = UserData.UserName + "님 보유포인트 : "+ Util.getPriceWithComma(UserData.MILEAGE) +"포인트";
        }
    }
}
