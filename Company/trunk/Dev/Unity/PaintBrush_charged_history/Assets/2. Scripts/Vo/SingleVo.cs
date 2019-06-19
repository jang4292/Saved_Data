using System.Collections.Generic;

namespace Vo
{
    public class SingleVo : Result
    {
        /// <summary>
        /// 싱글리스트
        /// </summary>
        public List<StoreSingleListVo> lists;

        /// <summary>
        /// 총 저장된 이미지 갯수
        /// </summary>
        public string totalCnt;

        public SingleVo() : base()
        {
            lists = new List<StoreSingleListVo>();
            this.totalCnt = "";
        }
    }
}

