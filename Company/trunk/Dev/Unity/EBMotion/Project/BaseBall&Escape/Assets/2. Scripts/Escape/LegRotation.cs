using UnityEngine;
using EBMotion;


namespace Escape
{
    public class LegRotation : MonoBehaviour
    {
        private Quaternion firstRotation = Quaternion.identity;
        public int ID = 0;
        private EbmDataParser ebmData;
        private Quaternion quat;

        private static bool check = false;
        private static float correctionValue = 0;

        void Start()
        {
            ebmData = GameObject.Find("EbmDataParser").GetComponent<EbmDataParser>();
            firstRotation = this.transform.rotation;
        }

        void Update()
        {
            if (GameState.directionState == GameState.Direction.Right)
            {
                if (ID == 4 || ID == 5)
                {
                    SetRightRotation();
                }
            }
            else
            {
                if (ID == 6 || ID == 7)
                {
                    SetLeftRotation();
                }
            }
        }

        void OnDestroy()
        {
            correctionValue = 0;
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void SetRightRotation()
        {
            if (!ebmData.QuatData[ID].Equals(new Quaternion(0, 0, 0, 0)))
            {
                quat = ebmData.QuatData[ID];
                Vector3 correction = ebmData.correction[4];

                if (correction != Vector3.zero && correctionValue == 0)
                {
                    correctionValue = correction.y;
                }

                if (correctionValue < 40f && correctionValue > 30f)
                {
                    quat.y = firstRotation.z - quat.z;
                    quat.x = firstRotation.y - quat.y;
                }
                else
                {
                    quat.y = firstRotation.y - quat.y;
                    quat.x = firstRotation.x + quat.x;
                }

                quat.w = firstRotation.w + quat.w;
                quat.z = 0;

                this.transform.rotation = quat;
            }
        }

        void SetLeftRotation()
        {
            if (!ebmData.QuatData[ID].Equals(new Quaternion(0, 0, 0, 0)))
            {
                quat = ebmData.QuatData[ID];
                Vector3 correction = ebmData.correction[6];

                if (correction != Vector3.zero && correctionValue == 0)
                {
                    correctionValue = correction.y;
                }

                if (correctionValue < -25f)
                {
                    quat.y = firstRotation.z + quat.z;
                    quat.x = firstRotation.y - quat.y;
                }
                else if((correctionValue > 0f && correctionValue < 15f))
                {
                    quat.y = firstRotation.z - quat.z;
                    quat.x = firstRotation.y - quat.y;
                }
                else
                {
                    quat.y = firstRotation.y - quat.y;
                    quat.x = firstRotation.x + quat.x;
                }

                quat.w = firstRotation.w + quat.w;
                quat.z = 0;

                this.transform.rotation = quat;
            }
        }
    }
}
