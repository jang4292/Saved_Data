using UnityEngine;
using System.Collections;

public class SettingManager : MonoBehaviour {

    // 유저 정보
    // 0 = 이름, 1 = 나이, 2 = 환자번호
    public TextMesh[] oneUserListText = null;
    // 유저 정보   순서주기위함
    public MeshRenderer[] oneUserListRen = null;
    public TextMesh[] twoUserListText = null;
    public MeshRenderer[] twoUserListRen = null;
    public TextMesh[] threeUserListText = null;
    public MeshRenderer[] threeUserListRen = null;
    public TextMesh[] fourUserListText = null;
    public MeshRenderer[] fourUserListRen = null;

    // 유저 그림 모음
    // 0 = 비선택, 1 = 선택
    public Sprite[] userSprite = null;

    // 유저 이름 컬러
    private Color colorUserName = new Color();

    // 유저 나이 컬러
    private Color colorUserAge = new Color();

    // 스펠링 모음
    private string[] spellingList = { "전체 보기" , "가", "나", "다", "라", "마", "바", "사", "아", "자", "차", "카", "타", "파", "하"};

    private int spellingNumber = 0;

    private int pageNumber = 1;

    // 0 = 스펠링 텍스트, 1 = 페이지 텍스트
    public TextMesh[] spelling_Page_Text = null;

    // 0 = 스펠링 , 1 = 페이지      순서주기 위함
    public MeshRenderer[] spelling_Page_Ren = null;

    // 0 = 효과음 텍스트, 1 = 배경음악 2 = 컨트롤, 3 = 튜토리얼 텍스트
    public TextMesh[] game_Setting_Text = null;

    // 0 = 효과음 , 1 = 배경음악 , 2 = 컨트롤, 3 = 튜토리얼
    public MeshRenderer[] game_Setting_Ren = null;

    // 0 = 환자선택, 1 = 게임설정, 2 = 인트로 버튼, 3 = 결과 화면
    public GameObject[] settingBackground = null;

    // 왼쪽오른쪽
    public Sprite[] previosNext = null;

    // 클릭 왼쪽오른쪽
    public Sprite[] previosNextPress = null;

    // 0 = 환자선택, 1 = 게임설정, 2 = 저장하기
    public Sprite[] menuBtn = null;

    public Sprite[] menuBtnPress = null;

    // EXIT Sprite 버튼
    // 0 = 노클릭, 1 = 클릭
    public Sprite[] exitBtn = null;

    // 손
    public GameObject hand = null;

    private HandMouseSource handSource = null;

    private SpriteRenderer spriteRenderer = null;

    // 스프라이트 체크
    private bool spriteCheck = false;

    private bool handCheck = false;

    private string objName = "";

    // 데이터
    private DataSave data = null;

    [SerializeField]
    private GameObject soundManager = null;

    private static bool firstStartCheck = false;

    public GameObject logoObj = null;

    public GameObject wall = null;

    public SpriteRenderer[] userSpriteRenderer = null;

    private TextMesh[][] textArray = new TextMesh[4][];

    private int playerNumber;

    public ResultManager result = null;


    /* 
       - 환자선택 - 

       0 = 환자선택, 1 = 게임설정, 2 = 저장하기 
       3 = 스펠링 이전, 4 = 스펠링 다음
       5 = 페이지 이전, 6 = 페이지 다음

       - 게임옵션 - 

       7 = 환자선택, 8 = 게임설정, 9 = 저장하기
       10 = 효과음 이전 , 11 = 효과음 다음
       12 = 배경음악 이전 , 13 = 배경음악 다음
       14 = 모드선택 이전 , 15 = 모드선택 다음
       16 = 튜토리얼 이전, 17 = 튜토리얼 다음
       
       - 게임결과 -
       
       18 = 홈버튼  

    */
    // 옵션창 버튼들 모음 클릭해제 하기 위함
    public SpriteRenderer[] optionRenderer = null;

    void Awake()
    {
        playerNumber = PlayerPrefs.GetInt("user");

        if (firstStartCheck == false)
        {
            data = GameObject.Find("Kinect").GetComponent<DataSave>();
            settingBackground[2].SetActive(false);
            StartCoroutine(LogoSizeChange());
            firstStartCheck = true;
        }
        else
        {
            data = GameObject.Find("four").GetComponent<DataSave>();
        }

    }

    // 로고 몇초뒤 삭제
    IEnumerator LogoSizeChange()
    {
        logoObj.SetActive(true);
        wall.SetActive(true);

        yield return new WaitForSeconds(6.5f);

        Destroy(logoObj);
        Destroy(wall);
        settingBackground[2].SetActive(true);
        
    }

