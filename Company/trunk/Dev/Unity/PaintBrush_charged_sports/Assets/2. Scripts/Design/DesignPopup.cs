using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Api;
using Vo;
using Public;

namespace Design
{
    public class DesignPopup : MonoBehaviour
    {
        private enum PageState
        {
            Idle,
            Pageing
        }

        private PageState pageState;

        /// <summary>
        /// 캐릭터 잠금 이미지
        /// </summary>
        public GameObject[] designObjs; //Unused

        /// <summary>
        /// 캐릭터 이미지들
        /// </summary>
        public Image[] characterObjs; //Unused

        /// <summary>
        /// 캐릭터 스프라이트
        /// </summary>
        public Sprite[] characterSpirtes; //Unused

        /// <summary>
        /// 선택된 이미지
        /// </summary>
        public Image seletedImage;

        /// <summary>
        /// 잠금 이미지
        /// </summary>
        public Sprite lockImage; //Unused

        public Sprite premiumLockImage; //Unused

        public GameObject premiumPartPrefab;

        public GameObject filmArea;

        /// <summary>
        /// 이미지 폭
        /// </summary>
        private const int IMAGE_WIDTH = 221;
        /// <summary>
        /// 이미지 높이
        /// </summary>
        private const int IMAGE_HEIGHT = 221;

        private RectTransform designSize;
        private Image designImage;
        private Button designButton;

        private WWWHelper helper;

        private GameObject premiumPart;
        private Image premLockImage;
        private Button premLockButton;

        private StoreApi storeApi;

        /// <summary>
        /// 서버에서 불러올 페이지 숫자
        /// </summary>
        private string pageingCount = "0";
        /// <summary>
        /// 서버에서 불러올 페이지당 갯수
        /// </summary>
        //private string pageViewCount = "50";
        private string pageViewCount = "60";

        /// <summary>
        /// 서버에 총 등록된 단일 그림 갯수
        /// </summary>
        private string singleTotalCnt;

        /// <summary>
        /// 총 등록된 오브젝트 갯수
        /// </summary>
        private int addObjectCount = 0;

        private List<StoreSingleListVo> premiumList;

        private float scrollCheckValue = 0.9f;

        public GameObject[] newItemsObjs;

        public Image[] newItemsImages;

        public GameObject newLabelObj;

        public GridLayoutGroup grid;

        public GameObject pageLoadingPopup;

        private int newItemCount = 0;

        public ScrollRect scrollRect;

        public Button button;
        public Sprite disableImage;
        public Sprite normalImage;

