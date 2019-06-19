using UnityEngine;
using EBMotion;
using System.Collections;

namespace BaseBall
{
    public class PartsRotation : MonoBehaviour
    {
        private Quaternion firstRotation = Quaternion.identity;
        public int ID = 0;
        private EbmDataParser ebmData;
        private Quaternion quat;

        private static bool check = false;
        private static float correctionValue = 0;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            ebmData = GameObject.Find("EbmDataParser").GetComponent<EbmDataParser>();
            firstRotation = this.transform.rotation;
            StartCoroutine(valueCheck());
        }

        void FixedUpdate()
        {
            if (GameState.directionState == GameState.Direction.Right)
            {
                if (ID == 0 || ID == 1)
                {
                    SetRightRotation();
                }
            }
            else
            {
                if (ID == 2 || ID == 3)
                {
                    SetLeftRotation();
                }
            }

        }

        void OnDestroy()
        {
            correctionValue = 0;
        }


        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        IEnumerator valueCheck()
        {
            yield return new WaitForSeconds(2f);
            check = true;
        }

        void SetRightRotation()
        {
            if (!ebmData.QuatData[ID].Equals(new Quaternion(0, 0, 0, 0)))
            {
                quat = ebmData.QuatData[ID];
                Vector3 correction = ebmData.correction[1];

                if (correction != Vector3.zero && correctionValue == 0 && check)
                {
                    correctionValue = correction.x;
                }

                if (correctionValue > 0f)
                {
                    if (correctionValue < 30f)
                    {
                        quat.z = firstRotation.z + quat.z;
                    }
                    else
                    {
                        quat.z = firstRotation.z - quat.z;
                    }

                    quat.x = firstRotation.x - quat.x;
                }
                else
                {
                    if (correctionValue > -14f)
                    {
                        quat.z = firstRotation.z - quat.z;
                    }
                    else
                    {
                        quat.z = firstRotation.z + quat.z;
                    }
                    quat.x = firstRotation.x + quat.x;
                }

                quat.y = firstRotation.y + quat.y;
                quat.w = firstRotation.w + quat.w;

                this.transform.rotation = quat;
            }
        }


        void SetLeftRotation()
        {
            if (!ebmData.QuatData[ID].Equals(new Quaternion(0, 0, 0, 0)))
            {
                quat = ebmData.QuatData[ID];
                Vector3 correction = ebmData.correction[3];

                if (correction != Vector3.zero && correctionValue == 0)
                {
                    correctionValue = correction.x;
                }

                if (correctionValue > 0f)
                {
                    if(correctionValue > 23f)
                    {
                        quat.z = firstRotation.z - quat.z;
                    }else
                    {
                        quat.z = firstRotation.z + quat.z;
                    }

                    quat.x = firstRotation.x - quat.x;
                }
                else
                {
                    if(correctionValue > -14f)
                    {
                        quat.z = firstRotation.z - quat.z;
                    }else
                    {
                        quat.z = firstRotation.z + quat.z;
                    }
                    quat.x = firstRotation.x + quat.x;
                }

                quat.y = firstRotation.y + quat.y;
                quat.w = firstRotation.w + quat.w;

                this.transform.rotation = quat;
            }
        }
    }
}