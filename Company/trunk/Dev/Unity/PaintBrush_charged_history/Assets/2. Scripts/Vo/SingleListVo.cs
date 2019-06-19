namespace Vo
{
    [System.Serializable]
    public class ThemaItemVo : Result
    {

        public int tidx;
        public string name;

        public ThemaItemVo() : base()
        {
            this.tidx = -1;
            this.name = "";
            this.result = false;
            this.resultMsg = "";
        }
    }
}