    // Use this for initialization
    void Start () {
        
        ColorUtility.TryParseHtmlString("#694A35", out colorUserName);
        ColorUtility.TryParseHtmlString("#D3641A", out colorUserAge);

        textArray[0] = oneUserListText;
        textArray[1] = twoUserListText;
        textArray[2] = threeUserListText;
        textArray[3] = fourUserListText;

        userSpriteRenderer[playerNumber].sprite = userSprite[1];
        textArray[playerNumber][0].color = Color.white;
        textArray[playerNumber][1].color = Color.white;

        // 효과음, 배경음악 온오프 확인
        CheckPlayerPrefbs(game_Setting_Text, 0, "effect");
        CheckPlayerPrefbs(game_Setting_Text, 1, "bgm");
        CheckPlayerPrefbs(game_Setting_Text, 2, "control");
        CheckPlayerPrefbs(game_Setting_Text, 3, "tutorial");

        if (game_Setting_Text[0].text.Equals("OFF"))
        {
            soundManager.SetActive(false);
        }

        if (game_Setting_Text[1].text.Equals("OFF"))
        {
            KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.OFF;
        }

        TextOrder(game_Setting_Ren);
        TextOrder(spelling_Page_Ren);

        /* 임시데이터 4개 */

        /* ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ */

        TextOrder(oneUserListRen);
        TextOrder(twoUserListRen);
        TextOrder(threeUserListRen);
        TextOrder(fourUserListRen);

        UserLoadData(oneUserListText,0);
        UserLoadData(twoUserListText,1);
        UserLoadData(threeUserListText,2);
        UserLoadData(fourUserListText,3);

        /* ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ */

        handSource = hand.GetComponent<HandMouseSource>();
    }

    // 처음 들어간거 체크
    private bool check = false;

