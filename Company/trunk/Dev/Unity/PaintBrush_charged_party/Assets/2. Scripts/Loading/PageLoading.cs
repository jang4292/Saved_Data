using UnityEngine;
using System.Collections;

namespace Loading
{
    public class PageLoading : MonoBehaviour
    {
        private float loadSpeed = 3f;

        void LateUpdate()
        {
            gameObject.transform.Rotate(Vector3.back, loadSpeed);
        }
    }
}
