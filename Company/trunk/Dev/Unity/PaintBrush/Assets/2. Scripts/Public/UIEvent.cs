using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Drawing;
using Sound;
using AndroidPlugins;

namespace Public
{
    public class UIEvent : MonoBehaviour
    {
        //private const string PERSONAL_INFORMATION_URL = @"https://mampcorp.appspot.com/term.html?pk=com.mamp.paintbrush";

        public enum ColorState
        {
            Red, Skin, White,
            Yellow, Brown, Gray,
            Orange, Green, Black,
            Pink, Blue, Palette
        }

        [SerializeField]
        private AndroidPluginManager androidPluginManager;

        public GameObject[] colorObjs = null;
        public Sprite[] deSeletedImages = null;
        public Sprite[] seletedImages = null;
        public ChangeColor changeColor;

        /// <summary>
        /// 0 - 비선택, 1 - 선택
        /// </summary>
        public Sprite[] settingImages;

        /// <summary>
        /// 0 - 왼쪽, 1 - 오른쪽
        /// </summary>
        public Image[] handSetting;
        public Text[] handTexts;

        /// <summary>
        /// 0 - 켜기, 1 - 끄기
        /// </summary>
        public Image[] bgmSetting;
        public Text[] bgmTexts;
        /// <summary>
        /// 0 - 켜기, 1 - 끄기
        /// </summary>
        public Image[] soundEffetSetting;
        public Text[] soundEffetTexts;

        public ColorState state;

        private ColorState[] stateArray = {
            ColorState.Red, ColorState.Skin, ColorState.White,
            ColorState.Yellow, ColorState.Brown, ColorState.Gray,
            ColorState.Orange, ColorState.Green, ColorState.Black,
            ColorState.Pink, ColorState.Blue, ColorState.Palette
        };

        /// <summary>
        /// 이전 색상 컬러 상태
        /// </summary>
        private string previousState;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Call Back Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            /// 스크린 비율 16:9
            float _height = Screen.width / 16f * 9;

            /// (가로, 세로(_height 1로 나눈 나머지 값이 0보다 크면 소수점을 자름), 전체화면을 할지 true or false) 
            Screen.SetResolution(Screen.width, (_height % 1f > 0) ? Mathf.RoundToInt(_height) : (int)_height, true);
        }

        void Start()
        {
            if (colorObjs.Length != 0)
            {
                state = ColorState.Red;
                colorAddToSpirte(colorObjs[0].name);
            }

            if (bgmSetting.Length != 0)
            {
                if (PlayerPrefs.GetString("BGM").Equals("true"))
                {
                    SetSettingChange(bgmSetting, bgmTexts, bgmSetting[0]);
                }
                else
                {
                    SetSettingChange(bgmSetting, bgmTexts, bgmSetting[1]);
                }

                if (PlayerPrefs.GetString("Click").Equals("true"))
                {
                    SetSettingChange(soundEffetSetting, soundEffetTexts, soundEffetSetting[0]);
                }
                else
                {
                    SetSettingChange(soundEffetSetting, soundEffetTexts, soundEffetSetting[1]);
                }
            }
        }
        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        public void ColorReSet()
        {
            state = ColorState.White;
            for (int i = 0; i < colorObjs.Length; i++)
            {
                Image image = colorObjs[i].GetComponent<Image>();
                RectTransform rect = colorObjs[i].GetComponent<RectTransform>();
                rect.localScale = Vector3.one;
                image.sprite = deSeletedImages[i];
            }

            DrawingManager drawingManager = GameObject.FindObjectOfType<DrawingManager>();

            drawingManager.ChangeThicknessSizeColor();
        }

        public void PreviousReturnState()
        {
            colorAddToSpirte(previousState);
        }

        public void colorAddToSpirte(string nowObj)
        {
            previousState = nowObj;

            for (int i = 0; i < colorObjs.Length; i++)
            {
                Image image = colorObjs[i].GetComponent<Image>();
                RectTransform rect = colorObjs[i].GetComponent<RectTransform>();
                if (colorObjs[i].name.Equals(nowObj))
                {
                    rect.localScale = new Vector3(1.5f, 1.5f, 1);
                    image.sprite = seletedImages[i];
                    state = stateArray[i];
                }
                else
                {
                    rect.localScale = Vector3.one;
                    image.sprite = deSeletedImages[i];
                }
            }

            DrawingManager drawingManager = GameObject.FindObjectOfType<DrawingManager>();

            drawingManager.ChangeThicknessSizeColor();
        }

        public void PaletteEnable(GameObject palette)
        {
            DrawingManager drawingManager = GameObject.FindObjectOfType<DrawingManager>();
            drawingManager.drawingState = DrawingManager.DrawingState.Stop;
            palette.SetActive(true);
        }

