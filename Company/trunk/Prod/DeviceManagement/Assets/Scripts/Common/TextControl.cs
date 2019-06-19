using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextControl : MonoBehaviour {

    [SerializeField]
    private Text Download;
    [SerializeField]
    private Text Shop;

    public static bool isShop = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(ChangedText());	
	}

	
    private IEnumerator ChangedText()
    {
        while(true)
        {
            if(!isShop)
            {
            Download.text = "Download";
            Shop.text = "Shop";

            } else
            {
                Download.text = "Buy";
                Shop.text = "Histroy";
            }
            yield return new WaitForSeconds(1);

        }
    }



}


