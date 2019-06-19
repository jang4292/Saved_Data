using UnityEngine;
using System.Collections;

namespace CircularScrolling
{
    public class ListScrollMouse : MonoBehaviour
    {
        public Camera toolCamera;

        void Update()
        {
            Vector3 pos = toolCamera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;
        }
    }
}