        /// <summary>
        /// 선택될 색상에 따른 Hex Color 반환
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public Color SetColorState(ColorState state)
        {
            Color color = Color.white;

            switch (state)
            {
                case ColorState.Red:
                    ColorUtility.TryParseHtmlString("#b11e23", out color);
                    break;
                case ColorState.Skin:
                    ColorUtility.TryParseHtmlString("#fed6b3", out color);
                    break;
                case ColorState.White:
                    ColorUtility.TryParseHtmlString("#ffffff", out color);
                    break;
                case ColorState.Yellow:
                    ColorUtility.TryParseHtmlString("#ffce02", out color);
                    break;
                case ColorState.Brown:
                    ColorUtility.TryParseHtmlString("#b87645", out color);
                    break;
                case ColorState.Gray:
                    ColorUtility.TryParseHtmlString("#c7c7c7", out color);
                    break;
                case ColorState.Orange:
                    ColorUtility.TryParseHtmlString("#f99c2e", out color);
                    break;
                case ColorState.Green:
                    ColorUtility.TryParseHtmlString("#499052", out color);
                    break;
                case ColorState.Black:
                    ColorUtility.TryParseHtmlString("#000000", out color);
                    break;
                case ColorState.Pink:
                    ColorUtility.TryParseHtmlString("#f5a59e", out color);
                    break;
                case ColorState.Blue:
                    ColorUtility.TryParseHtmlString("#367dbf", out color);
                    break;
                case ColorState.Palette:
                    color = changeColor.color;
                    break;
            }

            return color;
        }

        public void UndoClickEvent()
        {
            History history = GameObject.FindObjectOfType<History>();
            if (history != null)
            {
                history.UnDo();
            }
        }

        public void RedoClickEvent()
        {
            History history = GameObject.FindObjectOfType<History>();
            if (history != null)
            {
                history.Redo();
            }
        }

        public void ThicknessSizeEvent(ThicknessSize thicknessSize)
        {
            if (thicknessSize == null)
            {
                return;
            }

            ToolBoxSmallBox toolState = GameObject.FindObjectOfType<ToolBoxSmallBox>();

            DrawingManager drawingMananger = GameObject.FindObjectOfType<DrawingManager>();

            if (toolState.state == ToolBoxSmallBox.ToolState.Fill)
            {
                return;
            }

            drawingMananger.currentThickness = thicknessSize;
            drawingMananger.ChangeThicknessSizeColor();
        }

        public void Clean(GameObject nowObj)
        {
            DrawingManager drawingManager = GameObject.FindObjectOfType<DrawingManager>();
            drawingManager.CleanCurrentCharacterScreen();

            SetObjectDisable(nowObj);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void Setting()
        {
            SceneManager.LoadScene("Setting");
        }

        public void Home()
        {
            SceneManager.LoadScene("Main");
        }

        public void Game()
        {
            SceneManager.LoadScene("Game");
        }

        public void Album()
        {
            SceneManager.LoadScene("Album");
        }

        public void Store()
        {
            SceneManager.LoadScene("Store");
        }

        public Color SetSettingColor()
        {
            Color color;

            ColorUtility.TryParseHtmlString("#B8B8B8", out color);

            return color;
        }

        public void SetSettingChange(Image[] nowImage, Text[] text, Image setting)
        {
            /// 왼쪽
            if (nowImage[0].name.Equals(setting.name))
            {
                nowImage[0].sprite = settingImages[1];
                nowImage[1].sprite = settingImages[0];

                text[0].color = Color.white;
                text[1].color = SetSettingColor();
            }
            /// 오른쪽
            else
            {
                nowImage[0].sprite = settingImages[0];
                nowImage[1].sprite = settingImages[1];

                text[0].color = SetSettingColor();
                text[1].color = Color.white;
            }
        }

        public void SetHandSetting(Image setting)
        {
            SetSettingChange(handSetting, handTexts, setting);
        }

        public void SetBGMSetting(Image setting)
        {
            SetSettingChange(bgmSetting, bgmTexts, setting);
        }

        public void SetSoundEffetSetting(Image setting)
        {
            SetSettingChange(soundEffetSetting, soundEffetTexts, setting);
        }

        public void SetPlayerPrefsTrue(string key)
        {
            PlayerPrefs.SetString(key, "true");
        }

        public void SetPlayerPrefsFalse(string key)
        {
            PlayerPrefs.SetString(key, "false");
        }

        public void SetBGMPlay()
        {
            Audio audio = GameObject.FindObjectOfType<Audio>();
            if(!audio.bgmSound.isPlaying) audio.bgmSound.Play();
        }

        public void SetBGMStop()
        {
            Audio audio = GameObject.FindObjectOfType<Audio>();
            if (audio.bgmSound.isPlaying) audio.bgmSound.Stop();
        }

        public void SetObjectEnable(GameObject nowObj)
        {
            DrawingManager drawingMananger = GameObject.FindObjectOfType<DrawingManager>();
            if (drawingMananger != null) drawingMananger.drawingState = DrawingManager.DrawingState.Stop;

            nowObj.SetActive(true);
        }

        public void SetObjectDisable(GameObject nowObj)
        {
            DrawingManager drawingMananger = GameObject.FindObjectOfType<DrawingManager>();

            if(drawingMananger != null) drawingMananger.drawingState = DrawingManager.DrawingState.Playing;

            nowObj.SetActive(false);
        }

        public void ScreenShot()
        {
            DrawingManager drawingMananger = GameObject.FindObjectOfType<DrawingManager>();
            StartCoroutine(drawingMananger.ScreenShot());

            Home();
        }

        public void OpenNativeWebView()
        {
            Debug.Log("OpenNativeWebView()");
            androidPluginManager.OpenNativeWebView(Api.CommonUrl.PERSONAL_INFORMATION_URL);
        }
    }
}
