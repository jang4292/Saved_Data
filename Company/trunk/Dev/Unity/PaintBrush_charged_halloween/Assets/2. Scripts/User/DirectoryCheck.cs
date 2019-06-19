using UnityEngine;
using System.IO;
using Public;

namespace User
{
    public class DirectoryCheck : MonoBehaviour
    {
        private DirectoryInfo directoryInfo;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            if(GetFiles() != null && GetFiles().Length != 0)
            {
                SetFileMove();
            }

            if (!GetDirectoryInfo(Variables.assetBundlePath).Exists)
            {
                GetDirectoryInfo(Variables.assetBundlePath).Create();
            }
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        DirectoryInfo GetDirectoryInfo(string path)
        {
            return directoryInfo = new DirectoryInfo(path);
        }

        FileInfo[] GetFiles()
        {
            try
            {
                return GetDirectoryInfo(Application.persistentDataPath).GetFiles("*.png");
            }
            catch
            {
                return null;
            }
        }

        void SetFileMove()
        {
            if (!GetDirectoryInfo(Variables.moveFolderPath).Exists)
            {
                GetDirectoryInfo(Variables.moveFolderPath).Create();
            }

            foreach (FileInfo info in GetFiles())
            {
                File.Move(info.FullName, Variables.moveFolderPath + info.Name);
            }
        }
    }
}
