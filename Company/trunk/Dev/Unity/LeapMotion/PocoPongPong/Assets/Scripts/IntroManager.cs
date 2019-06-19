using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroManager : MonoBehaviour {

    // 인트로 버튼
    // 시작, 옵션, 치료결과, 종료
    [SerializeField]
    private SpriteRenderer[] btn = null;

    // 버튼 클릭 모음
    [SerializeField]
    private Sprite[] btnSeleted = null;

    // 버튼 비클릭 모음
    [SerializeField]
    private Sprite[] btnDeseleted = null;

    // 구름 모음
    [SerializeField]
    private GameObject[] clouds = null;

    // 결과창
    public GameObject resultBackground = null;

    private Vector3 cloudPos;

    private List<float> random = new List<float>();

    // Use this for initialization
    void Awake() {

        // 프레임 60으로 고정
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //스크린 비율 16:9
        float _height = Screen.width / 16f * 9;

        Screen.SetResolution(Screen.width, (_height % 1f > 0) ? Mathf.RoundToInt(_height) : (int)_height, true);
        CloudCreate();

        resultBackground.SetActive(false);
    }

    void Update()
    {
        CloudMove();
    }

    // 구름 스타트 지점
    private void CloudCreate()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            random.Add(Random.Range(0, 0.05f));
        }
    }

    // 구름 이동
    private void CloudMove()
    {
        for(int i = 0; i < clouds.Length; i++)
        {
            cloudPos = clouds[i].transform.position;
            cloudPos.x += random[i];

            if (cloudPos.x > 8.2f)
            {
                cloudPos.x = -13.58f;
            }

            clouds[i].transform.position = cloudPos;
        }
    }

    // 인트로 버튼 클릭
    public void btnIntroSeleted(int number)
    {
        btn[number].sprite = btnSeleted[number];
    }

    // 인트로 버튼 클릭 해제
    public void btnIntroDeseleted(int number)
    {
        btn[number].sprite = btnDeseleted[number];
    }

    public GameObject btnObj = null;
    public GameObject userObj = null;

    // 인트로 버튼 이벤트
    public void btnMapChange(int number, HandMouseSource.HandMouseStatus status)
    {
        // 체크 해제
        if (status == HandMouseSource.HandMouseStatus.CLICK && btn[number].sprite == btnDeseleted[number])
        {
            btnIntroSeleted(number);
        }

        // 이벤트
        if (status == HandMouseSource.HandMouseStatus.IDLE && btn[number].sprite == btnSeleted[number])
        {
            switch (number)
            {
                // 시작, 옵션, 치료결과, 종료
                case 0:
                    AutoFade.LoadLevel("Map", 1, 1, Color.white);
                    break;
                case 1:
                    btnObj.SetActive(false);
                    userObj.SetActive(true);
                    break;
                case 2:
                    btnObj.SetActive(false);
                    resultBackground.SetActive(true);
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
            SoundManager.clickSound();
            btnIntroDeseleted(number);
        }
    }
}
