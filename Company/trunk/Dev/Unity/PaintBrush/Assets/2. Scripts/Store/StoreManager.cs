using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AndroidPlugins;
using Api;
using Vo;
using GA;
using Public;

namespace Store
{
    public class StoreManager : MonoBehaviour
    {
        public enum StoreLabelState
        {
            Single,
            Package
        }

        private WWWHelper helper;

        private StoreApi storeApi;

        private List<StoreSingleListVo> premiumList;

        public StoreLabelState labelState;

        public Image singleImage;
        public Image packageImage;

        public Sprite singleSeletedSprite;
        public Sprite singleDeseletedSprite;
        public Sprite packageSeletedSprite;
        public Sprite packageDeseletedSprite;

        public GameObject singleScrollView;
        public GameObject singleScrollBar;
        public GameObject packageScrollView;
        public GameObject packageScrollBar;

        public GameObject singlePartPrefab;

        public Transform singlePartParent;

        public GameObject buyButton;

        public List<string> userBuyidxList;

        public Text userMoney;

        public GameObject singleBuyPartPrefab;

        public Transform singleBuyPartParent;

        public Text gemCount;

        private Dictionary<string, GameObject> popUpObjects = new Dictionary<string, GameObject>();

        private Dictionary<int, GameObject> storeSingleObjects = new Dictionary<int, GameObject>();

        private List<StoreSingleListVo> seletedPopupList = new List<StoreSingleListVo>();

        public AndroidPluginManager androidManager;

        /// <summary>
        /// 페이지 숫자
        /// </summary>
        private string pageingCount = "0";

        /// <summary>
        /// 한 페이지당 보여줄 갯수
        /// </summary>
        private string pageViewCount = "50";

        /// <summary>
        /// 총 등록된 단일 그림 갯수
        /// </summary>
        private string singleTotalCnt;

        public Button[] storeButtons;

        public GAController gAController;

        public GameObject inAppFailedPopup;

        public GameObject loadingPopup;

        private long inAppMoney;

        public GameObject networkPopup;

        public Text selectedCountText;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                networkPopup.SetActive(true);
                loadingPopup.SetActive(false);
            }