        public Sprite noImage;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Awake()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("cat")))
            {
                PlayerPrefs.SetString("cat", "true");
            }
        }

        void Start()
        {
            Variables.name = "cat";
            //PutImageSprite();

            pageLoadingPopup.SetActive(true);

            storeApi = new StoreApi(OnHttpRequest);


            storeApi.PostCheckedThemaNumber(CommonUrl.PRODUCT_THEMA_URL, API.API_PRODUCT_THEMA_NUMBER);

            //storeApi.PostCheckedThemaNumber(CommonUrl.PRODUCT_URL, API.API_PRODUCT_THEMA_NUMBER);


            //Unused
            //storeApi.PostItemsListInThema11(CommonUrl.PRODUCT_THEMA_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);
            //storeApi.PostUserNewItemList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_DESIGN_NEW_LIST, "3");

            //seletedImage.sprite = noImage;
            //button.enabled = false;
            //button.image.sprite = disableImage;
        }

        private void OnEnable()
        {
            seletedImage.sprite = noImage;
            button.enabled = false;
            button.image.sprite = disableImage;

        }

        void LateUpdate()
        {
            //if (gameObject.activeSelf) SetLinePosition();
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        /// <summary>
        /// 이미지 추가
        /// </summary>
        
        /*
        void PutImageSprite()
        {
            if (characterSpirtes.Length != 0)
            {
                for (int i = 0; i < characterSpirtes.Length; i++)
                {
                    RectTransform designSize = designObjs[i].GetComponent<RectTransform>();
                    Image designImage = designObjs[i].GetComponent<Image>();
                    Button designButton = designObjs[i].GetComponent<Button>();

                    characterObjs[i].sprite = characterSpirtes[i];

                    
                    //if (CharactersCheck()[i])
                    //if(CharactersCheck2(i))
                    {
                        designButton.enabled = false;
                        designImage.enabled = false;
                    }
                    //else
                    //{
                      //  Debug.Log("lockImage");
                       // designButton.interactable = false;
                        //designImage.sprite = lockImage;
                    //}
                    

                    designSize.sizeDelta = new Vector2(IMAGE_WIDTH, IMAGE_HEIGHT);
                }
            }
        }
        */
        private SingleVo GetSingleVo(string jsonData)
        {
            return JsonUtility.FromJson<SingleVo>(jsonData);
        }

        private ThemaVo getThemaVo(string jsonData)
        {
            return JsonUtility.FromJson<ThemaVo>(jsonData);
        }

        private bool IsResult(string json)
        {
            if (JsonUtility.FromJson<SingleVo>(json) == null) return false;

            return JsonUtility.FromJson<SingleVo>(json).result;
        }

        private string GetTotalCount(string json)
        {
            return JsonUtility.FromJson<SingleVo>(json).totalCnt;
        }

        void OnHttpRequest(int id, WWW www)
        {

            //Debug.Log("id : " + id);
            //Debug.Log("www : " + www);

            if (www.error == null)
            {
                switch (id)
                {
                    /*
                    case API.API_PRODUCT_DESIGN_NEW_LIST:

                        if (IsResult(www.text))
                        {
                            StartCoroutine(GetNewItem(www.text));
                        }
                        //TODO
                        //storeApi.PostStoreBuySingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);

                        //storeApi.PostItemsListInThema11(CommonUrl.PRODUCT_THEMA_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);
                        //storeApi.PostItemsListInThema(CommonUrl.PRODUCT_THEMA_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);

                        break;
                        */
                    case API.API_PRODUCT_DESING_BUY_LIST:

                        //Debug.Log("www2 : " + www);
                        //Debug.Log("www.text2 : " + www.text);
                        //if (IsResult(www.text))
                        //{
                        //scrollRect.inertia = false;

                        StartCoroutine(PutPremiumPart(www.text));
                        //singleTotalCnt = GetTotalCount(www.text);
                        //}
                        //else
                        //{
                        //pageLoadingPopup.SetActive(false);
                        //}

                        //StartCoroutine(GetNewItem(www.text));

                        break;

                    case API.API_PRODUCT_THEMA_NUMBER:
                        //Debug.Log("www1 : " + www);
                        //Debug.Log("www.text1 : " + www.text);



                        //Debug.Log("www.text :: " + www.text);
                        ThemaVo vo = getThemaVo(www.text);

                        int themaNumber = getThemaNumber(vo);
                        //Debug.Log("themaNumber :: " + themaNumber);
                        if (themaNumber != -1) {
                            storeApi.PostItemsListInThema(CommonUrl.PRODUCT_THEMA_URL, API.API_PRODUCT_DESING_BUY_LIST, themaNumber, pageingCount, pageViewCount);
                        }
                        else
                        {
                            
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

        //        private const string THEMA_NAME = "올인";
        //private const string THEMA_NAME = "파티";
        //private const string THEMA_NAME = "판타지";
        private const string THEMA_NAME = "스포츠";
        //private const string THEMA_NAME = "사극";
        //private const string THEMA_NAME = "할로윈";
        private int getThemaNumber(ThemaVo vo)
        {
            
            foreach(ThemaItemVo v in vo.lists)
            {

          //      Debug.Log("Test :  " + v.name);
                //Debug.Log("v.name :: " + v.name);
                //Debug.Log("v.tidx :: " + v.tidx);
                if (v.name.Equals(THEMA_NAME))
                {
                    //return 1;
                    return v.tidx;
                }
            }

            return -1;
        }

        /// <summary>
        /// 프리미엄 상품추가
        /// </summary>
        
            
        IEnumerator PutPremiumPart(string jsonData)
        {
            //Debug.Log("jsonData : " + jsonData);
            premiumList = GetSingleVo(jsonData).lists;

            //Debug.Log("premiumList.Count : " + premiumList.Count);
            
            for (int i = 0; i < premiumList.Count; i++)
            {
                premiumPart = Instantiate(premiumPartPrefab) as GameObject;
                premiumPart.name = premiumList[i].name;
                Image premCharacterImage = premiumPart.transform.Find("CharacterImage").GetComponent<Image>();

                WWW premWWW = new WWW(CommonUrl.BASE_URL + premiumList[i].imgThum.Substring(2));

                yield return premWWW;

                if (premWWW.error == null)
                {
                    Rect premLockRect = new Rect(0, 0, premWWW.texture.width, premWWW.texture.height);
                    Sprite sprite = Sprite.Create(premWWW.texture, premLockRect, Vector2.zero);
                    sprite.name = premiumList[i].name;
                    premCharacterImage.sprite = sprite;

                    premiumPart.transform.SetParent(filmArea.transform);
                    premiumPart.transform.localScale = filmArea.transform.localScale;

                    premiumPart.transform.SetSiblingIndex(addObjectCount);

                    addObjectCount++;
                }
                else
                {
                    Debug.Log("[Error] : " + premWWW.error);
                }

                premWWW.Dispose();
            }

            pageLoadingPopup.SetActive(false);
            scrollRect.inertia = true;
            pageState = PageState.Idle;
        }
        
        
        IEnumerator GetNewItem(string json)
        {
            List<StoreSingleListVo> premiumList = GetSingleVo(json).lists;

            for(int i = 0; i < premiumList.Count; i++)
            {
                if (!premiumList[i].isBill)
                {
                    newItemCount++;

                    WWW www = new WWW(CommonUrl.BASE_URL + premiumList[i].imgThum.Substring(2));

                    //Debug.Log("www.url : " + www.url);

                    yield return www;

                    if (www.error == null)
                    {
                        Rect premLockRect = new Rect(0, 0, www.texture.width, www.texture.height);
                        Sprite sprite = Sprite.Create(www.texture, premLockRect, Vector2.zero);
                        sprite.name = premiumList[i].name;


                        if (newItemsImages[0].sprite == null)
                        {
                            newItemsObjs[0].SetActive(true);
                            newItemsImages[0].sprite = sprite;
                        }
                        else if (newItemsImages[1].sprite == null)
                        {
                            newItemsObjs[1].SetActive(true);
                            newItemsImages[1].sprite = sprite;
                        }
                        else if (newItemsImages[2].sprite == null)
                        {
                            newItemsObjs[2].SetActive(true);
                            newItemsImages[2].sprite = sprite;
                        }
                    }
                    else
                    {
                        Debug.Log("[GetNewItem] : " + www.error);
                    }
                }
            }
            

            
              //Temporary new item count is 0 for no list of newlabeobj
             

            newItemCount = 0;
            if (newItemCount == 0)
            {
                newLabelObj.SetActive(false);
                grid.padding.top = 30;
                scrollCheckValue = 0.9f;
            }
            else
            {
                newLabelObj.SetActive(true);
                grid.padding.top = 550;
                scrollCheckValue = 0.7f;
            }
        }
        
        /// <summary>
        /// 캐릭터 풀어주기 위한 체크
        /// </summary>
        /// 
        /*
        List<bool> CharactersCheck()
        {
            List<bool> checkList = new List<bool>();

            for(int i = 0; i < Variables.characterNames.Count; i++)
            {
                /// null이거나 공백이면
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(Variables.characterNames[i])))
                {
                    Debug.Log("CharactersCheck :: false");
                    //checkList.Add(false);
                    checkList.Add(true);
                }
                else
                {
                    Debug.Log("CharactersCheck :: true");
                    checkList.Add(true);
                }
            }
            return checkList;
        }

        /// <summary>
        /// 캐릭터 풀어주기 위한 체크
        /// </summary>
        bool CharactersCheck2(int i)
        {
            //List<bool> checkList = new List<bool>();

            //for (int i = 0; i < Variables.characterNames.Count; i++)
            //{
                /// null이거나 공백이면
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(Variables.characterNames[i])))
                {
                    Debug.Log("CharactersCheck :: false");
                    //checkList.Add(false);
                    //checkList.Add(true);
                    return false;
                }
                else
                {
                    Debug.Log("CharactersCheck :: true");
                    //checkList.Add(true);
                    return true;
                }
            //}
            //return checkList;
        }
        */
        /// <summary>
        /// 선택된 이미지 변경
        /// </summary>
        /// <param name="image"></param>
        public void SeletedChangeSprite(Image image)
        {
            seletedImage.sprite = image.sprite;

            if (string.IsNullOrEmpty(image.sprite.name))
            {
                Variables.name = image.name;
            }
            else
            {
                Variables.name = image.sprite.name;
            }

            button.enabled = true;
            button.image.sprite = normalImage;
        }

        /// <summary>
        /// 라인 위치
        /// </summary>

        /*
        public void SetLinePosition()
        {
            Vector3 pos = newLabelObj.transform.position;
            // (pos.y + -28f : 처음 위치) - (pos.y - filmRect.position.y : 뺀 값)
            pos.y = (pos.y + -28f) - (pos.y - filmArea.transform.position.y);
            newLabelObj.transform.position = pos;
        }
        */
        /// <summary>
        /// 맨 아래단으로 갔을 시 페이징 처리
        /// </summary>
        /// <param name="scroll"></param>
        public void SetSinglePageing(Scrollbar scroll)
        {
            if (scroll.size != 1 && scroll.value <= scrollCheckValue)
            {
                if (!string.IsNullOrEmpty(singleTotalCnt))
                {
                    if (int.Parse(singleTotalCnt) > (int.Parse(pageingCount) + 1) * int.Parse(pageViewCount) && pageState == PageState.Idle)
                    {
                        pageState = PageState.Pageing;
                        pageingCount = (int.Parse(pageingCount) + 1) + "";
                        //TODO
                        //storeApi.PostStoreBuySingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);
                    }
                }
            }
        }

    }
}
