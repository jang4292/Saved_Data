namespace Network
{
    public class Url : CommonURL
    {
        private static Url instance = null;
        public static Url Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Url();
                }
                return instance;
            }
        }
        private string baseURL = null;
        private string serverAppName = @"/paintbrush/";

        protected override string BaseURL
        {
            get
            {
                if (baseURL == null)
                {
                    baseURL = ServerAddress + serverAppName;
                }
                return baseURL;
            }
        }


    }
}