    void Update()
    {
        // 스프라이트에 들어 갔을 경우
        if (Physics2D.OverlapPoint(hand.transform.position) != null)
        {
            spriteCheck = true;

            spriteRenderer = Physics2D.OverlapPoint(hand.transform.position).GetComponent<SpriteRenderer>();

            objName = Physics2D.OverlapPoint(hand.transform.position).name;

            // 클릭시
            if (handSource.status.Equals(HandMouseSource.HandMouseStatus.CLICK))
            {
                handCheck = true;

                if (check == false)
                {
                    switch (objName)
                    {
                        // 첫번째 유저
                        case "one":
                            userSpriteRenderer[0].sprite = userSprite[1];
                            oneUserListText[0].color = Color.white;
                            oneUserListText[1].color = Color.white;
                            break;
                        // 두번째 유저
                        case "two":
                            userSpriteRenderer[1].sprite = userSprite[1];
                            twoUserListText[0].color = Color.white;
                            twoUserListText[1].color = Color.white;
                            break;
                        // 세번째 유저
                        case "three":
                            userSpriteRenderer[2].sprite = userSprite[1];
                            threeUserListText[0].color = Color.white;
                            threeUserListText[1].color = Color.white;
                            break;
                        // 네번째 유저
                        case "four":
                            userSpriteRenderer[3].sprite = userSprite[1];
                            fourUserListText[0].color = Color.white;
                            fourUserListText[1].color = Color.white;
                            break;
                    }
                    check = true;
                }
                else
                {
                    switch (objName)
                    {
                        /* 게임결과 EXIT */

                        case "exit":
                            spriteRenderer.sprite = exitBtn[1];
                            break;

                        /* 환자선택 */

                        // 스펠링 왼쪽
                        case "spelling_previos":
                            spriteRenderer.sprite = previosNextPress[0];
                            break;
                        // 스펠링 오른쪽
                        case "spelling_next":
                            spriteRenderer.sprite = previosNextPress[1];
                            break;
                        // 페이지 왼쪽
                        case "page_previos":
                            spriteRenderer.sprite = previosNextPress[0];
                            break;
                        // 페이지 오른쪽
                        case "page_next":
                            spriteRenderer.sprite = previosNextPress[1];
                            break;

                        /* 게임옵션 */

                        // 이펙트 왼쪽
                        case "effect_previos":
                            spriteRenderer.sprite = previosNextPress[0];
                            break;
                        // 이펙트 오른쪽
                        case "effect_next":
                            spriteRenderer.sprite = previosNextPress[1];
                            break;
                        // 배경음악 왼쪽
                        case "bgm_previos":
                            spriteRenderer.sprite = previosNextPress[0];
                            break;
                        // 배경음악 오른쪽
                        case "bgm_next":
                            spriteRenderer.sprite = previosNextPress[1];
                            break;
                        // 컨트롤 왼쪽
                        case "control_previos":
                            spriteRenderer.sprite = previosNextPress[0];
                            break;
                        // 컨트롤 오른쪽
                        case "control_next":
                            spriteRenderer.sprite = previosNextPress[1];
                            break;
                        // 튜토리얼 왼쪽
                        case "tutorial_previos":
                            spriteRenderer.sprite = previosNextPress[0];
                            break;
                        // 튜토리얼 오른쪽
                        case "tutorial_next":
                            spriteRenderer.sprite = previosNextPress[1];
                            break;
                        // 환자선택
                        case "user_select_setting_btn_n":
                            spriteRenderer.sprite = menuBtnPress[0];
                            break;
                        // 게임설정
                        case "general_game_setting_btn_n":
                            spriteRenderer.sprite = menuBtnPress[1];
                            break;
                        // 저장하기
                        case "save_setting_btn_n":
                            spriteRenderer.sprite = menuBtnPress[2];
                            break;

                    }
                }
            }

            // 적용
            if (handSource.status.Equals(HandMouseSource.HandMouseStatus.IDLE) && handCheck)
            {
                switch (objName)
                {
                    /* 게임결과 EXIT */

                    case "exit":
                        spriteRenderer.sprite = exitBtn[0];
                        settingBackground[2].SetActive(true);
                        settingBackground[3].SetActive(false);
                        result.resultArray[0] = "Start";
                        break;

                    /* 환자선택 */

                    case "spelling_previos":
                        spriteRenderer.sprite = previosNext[0];
                        spellingNumber--;

                        if (spellingNumber == -1)
                        {
                            spellingNumber = spellingList.Length-1;
                        }

                        spelling_Page_Text[0].text = spellingList[spellingNumber];
                        break;
                    case "spelling_next":
                        spriteRenderer.sprite = previosNext[1];
                        spellingNumber++;

                        if (spellingNumber == spellingList.Length)
                        {
                            spellingNumber = 0;
                        }

                        spelling_Page_Text[0].text = spellingList[spellingNumber];
                        break;
                    case "page_previos":
                        spriteRenderer.sprite = previosNext[0];

                        pageNumber--;

                        if(pageNumber < 2)
                        {
                            pageNumber = 1;
                        }

                        spelling_Page_Text[1].text = pageNumber + " Page";
                        break;
                    case "page_next":
                        spriteRenderer.sprite = previosNext[1];

                        pageNumber++;

                        spelling_Page_Text[1].text = pageNumber + " Page";
                        break;
                    case "one":
                        UserBackgroundChange(0);
                        PlayerPrefs.SetInt("user", 0);
                        break;
                    case "two":
                        UserBackgroundChange(1);
                        PlayerPrefs.SetInt("user", 1);
                        break;
                    case "three":
                        UserBackgroundChange(2);
                        PlayerPrefs.SetInt("user", 2);
                        break;
                    case "four":
                        UserBackgroundChange(3);
                        PlayerPrefs.SetInt("user", 3);
                        break;

                    /* 게임옵션 */

                    case "effect_previos":
                        spriteRenderer.sprite = previosNext[0];

                        if (game_Setting_Text[0].text.Equals("OFF"))
                        {
                            break;
                        }

                        OnOFFChange(game_Setting_Text, 0, "OFF", "effect");
                        soundManager.SetActive(false);
                        break;
                    case "effect_next":
                        spriteRenderer.sprite = previosNext[1];

                        if (game_Setting_Text[0].text.Equals("ON"))
                        {
                            break;
                        }

                        OnOFFChange(game_Setting_Text, 0, "ON", "effect");
                        soundManager.SetActive(true);
                        break;
                    case "bgm_previos":
                        spriteRenderer.sprite = previosNext[0];

                        if (game_Setting_Text[1].text.Equals("OFF"))
                        {
                            break;
                        }

                        OnOFFChange(game_Setting_Text, 1, "OFF", "bgm");
                        KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.OFF;
                        break;
                    case "bgm_next":
                        spriteRenderer.sprite = previosNext[1];

                        if (game_Setting_Text[1].text.Equals("ON"))
                        {
                            break;
                        }

                        OnOFFChange(game_Setting_Text, 1, "ON", "bgm");
                        KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.ON;
                        break;
                    case "control_previos":
                        spriteRenderer.sprite = previosNext[0];

                        if (game_Setting_Text[2].text.Equals("왼손"))
                        {
                            break;
                        }

                        OnOFFChange(game_Setting_Text, 2, "왼손", "control");
                        break;
                    case "control_next":
                        spriteRenderer.sprite = previosNext[1];

                        if (game_Setting_Text[2].text.Equals("오른손"))
                        {
                            break;
                        }
                        OnOFFChange(game_Setting_Text, 2, "오른손", "control");
                        break;
                    case "tutorial_previos":
                        spriteRenderer.sprite = previosNext[0];

                        if (game_Setting_Text[3].text.Equals("OFF"))
                        {
                            break;
                        }
                        OnOFFChange(game_Setting_Text, 3, "OFF", "tutorial");
                        break;
                    case "tutorial_next":
                        spriteRenderer.sprite = previosNext[1];

                        if (game_Setting_Text[3].text.Equals("ON"))
                        {
                            break;
                        }
                        OnOFFChange(game_Setting_Text, 3, "ON", "tutorial");
                        break;
                    case "user_select_setting_btn_n":
                        spriteRenderer.sprite = menuBtn[0];
                        settingBackground[0].SetActive(true);
                        settingBackground[1].SetActive(false);
                        break;
                    case "general_game_setting_btn_n":
                        spriteRenderer.sprite = menuBtn[1];
                        settingBackground[0].SetActive(false);
                        settingBackground[1].SetActive(true);
                        break;
                    case "save_setting_btn_n":
                        spriteRenderer.sprite = menuBtn[2];
                        settingBackground[0].SetActive(false);
                        settingBackground[1].SetActive(false);
                        settingBackground[2].SetActive(true);
                        break;
                }
                SoundManager.clickSound();
                handCheck = false;
                playerNumber = PlayerPrefs.GetInt("user");
            }
            
        }

        // 스프라이트에서 들어갔다가 빠져나왔을 경우
        if (Physics2D.OverlapPoint(hand.transform.position) == null && spriteCheck)
        {
            check = false;
            handCheck = false;

            if (handSource.status.Equals(HandMouseSource.HandMouseStatus.CLICK))
            {
                UserBackgroundChange(playerNumber);
            }

            for(int i = 0; i< optionRenderer.Length; i++)
            {
                // 환자선택
                if(i == 0 || i == 7)
                {
                    optionRenderer[i].sprite = menuBtn[0];
                }

                // 게임설정
                if(i == 1 || i == 8)
                {
                    optionRenderer[i].sprite = menuBtn[1];
                }
                
                // 저장하기
                if(i == 2 || i == 9)
                {
                    optionRenderer[i].sprite = menuBtn[2];
                }

                // 이전
                if (i == 3 || i == 5 || i == 10 || i == 12 || i == 14 || i == 16)
                {
                    optionRenderer[i].sprite = previosNext[0];
                }

                // 다음
                if(i == 4 || i == 6 || i == 11 || i == 13 || i == 15 || i == 17)
                {
                    optionRenderer[i].sprite = previosNext[1];
                }

                // 홈 - EXIT
                if(i == 18)
                {
                    optionRenderer[i].sprite = exitBtn[0];
                }
                
            }

            spriteCheck = false;
        }
    }