            userMoney.text = Variables.userPoint + "";
            SetScrollAndState(StoreLabelState.Single);
            storeApi = new StoreApi(OnHttpRequest);
            storeApi.PostStoreBuyToSingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_SINGLE_STORE_LIST, pageingCount, pageViewCount);
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        public void SetLabelSeleted()
        {
            if(IsLabelSeleted(singleImage, singleDeseletedSprite))
            {
                SetScrollAndState(StoreLabelState.Single);
                singleImage.sprite = singleSeletedSprite;
                packageImage.sprite = packageDeseletedSprite;
            }
            else if(IsLabelSeleted(singleImage, singleSeletedSprite))
            {
                SetScrollAndState(StoreLabelState.Package);
                singleImage.sprite = singleDeseletedSprite;
                packageImage.sprite = packageSeletedSprite;
            }
        }

        private void SetScrollAndState(StoreLabelState state)
        {
            if(state == StoreLabelState.Single)
            {
                labelState = StoreLabelState.Single;
                buyButton.SetActive(true);
                SetSingleScrollActive(true);
                SetPackageScrollActive(false);
            }
            else
            {
                labelState = StoreLabelState.Package;
                buyButton.SetActive(false);
                SetSingleScrollActive(false);
                SetPackageScrollActive(true);
            }
        }

        private void SetSingleScrollActive(bool change)
        {
            singleScrollView.SetActive(change);
            singleScrollBar.SetActive(change);
        }

        private void SetPackageScrollActive(bool change)
        {
            packageScrollView.SetActive(change);
            packageScrollBar.SetActive(change);
        }

        private bool IsLabelSeleted(Image labelImage, Sprite labelSprite)
        {
            if(labelImage.sprite == labelSprite)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveHome()
        {
            SceneManager.LoadScene("Main");
        }

        public void AppQuit()
        {
            Application.Quit();
        }

        public void SetEnableObject(GameObject obj)
        {
            obj.SetActive(true);
        }

        public void SetDisableObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        public void SetEnableStorePopup(GameObject obj)
        {
            if (userBuyidxList == null || userBuyidxList.Count == 0) return;

            StartCoroutine(GetPopupSinglePart(obj));
        }

        /// <summary>
        /// 단일 or 여러개 구매
        /// </summary>
        public void SetSingleUserBuyPoint()
        {
            storeApi.PostUserBuyProduct(CommonUrl.USER_URL, API.API_USER_SINGLE_BUY_PRODUCT, GetBuyList(), "N");
        }

        public string GetBuyList()
        {
            StringBuilder listName = new StringBuilder();

            for (int i = 0; i < userBuyidxList.Count; i++)
            {
                listName.Append(userBuyidxList[i]);
            }

            return listName.ToString().Substring(0, listName.Length - 1);
        }

        private bool IsResult(string json)
        {
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
                    case API.API_USER_IN_APP_CASH:
                        UserInfoVo vo = JsonUtility.FromJson<UserInfoVo>(www.text);

                        if (vo.result)
                        {
                            Variables.userPoint = vo.point;
                            userMoney.text = Variables.userPoint + "";
                            androidManager.GetToast("결제가 완료되었습니다.");
                        }
                        else
                        {
                            inAppFailedPopup.SetActive(true);
                            gAController.PostInAppServerNonConnect("결제 후 서버와의 동기화 실패", inAppMoney);
                        }

                        StartCoroutine(InAppPurchaseDelay(true));
                        break;
                    case API.API_PRODUCT_SINGLE_STORE_LIST:
                        if (IsResult(www.text))
                        {
                            loadingPopup.SetActive(true);

                            StartCoroutine(PutSinglePremiumPart(www.text));
                            singleTotalCnt = GetTotalCount(www.text);
                        }
                        break;
                    case API.API_PRODUCT_SINGLE_STORE_LIST_PAGEING:
                        if (IsResult(www.text))
                        {
                            StartCoroutine(PutSinglePremiumPart(www.text));
                        }
                        break;
                    case API.API_USER_SINGLE_BUY_PRODUCT:
                        UserInfoVo uResult = JsonUtility.FromJson<UserInfoVo>(www.text);
                        if (uResult.result)
                        {
                            userBuyidxList.Clear();

                            storeApi.PostStoreBuyToSingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_SINGLE_STORE_REFRESH_LIST, "0", storeSingleObjects.Count + "");

                            Variables.userPoint = uResult.point;
                            userMoney.text = Variables.userPoint + "";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(uResult.resultMsg) && !uResult.resultMsg.Contains("Exception"))
                            {
                                androidManager.GetToast(uResult.resultMsg);
                            }
                            else
                            {
                                androidManager.GetToast("보석이 부족합니다. 충전이 필요합니다.");
                            }
                        }
                        break;
                    case API.API_PRODUCT_SINGLE_STORE_REFRESH_LIST:
                        if (IsResult(www.text))
                        {
                            premiumList = GetSingleVo(www.text).lists;
                            loadingPopup.SetActive(true);
                            StartCoroutine(PutLocalAssetBundle(premiumList));
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("[StoreManager] : " + www.error);
            }

            storeApi.MinusRequest();
        }

        /// <summary>
        /// 구매 후 -> 상품 구매처리 표시
        /// </summary>
        /// <param name="premiumList"></param>
        /// <returns></returns>
        IEnumerator PutLocalAssetBundle(List<StoreSingleListVo> premiumList)
        {
            int premiumListCount = premiumList.Count;
            for (int i = 0; i < premiumListCount; i++)
            {
                if (storeSingleObjects.ContainsKey(premiumList[i].idx) && premiumList[i].isBill)
                {
                    GameObject soldoutObj = storeSingleObjects[premiumList[i].idx].transform.Find("SoldOut").gameObject;

                    if (!soldoutObj.activeSelf)
                    {
                        storeSingleObjects[premiumList[i].idx].SetActive(false);
                        soldoutObj.SetActive(true);
                        storeSingleObjects[premiumList[i].idx].SetActive(true);

                        WWW www = new WWW(CommonUrl.BASE_URL + premiumList[i].imgRes);

                        yield return www;

                        if (string.IsNullOrEmpty(www.error))
                        {
                            File.WriteAllBytes(Variables.assetBundlePath + premiumList[i].name + ".unity3d", www.bytes);
                        }
                        else
                        {
                            Debug.Log(www.error);
                        }

                        www.Dispose();
                    }
                }
            }

            loadingPopup.SetActive(false);
        }

        SingleVo GetSingleVo(string jsonData)
        {
            return JsonUtility.FromJson<SingleVo>(jsonData);
        }

        /// <summary>
        /// 프리미엄 단일 상품추가
        /// </summary>
        IEnumerator PutSinglePremiumPart(string jsonData)
        {
            premiumList = GetSingleVo(jsonData).lists;

            for (int i = 0; i < premiumList.Count; i++)
            {
                if (string.IsNullOrEmpty(premiumList[i].imgStore))
                {
                    continue;
                }

                seletedPopupList.Add(premiumList[i]);

                GameObject premiumPart = Instantiate(singlePartPrefab) as GameObject;
                premiumPart.name = premiumList[i].idx + ",";
                storeSingleObjects.Add(premiumList[i].idx, premiumPart);

                Image premCharacterImage = premiumPart.transform.Find("CharacterImage").GetComponent<Image>();
                Text premGemCount = premiumPart.transform.Find("GemCount").GetComponent<Text>();
                GameObject premLockObj = premiumPart.transform.Find("SoldOut").gameObject;

                premGemCount.text = premiumList[i].downAmount + "개";

                premiumPart.transform.SetParent(singlePartParent);
                premiumPart.transform.localScale = singlePartParent.localScale;

                // 구매
                if (premiumList[i].isBill)
                {
                    premLockObj.SetActive(true);
                }
                else
                {
                    // 비구매
                    premLockObj.SetActive(false);
                }

                WWW premWWW = new WWW(CommonUrl.BASE_URL + premiumList[i].imgStore.Substring(2));

                yield return premWWW;

                if (premWWW.error == null)
                {
                    Rect premLockRect = new Rect(0, 0, premWWW.texture.width, premWWW.texture.height);
                    Sprite sprite = Sprite.Create(premWWW.texture, premLockRect, Vector2.zero);
                    sprite.name = premiumList[i].name;
                    premCharacterImage.sprite = sprite;
                }
                else
                {
                    Debug.Log("[PutSinglePremiumPart] : " + premWWW.error);
                }
            }

            loadingPopup.SetActive(false);
        }

        /// <summary>
        /// 구매 하기전 팝업
        /// </summary>
        /// <returns></returns>
        IEnumerator GetPopupSinglePart(GameObject popupObj)
        {
            for (int i = 0; i < seletedPopupList.Count; i++)
            {
                for (int a = 0; a < userBuyidxList.Count; a++)
                {
                    if (int.Parse(userBuyidxList[a].Replace(",", "")) == seletedPopupList[i].idx)
                    {
                        if (popUpObjects.ContainsKey(seletedPopupList[i].idx + ""))
                        {
                            popUpObjects[seletedPopupList[i].idx + ""].SetActive(true);
                        }
                        else
                        {
                            GameObject part = Instantiate(singleBuyPartPrefab) as GameObject;
                            part.name = seletedPopupList[i].idx + "";
                            popUpObjects.Add(part.name, part);
                            GameObject characterObj = part.transform.Find("CharacterImage").gameObject;
                            Image characterImage = characterObj.GetComponent<Image>();

                            WWW www = new WWW(CommonUrl.BASE_URL + seletedPopupList[i].imgThum.Substring(2));

                            yield return www;

                            if (www.error == null)
                            {
                                Rect premLockRect = new Rect(0, 0, www.texture.width, www.texture.height);
                                Sprite sprite = Sprite.Create(www.texture, premLockRect, Vector2.zero);
                                characterImage.sprite = sprite;
                                characterObj.SetActive(true);

                                part.transform.SetParent(singleBuyPartParent);
                                part.transform.localScale = singleBuyPartParent.localScale;
                            }
                            else
                            {
                                Debug.Log("[GetPopupSinglePart] : " + www.error);
                            }
                        }
                    }
                }
            }

            gemCount.text = (userBuyidxList.Count * 2) + "";

            popupObj.SetActive(true);
        }

        /// <summary>
        /// 구매하기전 팝업을 닫을 시 있던 오브젝트 비활성화
        /// </summary>
        public void PopUpObjectesDisable()
        {
            foreach (GameObject obj in popUpObjects.Values)
            {
                obj.SetActive(false);
            }
        }

        /// <summary>
        /// 맨 아래단으로 갔을 시 페이징 처리
        /// </summary>
        /// <param name="scroll"></param>
        public void SetSinglePageing(Scrollbar scroll)
        {
            if(scroll.size != 1 && scroll.value <= 0)
            {
                if (int.Parse(singleTotalCnt) > (int.Parse(pageingCount) + 1) * int.Parse(pageViewCount))
                {
                    pageingCount = (int.Parse(pageingCount) + 1) + "";
                    storeApi.PostStoreBuyToSingleList(CommonUrl.PRODUCT_URL, API.API_PRODUCT_SINGLE_STORE_LIST, pageingCount, pageViewCount);
                }
            }
        }

        /// <summary>
        /// 보석 구매
        /// </summary>
        /// <param name="money"></param>
        public void BuyUserMoney(string money)
        {
            inAppMoney = long.Parse(money);
            storeApi.PostUserInAppBuyCash(CommonUrl.USER_URL, API.API_USER_IN_APP_CASH, money);
        }


        /// <summary>
        /// 결제 취소시 호출
        /// </summary>
        /// <param name="message"></param>
        public void InAppPurchaseCancel(string message)
        {
            if (message.Equals("cancel")) SetStoreButtonsShow(true);
        }

        /// <summary>
        /// 스토어의 모든 버튼 활성화 관리
        /// </summary>
        /// <param name="show"></param>
        public void SetStoreButtonsShow(bool show)
        {
            foreach(Button btn in storeButtons)
            {
                btn.enabled = show;
            }
        }

        /// <summary>
        /// InApp 결제 후 중복구매가 안되게 딜레이를 줌
        /// </summary>
        /// <param name="show"></param>
        /// <returns></returns>
        IEnumerator InAppPurchaseDelay(bool show)
        {
            yield return new WaitForSeconds(2f);

            SetStoreButtonsShow(show);
        }

        /// <summary>
        /// 선택된 그림 갯수 
        /// </summary>
        public void SetSelectedCount()
        {
            selectedCountText.text = userBuyidxList.Count + " / 5";
        }
    }
}
