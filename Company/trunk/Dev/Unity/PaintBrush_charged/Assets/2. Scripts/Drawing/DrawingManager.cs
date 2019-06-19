using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Api;
using Vo;
using AndroidPlugins;
using Public;
using CircularScrolling;

namespace Drawing
{
    [DisallowMultipleComponent]
    public class DrawingManager : MonoBehaviour
    {
        public enum DrawingState
        {
            Playing,
            Stop
        }

        public enum CharacterLoading
        {
            Idle,
            Start,
            Stop
        }

        /// <summary>
        /// 로딩상태
        /// </summary>
        public CharacterLoading loadingState;

        /// <summary>
        /// 그리는 상태
        /// </summary>
        public DrawingState drawingState;

        /// <summary>
        /// 브러쉬라인 프리팹
        /// </summary>
        public GameObject brushPrefab;

        /// <summary>
        /// 라인 프리팹
        /// </summary>
        public GameObject linePrefab;

        /// <summary>
        /// 그림을 그릴 지역
        /// </summary>
        public GameObject drawingAreaPrefab;

        /// <summary>
        /// 마우스
        /// </summary>
        public Transform mousePos;

        /// <summary>
        /// 색상 상태를 알려줄 스크립트
        /// </summary>
        public UIEvent colorState;

        /// <summary>
        /// The drawing area.
        /// </summary>
        public Transform drawingCanvasArea;

        /// <summary>
        /// 넓이 사이즈
        /// </summary>
        public ThicknessSize currentThickness;

        /// <summary>
        /// 넓이 사이즈 배열
        /// </summary>
        public Image[] thicknessSizeImages;

        /// <summary>
        /// Color UI 오브젝트
        /// </summary>
        public GameObject colorUI;

        /// <summary>
        /// 파레트
        /// </summary>
        public GameObject palette;

        /// <summary>
        /// 스크린샷 찍을 카메라
        /// </summary>
        public Camera screenShotCamera;

        /// <summary>
        /// 툴 상태를 확인해줄 스크립트
        /// </summary>
        public ToolBoxSmallBox toolState;

        /// <summary>
        /// 그리는 지역을 체크해줄 스프라이트렌더러
        /// </summary>
        private SpriteRenderer areaSpriteRenderer;

        /// <summary>
        /// 라인
        /// </summary>
        private MouseLine currentLine;

        /// <summary>
        /// 캐릭터 상속받고 있을 오브젝트
        /// </summary>
        private GameObject characterCnavas;

        /// <summary>
        /// 처음 그림 그릴 지역 및 그림을 그려줄 위치
        /// </summary>
        private Vector3 startPos = new Vector3(-1.5f, 0, 0);

        /// <summary>
        /// 가로 해상도
        /// </summary>
        private int resWidth;

        /// <summary>
        /// 세로 해상도
        /// </summary>
        private int resHeight;

        /// <summary>
        /// 그림 프리팹
        /// </summary>
        [System.NonSerialized]
        public GameObject characterPrefab;

        /// <summary>
        /// 앱 이름
        /// </summary>
        private const string APP_NAME = "paint_brush";

        private AssetBundle bundle;

        private StoreApi storeApi;

        private const int CAMERA_IMAGE = 9;

        /// <summary>
        /// 안드로이드 플러그인 매니저
        /// </summary>
        public AndroidPluginManager androidManager;

        [System.NonSerialized]
        public AssetBundleCreateRequest bundleLoadRequest;

        public GameObject loadingPopup;

        public ListArea listArea;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            /// 스크린 비율 16:9
            resWidth = Screen.width;
            float _height = Screen.width / 16f * 9;
            resHeight = (_height % 1f > 0) ? Mathf.RoundToInt(_height) : (int)_height;

            SetCharacterPrefab();
        }

