using System.Collections.Generic;

namespace CommonData.Data
{
    [System.Serializable]
    public class LoginInfoList : Result
    {
        public int totalCnt;
        public List<LoginInfo> lists;
        public string mileage;
        public string name;

        public LoginInfoList() : base()
        {
            this.lists = new List<LoginInfo>();
            this.mileage = "";
            this.totalCnt = -1;
            this.name = "";
        }

        [System.Serializable]
        public class LoginInfo
        {
            public string machineUID;
            public string ownerUID;

            public LoginInfo() : base()
            {
                machineUID = "";
                ownerUID = "";
            }
        }
    }
}
