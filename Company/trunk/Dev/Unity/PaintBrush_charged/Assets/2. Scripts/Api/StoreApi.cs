using System.Collections.Generic;
using Public;

namespace Api
{
    public class StoreApi : API
    {
        public StoreApi(WWWHelper.HttpRequestDelegate request)
        {
            if (helper == null) helper = WWWHelper.Instance;

            mRequest = request;
        }

        public void PostSingleProductSearch(string url, int id, string searchText)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "lists");
            data.Add("search", searchText);
            data.Add("page", "0");
            data.Add("count", "1");

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostUserNewItemList(string url, int id, string count)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "user_not_buy");
            data.Add("uidx", Variables.userIdx + "");
            data.Add("count", count);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }


        public void PostStoreBuyToSingleList(string url, int id, string page, string count)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "user_lists");
            data.Add("uidx", Variables.userIdx+"");
            data.Add("search", "");
            data.Add("page", page);
            data.Add("count", count);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostStoreBuySingleList(string url, int id, string page, string count)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "user_buy_lists");
            data.Add("uidx", Variables.userIdx + "");
            data.Add("search", "");
            data.Add("page", page);
            data.Add("count", count);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostCheckedThemaNumber(string url, int id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "lists");
            //data.Add("mth", "list");

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        public void PostItemsListInThema(string url, int id, int tidx, string page, string count)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "items");
            //UnityEngine.Debug.Log("tidx.ToString() :" + tidx.ToString());
            data.Add("tidx", tidx.ToString());
            //data.Add("tidx", "1");
            data.Add("search", "");
            data.Add("page", page);
            data.Add("count", count);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        /// <summary>
        /// 사용자 포인트 사용
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="seletedIdx">1,3,5,6 으로 선택 idx 값을 콤마(,) 구분하여 String 값으로 전달</param>
        /// <param name="type">N : 단일, P : 패키지</param>
        public void PostUserBuyProduct(string url, int id, string seletedIdx, string type)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "buy_product");
            data.Add("idx", Variables.userIdx + "");
            data.Add("pidx", seletedIdx);
            data.Add("ptype", type);

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }

        /// <summary>
        /// 인앱결제
        /// </summary>
        /// <param name="url"></param>
        /// <param name="id"></param>
        /// <param name="money">금액</param>
        public void PostUserInAppBuyCash(string url, int id, string money)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("mth", "buy_point");
            data.Add("idx", Variables.userIdx + "");
            data.Add("buy", money);
            data.Add("point", Variables.userPoint + "");

            helper.OnHttpRequest += mRequest;
            helper.Post(id, url, data);
        }
    }
}