        void Update()
        {
            if(listArea.state == ListArea.ListAreaState.Out && drawingState == DrawingState.Playing)
            {
                if (toolState.state == ToolBoxSmallBox.ToolState.Pencil || toolState.state == ToolBoxSmallBox.ToolState.Eraser)
                {
                    LineDrawing();
                }
                else if (toolState.state == ToolBoxSmallBox.ToolState.Brush)
                {
                    BrushDrawing();
                }
                else if (toolState.state == ToolBoxSmallBox.ToolState.Fill)
                {
                    UseFillFeature();
                }

                ColorUISetActive();
            }
        }

        void OnDestroy()
        {
            Area.charactersDrawingContents.Clear();

            if(bundle != null)
            {
                bundle.Unload(true);
                bundle = null;
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

#if UNITY_ANALYTICS

        private string customEventName = "Character";

        private Dictionary<string, object> data;

        /// <summary>
        /// 아날리틱스
        /// </summary>
        void LogAnalytics(string name)
        {
            if (data == null) data = new Dictionary<string, object>();
            data.Add(name, name);
            Analytics.CustomEvent(customEventName, data);
            data = null;
        }
#endif
        void SetCharacterPrefab()
        {
            characterPrefab = Resources.Load("Prefabs/Character/" + Variables.name) as GameObject;

            if (characterPrefab == null)
            {
                if (IsFileCheck())
                {
                    StartCoroutine(GetAssetBundleToObject(Variables.name));
                }
                else
                {
                    storeApi = new StoreApi(OnHttpRequest);
                    storeApi.PostSingleProductSearch(CommonUrl.PRODUCT_URL, API.API_PRODUCT_SINGLE_SEARCH, Variables.name);
                }
            }
            else
            {
                loadingPopup.SetActive(false);

                Init();
            }
        }

        void Init()
        {
            LogAnalytics(Variables.name);

            characterCnavas = GameObject.Find("CharacterCnavas");
            toolState.state = ToolBoxSmallBox.ToolState.Fill;
            InstantiateCharacters();
            InstantiateDrawingContents();
        }

        /// <summary>
        /// 파일이 존재 하는지 체크
        /// </summary>
        /// <returns></returns>
        private bool IsFileCheck()
        {
            string dirPath = Application.streamingAssetsPath + "/";

            if (File.Exists(dirPath + Variables.name + ".unity3d"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void OnHttpRequest(int id, WWW www)
        {
            if (www.error == null)
            {
                switch (id)
                {
                    case API.API_PRODUCT_SINGLE_SEARCH:
                        if (GetUserInfo(www.text).result)
                        {
                            GetFileDownloading(GetUserInfo(www.text));
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("[Error] : " + www.error);
            }

            storeApi.MinusRequest();
        }

        private SingleVo GetUserInfo(string json)
        {
            return JsonUtility.FromJson<SingleVo>(json);
        }

        private void GetFileDownloading(SingleVo singleVo)
        {
            List<StoreSingleListVo> vo = singleVo.lists;

            if (vo.Count != 0)
            {
                StartCoroutine(PutLocalAssetBundle(CommonUrl.BASE_URL + vo[0].imgRes, vo[0].name));
            }

        }

        /// <summary>
        /// 에셋 번들 로컬에 저장
        /// </summary>
        /// <param name="url">로컬 저장 위치</param>
        /// <param name="name">파일명</param>
        /// <returns></returns>
        IEnumerator PutLocalAssetBundle(string url, string name)
        {
            WWW www = new WWW(url);

            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                File.WriteAllBytes(Variables.assetBundlePath + name + ".unity3d", www.bytes);
                StartCoroutine(GetAssetBundleToObject(Variables.name));
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        /// <summary>
        /// 로컬에 저장된 에셋번들 불러와 프리팹으로 만들어줌
        /// </summary>
        /// <param name="name"></param>
        IEnumerator GetAssetBundleToObject(string name)
        {
            bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Variables.assetBundlePath, name + ".unity3d"));

            loadingState = CharacterLoading.Start;

            bundleLoadRequest.priority = 0;

            yield return bundleLoadRequest;

            bundle = bundleLoadRequest.assetBundle;
            if (bundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield break;
            }

            AssetBundleRequest request = bundle.LoadAssetAsync(name, typeof(Object));

            yield return request;

            characterPrefab = request.asset as GameObject;
            characterPrefab.name = Variables.name.Substring(3);
            characterPrefab.layer = CAMERA_IMAGE;

            Init();
        }

        void ColorUISetActive()
        {
            if(toolState.state == ToolBoxSmallBox.ToolState.Eraser)
            {
                if (colorUI.activeInHierarchy)
                {
                    colorUI.SetActive(false);
                    colorState.ColorReSet();
                }
            }
            else
            {
                if (!colorUI.activeInHierarchy)
                {
                    colorUI.SetActive(true);
                    colorState.PreviousReturnState();
                }
            }
        }

        /// <summary>
        /// 라인 그리기
        /// </summary>
        void LineDrawing()
        {
            // 삼성 노트 S pen 사용 시 플랫폼은 Android이나, Input은 Mouse로 받는다

            //Mobile Platform
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (DrawingAreaCheck())
                    {
                        LineFeatureOnClickBegan();
                    }

                }
            }
            //Others
            else if (Input.GetMouseButtonDown(0))
            {
                if (DrawingAreaCheck())
                {
                    LineFeatureOnClickBegan();
                }
            }
        }

        /// <summary>
        /// 라인 그리기
        /// </summary>
        void BrushDrawing()
        {
            // 삼성 노트 S pen 사용 시 플랫폼은 Android이나, Input은 Mouse로 받는다

            //Mobile Platform
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (DrawingAreaCheck())
                    {
                        BrushFeatureOnClickBegan();
                    }
                }
            }
            //Others
            else if (Input.GetMouseButtonDown(0))
            {
                if (DrawingAreaCheck())
                {
                    BrushFeatureOnClickBegan();
                }
            }
        }

        /// <summary>
        /// Use the fill feature.
        /// </summary>
        void UseFillFeature()
        {
            // 삼성 노트 S pen 사용 시 플랫폼은 Android이나, Input은 Mouse로 받는다

            //Mobile Platform
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (DrawingAreaCheck())
                    {
                        FillFeatureOnClickBegan();
                    }
                }
            }
            //Others
            else if (Input.GetMouseButtonDown(0))
            {
                if (DrawingAreaCheck())
                {
                    FillFeatureOnClickBegan();
                }
            }
        }