    // 텍스트 order 순서
    private void TextOrder(MeshRenderer[] text)
    {
        for(int i = 0; i < text.Length; i++)
        {
            text[i].sortingOrder = 10;
        }
    }

    // 임시로 4개만 불러오기
    private void UserLoadData(TextMesh[] textArray, int number)
    {
        textArray[0].text = data.playerData[number].name;
        textArray[1].text = "남자    " + data.playerData[number].age + "세";
        textArray[2].text = "환자번호  "+ data.playerData[number].number;
    }

    // 유저 바탕색 변경
    private void UserBackgroundChange(int number)
    {
        for (int i = 0; i< userSpriteRenderer.Length; i++)
        {
            if(i == number)
            {
                continue;
            }
            userSpriteRenderer[i].sprite = userSprite[0];
            textArray[i][0].color = colorUserName;
            textArray[i][1].color = colorUserAge;
        }
    }

    // 텍스트 온오프
    private void OnOFFChange(TextMesh[] text ,int Number, string onOff, string prefs)
    {
        switch (onOff)
        {
            case "ON":
                text[Number].text = "ON";
                PlayerPrefs.SetString(prefs, "ON");
                break;
            case "OFF":
                text[Number].text = "OFF";
                PlayerPrefs.SetString(prefs, "OFF");
                break;
            case "왼손":
                text[Number].text = "왼손";
                PlayerPrefs.SetString(prefs, "왼손");
                break;
            case "오른손":
                text[Number].text = "오른손";
                PlayerPrefs.SetString(prefs, "오른손");
                break;
        }
    }

    // 데이터가 저장되어있는지
    private void CheckPlayerPrefbs(TextMesh[] text,int number, string prefbs)
    {
        if (!PlayerPrefs.GetString(prefbs).Equals(""))
        {
            text[number].text = PlayerPrefs.GetString(prefbs);
        }
        else
        {
            if (prefbs.Equals("control"))
            {
                text[number].text = "오른손";
                PlayerPrefs.SetString(prefbs, "오른손");
            }
            else
            {
                text[number].text = "ON";
                PlayerPrefs.SetString(prefbs, "ON");
            }
        }
    }
}
