using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using Public;

namespace User
{
    public class PlayerRating : MonoBehaviour
    {

        /// <summary>
        /// 레이팅 그림들
        /// </summary>
        public Sprite[] ratingSprite;

        /// <summary>
        /// 레이팅 오브젝트 이미지
        /// </summary>
        private Image image;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            image = GetComponent<Image>();
            StartCoroutine(SetRating());
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        /// <summary>
        /// 경로
        /// </summary>
        /// <returns></returns>
        string SetDirPath()
        {
#if UNITY_ANDROID
            return Variables.moveFolderPath;
#else
            return Application.dataPath + "/screenShot";
#endif
        }

        /// <summary>
        /// Image File Read
        /// </summary>
        /// <returns></returns>
        public IEnumerator SetRating()
        {
            yield return new WaitForEndOfFrame();

            string dirPath = SetDirPath();

            /// 지정한 경로에 폴더가 존재 하는지 확인합니다.
            if (Directory.Exists(dirPath))
            {
                /// DirectoryInfo Class를 생성합니다.
                DirectoryInfo di = new DirectoryInfo(dirPath);

                /// 새내기 0 ~ 5
                if (di.GetFiles().Length < 6)
                {
                    image.sprite = ratingSprite[0];
                }
                /// 초보 6 ~ 10
                else if (di.GetFiles().Length > 5 && di.GetFiles().Length < 11)
                {
                    image.sprite = ratingSprite[1];
                }
                /// 일반 11 ~ 20
                else if (di.GetFiles().Length > 10 && di.GetFiles().Length < 21)
                {
                    image.sprite = ratingSprite[2];
                }
                /// 수석 21 ~ 30
                else if (di.GetFiles().Length > 20 && di.GetFiles().Length < 31)
                {
                    image.sprite = ratingSprite[3];
                }
                /// 전문가 31 ~
                else if (di.GetFiles().Length > 30)
                {
                    image.sprite = ratingSprite[4];
                }
            }
        }
    }
}
