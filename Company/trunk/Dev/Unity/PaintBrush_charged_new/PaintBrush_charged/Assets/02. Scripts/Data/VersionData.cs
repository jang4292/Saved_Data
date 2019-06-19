
namespace Data
{
    public class VersionData : Result
    {
        /// <summary>
        /// 버전 정보
        /// </summary>
        public string version;
        /// <summary>
        /// 패키지 이름
        /// </summary>
        public string packageName;

        public VersionData() : base()
        {
            this.version = "";
            this.packageName = "";
        }
    }
}
