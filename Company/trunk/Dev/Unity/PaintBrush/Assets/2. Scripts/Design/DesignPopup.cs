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
        public enum PageState
        {
            Idle,
            Pageing
        }

        public PageState pageState;

        /// <summary>
        /// 캐릭터 잠금 이미지
        /// </summary>
        public GameObject[] designObjs;

        /// <summary>
        /// 캐릭터 이미지들
        /// </summary>
        public Image[] characterObjs;

        /// <summary>
        /// 캐릭터 스프라이트
        /// </summary>
        public Sprite[] characterSpirtes;

        /// <summary>
        /// 선택된 이미지
        /// </summary>
        public Image seletedImage;

        /// <summary>
        /// 잠금 이미지
        /// </summary>
        public Sprite lockImage;

        public Sprite premiumLockImage;

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
        private string pageViewCount = "50";

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
            PutImageSprite();

            pageLoadingPopup.SetActive(true);

            storeApi = new StoreApi(OnHttpRequest);
            storeApi.PostUserNewItemList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_DESIGN_NEW_LIST, "3");
        }

        void LateUpdate()
        {
            if (gameObject.activeSelf) SetLinePosition();
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        /// <summary>
        /// 이미지 추가
        /// </summary>
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

                    if (CharactersCheck()[i])
                    {
                        designButton.enabled = false;
                        designImage.enabled = false;
                    }
                    else
                    {
                        designButton.interactable = false;
                        designImage.sprite = lockImage;
                    }

                    designSize.sizeDelta = new Vector2(IMAGE_WIDTH, IMAGE_HEIGHT);
                }
            }
        }

        private SingleVo GetSingleVo(string jsonData)
        {
            return JsonUtility.FromJson<SingleVo>(jsonData);
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
            if (www.error == null)
            {
                switch (id)
                {
                    case API.API_PRODUCT_DESIGN_NEW_LIST:

                        if (IsResult(www.text))
                        {
                            StartCoroutine(GetNewItem(www.text));
                        }

                        storeApi.PostStoreBuySingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);
                        break;
                    case API.API_PRODUCT_DESING_BUY_LIST:
                        if (IsResult(www.text))
                        {
                            scrollRect.inertia = false;

                            StartCoroutine(PutPremiumPart(www.text));
                            singleTotalCnt = GetTotalCount(www.text);
                        }
                        else
                        {
                            pageLoadingPopup.SetActive(false);
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

        /// <summary>
        /// 프리미엄 상품추가
        /// </summary>
        IEnumerator PutPremiumPart(string jsonData)
        {
            premiumList = GetSingleVo(jsonData).lists;

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

            if(newItemCount == 0)
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
        List<bool> CharactersCheck()
        {
            List<bool> checkList = new List<bool>();

            for(int i = 0; i < Variables.characterNames.Count; i++)
            {
                /// null이거나 공백이면
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(Variables.characterNames[i])))
                {
                    checkList.Add(false);
                }
                else
                {
                    checkList.Add(true);
                }
            }
            return checkList;
        }

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
        }

        /// <summary>
        /// 라인 위치
        /// </summary>
        public void SetLinePosition()
        {
            Vector3 pos = newLabelObj.transform.position;
            // (pos.y + -28f : 처음 위치) - (pos.y - filmRect.position.y : 뺀 값)
            pos.y = (pos.y + -28f) - (pos.y - filmArea.transform.position.y);
            newLabelObj.transform.position = pos;
        }

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
                        storeApi.PostStoreBuySingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_DESING_BUY_LIST, pageingCount, pageViewCount);
                    }
                }
            }
        }

    }
}
