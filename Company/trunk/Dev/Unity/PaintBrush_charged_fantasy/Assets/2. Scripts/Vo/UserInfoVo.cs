
namespace Vo
{
    public class UserInfoVo : Result
    {
        /// <summary>
        /// 사용자 idx
        /// </summary>
        public int idx;

        /// <summary>
        /// 사용자 uuid
        /// </summary>
        public string uuid;

        /// <summary>
        /// 보낸 uuid
        /// </summary>
        public string requuid;

        /// <summary>
        /// 사용자 포인트
        /// </summary>
        public int point;

        /// <summary>
        /// 사용자 구매 후 적립포인트
        /// </summary>
        public string buyPoint;

        /// <summary>
        /// GCM ID
        /// </summary>
        public string gid;

        /// <summary>
        /// 사용자 계정 생성 날자
        /// </summary>
        public string regDate;

        public UserInfoVo() : base()
        {
            this.idx = -1;
            this.uuid = "";
            this.requuid = "";
            this.point = -1;
            this.buyPoint = "";
            this.gid = "";
            this.regDate = "";
        }
    }
}
