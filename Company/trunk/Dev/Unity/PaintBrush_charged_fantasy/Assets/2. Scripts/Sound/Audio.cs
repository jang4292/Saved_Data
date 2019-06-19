using UnityEngine;

namespace Sound
{
    public class Audio : MonoBehaviour
    {
        /// <summary>
        /// 배경 음악
        /// </summary>
        public AudioSource bgmSound;
        /// <summary>
        /// 클릭 사운드
        /// </summary>
        public AudioSource clickSound;

        /// <summary>
        /// 생성된지 체크
        /// </summary>
        private static bool createClick;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            if (createClick)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                createClick = true;

                IsPlayerPrefs("BGM");
                IsPlayerPrefs("Click");

                if (PlayerPrefs.GetString("BGM").Equals("true"))
                {
                    bgmSound.Play();
                }
            }
        }

        void Update()
        {
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount != 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        ClickSoundStart();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ClickSoundStart();
                }
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        /// <summary>
        /// 체크 사운드 시작
        /// </summary>
        void ClickSoundStart()
        {
            if (PlayerPrefs.GetString("Click").Equals("true"))
            {
                clickSound.Play();
            }
        }

        /// <summary>
        /// 저장값이 있는지 확인하여, 없으면 True로 변환
        /// </summary>
        void IsPlayerPrefs(string key)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(key)))
            {
                PlayerPrefs.SetString(key, "true");
            }
        }
    }

}
