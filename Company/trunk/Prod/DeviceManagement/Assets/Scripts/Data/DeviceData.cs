using UnityEngine;
using System;
using UnityEngine.UI;
namespace CommonData.Data
{
    public class DeviceData : MonoBehaviour
    {

        [SerializeField]
        private DeviceInfo[] deviceInfos;

        [Serializable]
        public class DeviceInfo
        {
            public Image item;
            public bool isConnected;
            public int sendCount = 0;
        }
    }
}