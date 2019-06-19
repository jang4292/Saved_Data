
namespace Vo
{
    [System.Serializable]
    public class StoreSingleListVo : Result
    {
        /// <summary>
        /// 아이디값
        /// </summary>
        public int idx;

        /// <summary>
        /// 이름
        /// </summary>
        public string name;

        /// <summary>
        /// 섬네일 url
        /// </summary>
        public string imgThum;

        /// <summary>
        /// 에셋 url
        /// </summary>
        public string imgRes;

        /// <summary>
        /// 스토어 url
        /// </summary>
        public string imgStore;

        /// <summary>
        /// 종류 ex) animal or person
        /// </summary>
        public string kind;

        /// <summary>
        /// 가격 ( 보석 갯수 )
        /// </summary>
        public string downAmount;

        /// <summary>
        /// 다운받은 횟수
        /// </summary>
        public int downCnt;

        /// <summary>
        /// 생성 날짜
        /// </summary>
        public string regDate;

        /// <summary>
        /// 구매 했는지
        /// </summary>
        public bool isBill;

        public StoreSingleListVo() : base()
        {
            this.idx = -1;
            this.name = "";
            this.imgThum = "";
            this.imgRes = "";
            this.imgStore = "";
            this.kind = "";
            this.downAmount = "";
            this.downCnt = -1;
            this.regDate = "";
            this.isBill = false;
        }
    }
}
