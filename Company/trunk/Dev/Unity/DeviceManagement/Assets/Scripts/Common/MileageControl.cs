using UnityEngine;
using UnityEngine.UI;

public class MileageControl : MonoBehaviour, CustomController.IMILEAGE {

    [SerializeField]
    private Text text;

    public Text RefreshMileage()
    {
        return text;
    }


    private void Awake()
    {
        CustomController.Instance.setIMILEAGE(this);
	}

}