        /// <summary>
        /// 그리는 지역 체크
        /// </summary>
        /// <returns></returns>
        public bool DrawingAreaCheck()
        {
            if (areaSpriteRenderer == null) areaSpriteRenderer = GameObject.Find("DrawingArea").GetComponent<SpriteRenderer>();

            if (areaSpriteRenderer.bounds.Contains(mousePos.position))
            {
                return true;
            }

            return false;
        }

        public float ToolThicknessSize()
        {
            float size = 1.0f;

            switch (toolState.state)
            {
                case ToolBoxSmallBox.ToolState.Pencil:
                    size = 1.0f;
                    break;
                case ToolBoxSmallBox.ToolState.Brush:
                    size = 1.5f;
                    break;
                case ToolBoxSmallBox.ToolState.Eraser:
                    size = 1.0f;
                    break;
            }

            return size;
        }

        /// <summary>
        /// Line feature on click began.
        /// </summary>
        private void LineFeatureOnClickBegan()
        {
            ///Create new line gameobject
            GameObject line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity) as GameObject;

            GameObject character = GameObject.Find(characterPrefab.name + " Contents");

            ///Set the parent of line
            line.transform.SetParent(character.transform);

            ///Set the name of the line
            line.name = "Line";

            ///Get the Line component
            currentLine = line.GetComponent<MouseLine>();

            ///Increase current sorting order
            Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder++;
            if (Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder <= Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder)
            {
                Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder = Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder + 1;
            }

