using System.Collections.Generic;

namespace Vo
{
    public class ThemaVo : Result
    {
        /// <summary>
        /// 싱글리스트
        /// </summary>
        public List<ThemaItemVo> lists;

        /// <summary>
        /// 총 저장된 이미지 갯수
        /// </summary>
        public int totalCnt;

        public ThemaVo() : base()
        {
            lists = new List<ThemaItemVo>();
            this.totalCnt = -1;
        }
    }
}

