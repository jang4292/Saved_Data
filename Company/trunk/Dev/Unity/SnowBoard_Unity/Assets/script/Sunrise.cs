using UnityEngine;
using System.Collections;

public class Sunrise : MonoBehaviour {

    public Light SunRise;
    float SunriseRotationValue;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        SunriseRotation();
        SunriseRotationValue = 0.1f;
	}
    void SunriseRotation() {
        SunRise.transform.Rotate(SunriseRotationValue,0, 0);
        if(SunRise.transform.rotation.x < 60f && SunRise.transform.rotation.x >= 0) {
            SunRise.intensity = 0.4f;
        }
    }

}
