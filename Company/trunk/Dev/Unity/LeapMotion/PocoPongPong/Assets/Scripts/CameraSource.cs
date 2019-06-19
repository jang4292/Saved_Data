using UnityEngine;

public class CameraSource : MonoBehaviour {

    public Transform camTransform;

    public float shake = 0f;

    // 흔들림 양
    private float shakeAmount = 0.1f;

    private float decreaseFactor = 1.0f;

    private Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;

            if(shake < 2 && shake > 0)
            {
                shake = 100;
            }
        }
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}
