using UnityEngine;
using System.Collections;

public class CardTouchTrigger : MonoBehaviour {

    public GameObject gameController;

	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Card")
        {
            gameController.GetComponent<CardGameController>()
                .controlNowTouchedCard(other.gameObject);
        }
    }

}
