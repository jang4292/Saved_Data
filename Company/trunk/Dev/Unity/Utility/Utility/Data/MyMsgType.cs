using UnityEngine.Networking;

namespace CommonData
{
    public class MyMsgType : MsgType
    {
        public const short CheckingKeyValue = Highest + 1;
        public const short OnMonitoring = Highest + 2;
    }
}