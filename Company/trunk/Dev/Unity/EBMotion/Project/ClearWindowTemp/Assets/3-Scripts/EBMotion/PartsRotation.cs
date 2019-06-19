using UnityEngine;
using ClearWindow;
using System.Collections;

namespace EBMotion
{
    public class PartsRotation : MonoBehaviour
    {
        /*
        private Quaternion firstRotation = Quaternion.identity;

        public int ID = 0;
        EbmDataParser ebmData;

        Quaternion quat;

        private GameManager gameManager = null;

        //天天天天天天天天天天 Overriding Method 天天天天天天天天天天//

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            ebmData = GameObject.Find("EbmDataParser").GetComponent("EbmDataParser") as EbmDataParser;
            firstRotation = this.transform.rotation;
        }

        void FixedUpdate()
        {
            if(gameManager.state == GameManager.MotionState.Right)
            {
                if(ID == 0 || ID == 1)
                {
                    SetRotation();
                }
            }
            else
            {
                if(ID == 2 || ID == 3)
                {
                    SetRotation();
                }
            }
            
        }

        //天天天天天天天天天天 Add Method 天天天天天天天天天天天//

        void SetRotation()
        {
            if (!ebmData.QuatData[ID].Equals(new Quaternion(0, 0, 0, 0)))
            {
                quat = ebmData.QuatData[ID];

                quat.x = firstRotation.x + quat.x;
                quat.y = firstRotation.y + quat.y;
                quat.z = quat.z + 90f;
                quat.w = firstRotation.w + quat.w;

                quat.z = 0;
                
                this.transform.rotation = quat;
            }
        }
        */

        private GameManager gameManager = null;

        private Quaternion firstRotation = Quaternion.identity;
        public int ID = 0;
        private EbmDataParser ebmData;
        private Quaternion quat;

        private static bool check = false;
        private static float correctionValue = 0;

        //天天天天天天天天天天 Overriding Method 天天天天天天天天天天//

        void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            ebmData = GameObject.Find("EbmDataParser").GetComponent<EbmDataParser>();
            firstRotation = this.transform.rotation;
            StartCoroutine(valueCheck());
        }

        void FixedUpdate()
        {
            //if (GameState.directionState == GameState.Direction.Right)
            if (gameManager.state == GameManager.MotionState.Right)
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


        //天天天天天天天天天天 Add Method 天天天天天天天天天天天//

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
                    if (correctionValue > 23f)
                    {
                        quat.z = firstRotation.z - quat.z;
                    }
                    else
                    {
                        quat.z = firstRotation.z + quat.z;
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
    }
}
