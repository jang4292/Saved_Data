using UnityEngine;
using UnityEngine.UI;

public class LocalIPAddress : MonoBehaviour {

    [SerializeField]
    private Text IPText;

    public void Start()
    {
        IPText.text = "Host IP : " + Utility.Function.Util.GetLocalIP();
    }
}
