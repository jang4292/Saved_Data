using UnityEngine.Networking;
namespace CommonData.Data
{
    public class MyMessage : MessageBase
    {
        public int number=-1;
        public bool isNeededResponse = false;
        public int deviceNumber=-1;
        public int responeTime = 1000;
        public string delayTimeForStart = "1000";
        public string title="";


    }
}