using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HandMouseSource : MonoBehaviour {

    public enum HandMouseStatus
    {
        NONE, // 설정 안됨.
        IDLE, // 대기.
        CLICK // 클릭.
    }

    public Sprite spriteIdle; // 대기 상태.
    public Sprite spriteClick; // 클리그 상태.

    public HandMouseStatus status; // 상태.

    private SpriteRenderer spriteRenderer;

    private Collider2D handCollider;

    private PieceSource prevPiece; // 이전에 선탁한 조각.
    private ArrayList selectedPieces = new ArrayList(); // 선택된 조각 모음.

	private ScoreSource score; // 점수

	private Animator handAnim; // 동작 상태

    private bool withKinect = false;
    private bool withLeapMotion = true;

    // 스코어, 동작감지상태, 사람 애니
    [SerializeField]
    private GameObject[] scoreHand = null;
    // 결과 버튼 소스
    [SerializeField]
    private ButtonSource resultbtn = null;

    // 맵 버튼
    [SerializeField]
    private MapBtnManager mapBtn = null;

    // 인트로 매니저
    [SerializeField]
    private IntroManager introBtn = null;

    // 조각 생성 매니저
    [SerializeField]
    private SpawnManager mgr = null;
    // 게이지
    [SerializeField]
    private ProgressSoucre pro = null;

    // 말풍선
    public GameObject talkObj = null;

    // 로고
    public GameObject logoObj = null;

    // Use this for initialization
    void Start () {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 15;
        
        handCollider = gameObject.GetComponent<BoxCollider2D>();

        if(scoreHand.Length != 0) { 
            score = scoreHand[0].GetComponent<ScoreSource>(); // 스코어 
            handAnim = scoreHand[1].GetComponent<Animator>(); // 손 애니
        }

        if (!withKinect)
        {
            OnStatusChanged(HandMouseStatus.IDLE);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 17;
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    // 이전 마우스 포지션
    private Vector3 handPos = Vector3.zero;

    // 마우스 움직임
    private Vector3 movePos = Vector3.zero;

    // 말풍선 움직임
    private Vector3 talkPos = Vector3.zero;

    float moveing = .01f;

    float movingNumber = 0;

    void Update()
    {
        // 말풍선
        movePos = handPos - gameObject.transform.position;

        if (status.Equals(HandMouseStatus.IDLE) && movePos.x > -moveing && movePos.x < moveing && movePos.y > -moveing && movePos.y < moveing)
        {
            movingNumber += Time.deltaTime;
        }
        else
        {
            movingNumber = 0;
            talkObj.SetActive(false);
        }

        if (movingNumber > 6)
        {
            talkObj.SetActive(true);
            talkPos = gameObject.transform.position;
            talkPos.y -= 1.5f;
            talkObj.transform.position = talkPos;
        }

        handPos = gameObject.transform.position;

        if(handAnim != null)
        {
            score.scoreBig(selectedPieces.Count);
            if (spriteRenderer.sprite == spriteClick)
            {
                handAnim.SetBool("move", true);
            }
            else
            {
                handAnim.SetBool("move", false);
            }
        }

        if (!withKinect && !withLeapMotion)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 17;
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                OnStatusChanged(HandMouseStatus.CLICK);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnStatusChanged(HandMouseStatus.IDLE);
            }
        }
    }

    // 상태 변화 이벤트.
    public void OnStatusChanged(HandMouseStatus status)
    {
        if (this.status == status || spriteRenderer == null) return; // 동일 상태이거나 렌더러가 널이면 제외.

        // 점수 판정.
        if (this.status == HandMouseStatus.CLICK && status == HandMouseStatus.IDLE)
        {
            CheckSelectedPieces();
        }

        // 클릭.
        else if(this.status == HandMouseStatus.IDLE && status == HandMouseStatus.CLICK)
        {
            //콜라이더 트리거만으로 클릭 판정이 부족해서 추가.
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                // 클릭 판정됨.
                if(hit.transform.tag == "Piece")
                {
                    PieceSource piece = hit.transform.gameObject.GetComponent<PieceSource>();
                    if(piece.pieceStatus == PieceSource.PieceStatus.DEFAULT)
                    {
                        SelectPiece(piece); // 선택.
                        break;
                    }
                }
            }

        }

        // 상태 변화.
        this.status = status; // 상태 저장.
        if(this.status == HandMouseStatus.IDLE) // 대기.
        {
            // logoObj 가 있으면 true, 없으면 false
            handCollider.isTrigger = logoObj != null ? true : false;
            spriteRenderer.sprite = spriteIdle;
        } else if(this.status == HandMouseStatus.CLICK) // 클릭.
        {
            spriteRenderer.sprite = spriteClick;
            handCollider.isTrigger = true; // 트리거 켬.
        }
    }

    // 콜라이더 트리거 판정.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (status == HandMouseStatus.CLICK)
        {
            switch (other.tag)
            {
                // 조각
                case "Piece":
                    PieceSource piece = other.gameObject.GetComponent<PieceSource>();
                    if (piece.pieceStatus == PieceSource.PieceStatus.DEFAULT) // 선택되지 않은 조각만 처리.
                    {
                        if (prevPiece != null)
                        {
                            if (prevPiece.typeNumber == piece.typeNumber // 같은 타입의 조각만.
                                && IsLinkable(piece)) // 인접한 조각들만(대각선 제외).
                            {
                                SelectPiece(piece); // 선택.
                            }
                        }
                        // 최근 조각이 없으면 클릭으로 판정.
                        else
                        {
                            SelectPiece(piece); // 선택.
                        }
                    }
                    break;
                // 결과 버튼
                case "Home":
                    resultbtn.btnResultSeleted(0);
                    break;
                case "Map":
                    resultbtn.btnResultSeleted(1);
                    break;
                case "Reset":
                    resultbtn.btnResultSeleted(2);
                    break;
                case "Stop":
                    resultbtn.btnResultSeleted(3);
                    break;
                // 맵
                case "Previous":
                    mapBtn.btnMapSeleted(0);
                    break;
                case "Start":
                    mapBtn.btnMapSeleted(1);
                    break;
                case "Next":
                    mapBtn.btnMapSeleted(2);
                    break;
                case "MapHome":
                    mapBtn.btnMapSeleted(3);
                    break;
                // 인트로
                case "IntroStart":
                    introBtn.btnIntroSeleted(0);
                    break;
                case "Option":
                    introBtn.btnIntroSeleted(1);
                    break;
                case "Resque":
                    introBtn.btnIntroSeleted(2);
                    break;
                case "Exit":
                    introBtn.btnIntroSeleted(3);
                    break;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.tag)
        {
            // 결과
            case "Home":
                resultbtn.btnResultChange(0, status);
                break;
            case "Map":
                resultbtn.btnResultChange(1, status);
                break;
            case "Reset":
                resultbtn.btnResultChange(2, status);
                break;
            case "Stop":
                resultbtn.btnResultChange(3, status);
                break;
            // 맵
            case "Previous":
                mapBtn.btnMapChange(0, status);
                break;
            case "Start":
                mapBtn.btnMapChange(1, status);
                break;
            case "Next":
                mapBtn.btnMapChange(2, status);
                break;
            case "MapHome":
                mapBtn.btnMapChange(3, status);
                break;
            // 인트로
            case "IntroStart":
                introBtn.btnMapChange(0, status);
                break;
            case "Option":
                introBtn.btnMapChange(1, status);
                break;
            case "Resque":
                introBtn.btnMapChange(2, status);
                break;
            case "Exit":
                introBtn.btnMapChange(3, status);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            // 결과
            case "Home":
                resultbtn.btnResultDeseleted(0);
                break;
            case "Map":
                resultbtn.btnResultDeseleted(1);
                break;
            case "Reset":
                resultbtn.btnResultDeseleted(2);
                break;
            case "Stop":
                resultbtn.btnResultDeseleted(3);
                break;
            // 맵
            case "Previous":
                mapBtn.btnMapDeseleted(0);
                break;
            case "Start":
                mapBtn.btnMapDeseleted(1);
                break;
            case "Next":
                mapBtn.btnMapDeseleted(2);
                break;
            case "MapHome":
                mapBtn.btnMapDeseleted(3);
                break;
            // 인트로
            case "IntroStart":
                introBtn.btnIntroDeseleted(0);
                break;
            case "Option":
                introBtn.btnIntroDeseleted(1);
                break;
            case "Resque":
                introBtn.btnIntroDeseleted(2);
                break;
            case "Exit":
                introBtn.btnIntroDeseleted(3);
                break;
        }
    }
    
    // 조각 선택.
    void SelectPiece(PieceSource piece)
    {
        prevPiece = piece; // 최근 조각에 저장.
        piece.select(); // 조각 상태 변화.
        selectedPieces.Add(piece); // 선택된 조각 모음에 저장.
    }

    // 인접한 조각(대각선 제외)이면 true.
    bool IsLinkable(Vector3 source, Vector3 dest)
    {
        float x = Mathf.Abs(source.x - dest.x); // 1.2f 간격으로 설정.
        float y = Mathf.Abs(source.y - dest.y); // 0.7f 간격으로 성정.
        return x > 1.1 && x < 1.3 && y > 0.6 && y < 0.8;
    }

    // 선택된 조각 모음과의 인접한지 여부.
    bool IsLinkable(PieceSource dest)
    {
        foreach(PieceSource selectedPiece in selectedPieces)
        {
            if (IsLinkable(selectedPiece.transform.position, dest.transform.position))
                return true;
        }
        return false;
    }

    // 선택된 조감 모음 판정.
    void CheckSelectedPieces()
    {
        // 3개 이상이면 점수로 환산.
        if(selectedPieces.Count > 2 && score != null)
        {
            DestroyAndSpawnSelectedPieces(); // 선택된 조각 모음 전체 제거.
        }
        // 2개 이하면 선택 해제. 
        else
        {
            ClearSelectedPieces(); // 선택된 조각 모음 상태 전체 해제.
        }
        selectedPieces.Clear(); // 조각 모음 비우기.
        prevPiece = null; // 최근 조각 비우기.
    }

    // 선택된 조각 모음 상태 전체 해제.
    void ClearSelectedPieces()
    {
        if(!SceneManager.GetActiveScene().name.Equals("Intro") && !SceneManager.GetActiveScene().name.Equals("Map") && selectedPieces.Count > 0)
        {
            foreach (PieceSource selectedPiece in selectedPieces)
            {
                selectedPiece.deselect();
            }
            SoundManager.cakeClickRelease();
        }
    }

    // 선택된 조각 모음 전체 제거.
    void DestroyAndSpawnSelectedPieces()
    {
        if(score != null) {
            StartCoroutine(score.scoreEffect());
            StartCoroutine(score.eatAnimation());
            score.scoreSmall(selectedPieces.Count);
        }

        // 선택된 조각 전체.
        foreach (PieceSource selectedPiece in selectedPieces)
        {
            StartCoroutine(selectedPiece.Boom()); // 이펙트 생성.

            selectedPiece.pieceStatus = PieceSource.PieceStatus.Destory; // 조각 상태 변경.

            if(mgr != null)
            mgr.addToDestoryQueue(selectedPiece); // 큐에 저장.
        }

		if (pro.progressStatus == ProgressSoucre.ProgressStatus.READY && pro != null) {
			pro.progressAugment ();		
		}

        SoundManager.cakeDestroySound();
    }
}
