using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using AndroidPlugins;
using Public;

namespace DrawingAlbum
{
    public class Album : MonoBehaviour
    {
        /// <summary>
        /// 앨범 빈 조각
        /// </summary>
        public GameObject albumPrefab;

        /// <summary>
        /// 앨범 조각을 담아줄 그리드 뷰
        /// </summary>
        public Transform albumGridView;

        /// <summary>
        /// 저장된 그림이 없을 시 보여줄 텍스트
        /// </summary>
        public GameObject noneImageText;

        /// <summary>
        /// 선택된 이미지 오브젝트 
        /// </summary>
        public GameObject seletedObj;

        /// <summary>
        /// 이미지 폭
        /// </summary>
        private const int IMAGE_WIDTH = 278;
        /// <summary>
        /// 이미지 높이
        /// </summary>
        private const int IMAGE_HEIGHT = 221;

        /// <summary>
        /// 이미지 삭제 리스트
        /// </summary>
        public List<GameObject> imageDeletedList;

        /// <summary>
        /// 선택한 그림 파일 경로
        /// </summary>
        public string seletedFilePath = "";

        /// <summary>
        /// 안드로이드 플러그인 매니저
        /// </summary>
        public AndroidPluginManager androidManager;

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Overriding Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        void Start()
        {
            StartCoroutine(ImageFileRead());
            seletedObj.SetActive(false);
        }

        void OnDestroy()
        {
            if(imageDeletedList != null) imageDeletedList.Clear();
        }

        //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ Add Method ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ//

        /// <summary>
        /// Main으로 씬이동
        /// </summary>
        public void SetMoveMain()
        {
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// 오브젝트 활성화
        /// </summary>
        /// <param name="obj"></param>
        public void SetObjectEnable(GameObject obj)
        {
            obj.SetActive(true);
        }

        public void SetObjectDesable(GameObject obj)
        {
            obj.SetActive(false);
        }

        /// <summary>
        /// 앨범 추가
        /// </summary>
        public void AddAlbumPart(string name, Texture2D tex)
        {
            GameObject part = Instantiate(albumPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            part.name = name;
            RectTransform size = part.GetComponent<RectTransform>();
            RawImage image = part.GetComponent<RawImage>();

            part.transform.SetParent(albumGridView.transform);
            image.texture = tex;
            size.localScale = Vector3.one;
            size.sizeDelta = new Vector2(IMAGE_WIDTH, IMAGE_HEIGHT);
        }

        /// <summary>
        /// Image File Read
        /// </summary>
        /// <returns></returns>
        public IEnumerator ImageFileRead()
        {
            yield return new WaitForEndOfFrame();

            string dirPath = SetDirPath();

            /// 지정한 경로에 폴더가 존재 하는지 확인합니다.
            if (Directory.Exists(dirPath))
            {
                /// DirectoryInfo Class를 생성합니다.
                DirectoryInfo di = new DirectoryInfo(dirPath);

                FileInfo[] files = di.GetFiles("*.png").OrderBy(f => f.CreationTime).ToArray();

                if (files.Length != 0)
                {
                    /// 폴더 내부에 있는 파일 목록 불러오기
                    for (int i = 0; i < files.Length; i++)
                    {
                        string filePath = dirPath + "/" + files[i].Name;

                        if (File.Exists(filePath))
                        {
                            byte[] data = File.ReadAllBytes(filePath);
                            Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
                            texture.LoadImage(data);
                            AddAlbumPart(files[i].Name, texture);
                        }
                    }
                }
                /// 저장된 그림이 없으면 noneImageText를 보여준다.
                else
                {
                    noneImageText.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <returns></returns>
        public IEnumerator ImageFileDeleted()
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < imageDeletedList.Count; i++)
            {
                if (File.Exists(SetDirPath() + "/" + imageDeletedList[i].name))
                {
                    try
                    {
                        File.Delete(SetDirPath() + "/" + imageDeletedList[i].name);
                        androidManager.GetGalleryFileRefresh(SetDirPath() + "/" + imageDeletedList[i].name);
                        imageDeletedList[i].SetActive(false);
                    }
                    catch (IOException e)
                    {
                        Debug.Log(e.Message);
                    }
                }
            }
            imageDeletedList.Clear();
        }

        public void FileRemove()
        {
            if (imageDeletedList != null && imageDeletedList.Count != 0) StartCoroutine(ImageFileDeleted());
        }

        /// <summary>
        /// 경로
        /// </summary>
        /// <returns></returns>
        string SetDirPath()
        {
#if UNITY_ANDROID
            return Variables.moveFolderPath;
#endif
        }
    }
}
