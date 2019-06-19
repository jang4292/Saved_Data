using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextControl : MonoBehaviour
{

    private bool isPrevShop = false;
    public static bool isShop = false;

    [SerializeField]
    private GameObject HistroyPanel;
    [SerializeField]
    private GameObject ShopPanel;




    // Use this for initialization
    void Start()
    {
        StartCoroutine(ChangedText());
    }


    private IEnumerator ChangedText()
    {
        while (true)
        {

            if (!isPrevShop.Equals(isShop))
            {
                isPrevShop = isShop;

                ShopPanel.SetActive(isShop);
                HistroyPanel.SetActive(!isShop);
            }
            yield return new WaitForSeconds(1);

        }
    }



}


