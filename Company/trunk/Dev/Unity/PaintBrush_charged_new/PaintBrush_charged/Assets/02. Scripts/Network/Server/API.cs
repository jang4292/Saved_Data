
using Network;

namespace Api
{
    public class API
    {
        public WWWHelper helper;

        public WWWHelper.HttpRequestDelegate mRequest;

        public void MinusRequest()
        {
            if (helper != null) helper.OnHttpRequest -= mRequest;
        }
    }
}
