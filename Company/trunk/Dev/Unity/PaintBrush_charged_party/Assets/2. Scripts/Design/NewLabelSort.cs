using UnityEngine;
using System.Collections;

namespace Design
{
    public class NewLabelSort : MonoBehaviour
    {
        public GameObject newLabelSort;

        void Start()
        {
            gameObject.transform.position = newLabelSort.transform.position;
        }
    }
}