            /// Add the element to history
            History.Element element = new History.Element();
            element.transform = line.transform;
            element.type = History.Element.EType.Object;
            element.sortingOrder = Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder;
            Area.charactersDrawingContents[Character.currentCharacterIndex].GetComponent<History>().AddToPool(element);
            
            ///Set the sorting order of the line (last line must be on the top)
            currentLine.SetSortingOrder(Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder);

            if (currentThickness != null)
            {
                currentLine.SetWidth(currentThickness.value * ToolThicknessSize(), currentThickness.value * ToolThicknessSize());
            }
        }

        /// <summary>
        /// Line feature on click began.
        /// </summary>
        private void BrushFeatureOnClickBegan()
        {
            ///Create new line gameobject
            GameObject line = Instantiate(brushPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            GameObject character = GameObject.Find(characterPrefab.name + " Contents");

            ///Set the parent of line
            line.transform.SetParent(character.transform);

            ///Set the name of the line
            line.name = "Line";

            ///Get the Line component
            currentLine = line.GetComponent<MouseLine>();

            ///Increase current sorting order
            Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder++;
            if (Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder <= Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder)
            {
                Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder = Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder + 1;
            }

            /// Add the element to history
            History.Element element = new History.Element();
            element.transform = line.transform;
            element.type = History.Element.EType.Object;
            element.sortingOrder = Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder;
            Area.charactersDrawingContents[Character.currentCharacterIndex].GetComponent<History>().AddToPool(element);

            ///Set the sorting order of the line (last line must be on the top)
            currentLine.SetSortingOrder(Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder);

            if (currentThickness != null)
            {
                currentLine.SetWidth(currentThickness.value * ToolThicknessSize(), currentThickness.value * ToolThicknessSize());
            }
        }

        /// <summary>
        /// Line feature on click released.
        /// </summary>
        private void LineFeatureOnClickReleased()
        {
            if (currentLine != null)
            {
                if (currentLine.GetPointsCount() <= 2)
                {
                    Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder--;
                    Area.charactersDrawingContents[Character.currentCharacterIndex].GetComponent<History>().RemoveLastElement();
                    Destroy(currentLine.gameObject);
                }
            }

            ///Release current line
            currentLine = null;
        }

        /// <summary>
        /// Instantiate the drawing contents for each Character.
        /// </summary>
        private void InstantiateDrawingContents()
        {
            if (Area.charactersDrawingContents.Count == 0)
            {
                if (characterPrefab == null)
                {
                    return;
                }
                GameObject drawingContents = new GameObject(characterPrefab.name + " Contents");
                drawingContents.layer = LayerMask.NameToLayer("Image");
                DrawingContents drawingContentsComponent = drawingContents.AddComponent(typeof(DrawingContents)) as DrawingContents;
                drawingContents.AddComponent(typeof(History));
                drawingContents.transform.SetParent(drawingCanvasArea);

                Transform characterParts = characterPrefab.transform.Find("Parts");
                if (characterParts != null)
                {
                    foreach (Transform part in characterParts)
                    {
                        if (part.GetComponent<SpriteRenderer>() != null)
                        {
                            drawingContentsComponent.characterPartsColors.Add(part.name, part.GetComponent<SpriteRenderer>().color);
                            drawingContentsComponent.characterPartsSortingOrder.Add(part.name, part.GetComponent<SpriteRenderer>().sortingOrder);
                        }
                    }
                }
                Area.charactersDrawingContents.Add(drawingContentsComponent);
            }
        }

        /// <summary>
        /// Instantiate the characters.
        /// </summary>
        public void InstantiateCharacters()
        {
            if (characterPrefab == null)
            {
                Debug.LogError("Characters Container is undefined");
                return;
            }
            GameObject area = Instantiate(drawingAreaPrefab, startPos, Quaternion.identity) as GameObject;
            area.transform.SetParent(characterCnavas.transform);//set the parent of the character
            area.name = drawingAreaPrefab.name;
            GameObject character = Instantiate(characterPrefab, startPos, Quaternion.identity) as GameObject;

            if(Variables.characterNames.IndexOf(characterPrefab.name) + 1 < Variables.characterNames.Count)
            {
                PlayerPrefs.SetString(Variables.characterNames[Variables.characterNames.IndexOf(characterPrefab.name) + 1], "true");
            }

            character.name = characterPrefab.name;//set the name of the character
            character.transform.SetParent(characterCnavas.transform);//set the parent of the character
        }

        /// <summary>
        /// fill feature on click began.
        /// </summary>
        private void FillFeatureOnClickBegan()
        {
            RaycastHit2D hit2d = Physics2D.Raycast(GetCurrentPlatformClickPosition(), Vector2.zero);
            if (hit2d.collider != null)
            {
                CharacterPart characterPart = hit2d.collider.GetComponent<CharacterPart>();//get the CharacterPart component
                if (characterPart != null)
                {//Character Part
                    SpriteRenderer spriteRenderer = hit2d.collider.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        if (characterPart.targetColor != colorState.SetColorState(colorState.state))
                        {

                            ///Apply and save the attributes (Color,sortingOrder,lastPartSortingOrder)
                            characterPart.SetColor(colorState.SetColorState(colorState.state));
                            Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsColors[hit2d.collider.name] = colorState.SetColorState(colorState.state);
                            spriteRenderer.sortingOrder = Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder + 1;
                            Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsSortingOrder[hit2d.collider.name] = spriteRenderer.sortingOrder;
                            Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder = spriteRenderer.sortingOrder;

                            ///Add the element to history
                            History.Element element = new History.Element();
                            element.transform = hit2d.collider.transform;
                            element.type = History.Element.EType.Color;
                            element.color = colorState.SetColorState(colorState.state);
                            element.sortingOrder = spriteRenderer.sortingOrder;
                            Area.charactersDrawingContents[Character.currentCharacterIndex].GetComponent<History>().AddToPool(element);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the current platform click position.
        /// </summary>
        /// <returns>The current platform click position.</returns>
        private Vector3 GetCurrentPlatformClickPosition()
        {
            Vector3 clickPosition = Vector3.zero;

            //current platform is mobile
            if (Input.touchCount != 0)
            {
                Touch touch = Input.GetTouch(0);
                clickPosition = touch.position;
            }
            //others
            else
            {
                clickPosition = Input.mousePosition;
            }

            clickPosition = Camera.main.ScreenToWorldPoint(clickPosition);//get click position in the world space
            clickPosition.z = 0;
            return clickPosition;
        }

        /// <summary>
        /// Change the color of the thickness size (circles).
        /// </summary>
        public void ChangeThicknessSizeColor()
        {
            if (thicknessSizeImages != null)
            {
                Color thicknessSizeColor = colorState.SetColorState(colorState.state);
                thicknessSizeColor.a = 1;

                foreach (Image img in thicknessSizeImages)
                {
                    img.color = thicknessSizeColor;
                    if (img.gameObject.GetInstanceID() == currentThickness.gameObject.GetInstanceID())
                    {
                        currentThickness.EnableSelection();
                    }
                }
            }
        }

        /// <summary>
        /// PaletteDisable
        /// </summary>
        public void PaletteDisable()
        {
            palette.SetActive(false);
        }

        /// <summary>
        /// ClearCharacter
        /// </summary>
        public void ClearCharacter()
        {
            //CleanCurrentCharacterScreen();
            Transform characterParts = GameObject.Find(characterPrefab.name).transform.Find("Parts");
            if (characterParts != null)
            {
                int test = Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder + 1;

                foreach (Transform part in characterParts)
                {
                    SpriteRenderer spriteRen = part.GetComponent<SpriteRenderer>();
                    Collider2D collider = part.GetComponent<Collider2D>();

                    if (spriteRen != null)
                    {
                        spriteRen.color = Color.white;

                        Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsColors[collider.name] = Color.white;
                        spriteRen.sortingOrder = test;
                        Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsSortingOrder[collider.name] = test;
                        Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder = test;

                        
                    }
                }

                ///Add the element to history
                History.Element element = new History.Element();
                element.transform = characterParts;
                element.type = History.Element.EType.Color;
                element.color = Color.white;
                element.sortingOrder = test;
                Area.charactersDrawingContents[Character.currentCharacterIndex].GetComponent<History>().AddToPool(element);
            }
        }

        /// <summary>
        /// Clean the screen.
        /// </summary>
        public void CleanCurrentCharacterScreen()
        {
            if (Area.charactersDrawingContents[Character.currentCharacterIndex] == null)
            {
                return;
            }

            //Clean the history for the current character
            Area.charactersDrawingContents[Character.currentCharacterIndex].GetComponent<History>().CleanPool();

            //Remove all the childern in drawContents
            foreach (Transform child in Area.charactersDrawingContents[Character.currentCharacterIndex].transform)
            {
                Destroy(child.gameObject);
            }

            Transform characterParts = GameObject.Find(characterPrefab.name).transform.Find("Parts");
            if (characterParts != null)
            {
                foreach (Transform part in characterParts)
                {
                    part.GetComponent<SpriteRenderer>().color = Color.white;
                    Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsColors[part.name] = Color.white;
                    part.GetComponent<CharacterPart>().ApplyInitialSortingOrder();
                    part.GetComponent<CharacterPart>().ApplyInitialColor();
                    Area.charactersDrawingContents[Character.currentCharacterIndex].characterPartsSortingOrder[part.name] = part.GetComponent<CharacterPart>().initialSortingOrder;
                }
            }

            Area.charactersDrawingContents[Character.currentCharacterIndex].currentSortingOrder = 0;
            Area.charactersDrawingContents[Character.currentCharacterIndex].lastPartSortingOrder = 0;
        }

        /// <summary>
        /// 저장할 이름
        /// </summary>
        /// <param name="width">가로해상도</param>
        /// <param name="height">세로해상도</param>
        /// <returns></returns>
        public string ScreenShotName(int width, int height)
        {
#if UNITY_ANDROID
            return string.Format("{0}/" + APP_NAME + "_{1}x{2}_{3}.png",
                                 Variables.moveFolderPath,
                                 width, height,
                                 System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
#endif
        }

        /// <summary>
        /// 스크린 샷
        /// </summary>
        /// <returns></returns>
        public IEnumerator ScreenShot()
        {
            /// 모든 렌더링작업이 끝날 때까지 대기
            yield return new WaitForEndOfFrame();

            /// RenderTexture 포맷
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            /// 카메라 타겟텍스처 설정
            screenShotCamera.targetTexture = rt;
            /// Texture 포맷
            Texture2D screenShot = new Texture2D(screenShotCamera.pixelWidth, screenShotCamera.pixelHeight, TextureFormat.RGB24, false);
            screenShotCamera.Render();
            RenderTexture.active = rt;
            /// 현재 화면을 픽셀 단위로 읽음
            screenShot.ReadPixels(screenShotCamera.pixelRect, 0, 0);
            /// 오류가 발생 안되도록 추가
            screenShotCamera.targetTexture = null;
            RenderTexture.active = null;
            /// 사용이 끝난 텍스쳐 메모리에서 바로 해제
            DestroyImmediate(rt);
            /// 읽어 드린 픽셀을 Byte[]에 PNG 형식으로 인코딩
            byte[] bytes = screenShot.EncodeToPNG();
            /// 파일 이름
            string filename = ScreenShotName(screenShotCamera.pixelWidth, screenShotCamera.pixelHeight);

            /// 파일 저장
            File.WriteAllBytes(filename, bytes);

            /// 갤러리에서 파일 리플레쉬
            androidManager.GetGalleryFileRefresh(filename);
        }
    }
}
