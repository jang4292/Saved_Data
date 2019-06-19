using UnityEngine;
using System.Collections;

public class PieceSource : MonoBehaviour {

    // 케이커 정보.
    private string[] CAKE_NAMES = new string[5] { "Strawberry", "Cherry", "Melon", "Cookie", "Orange" };

    // 케이크 정보
    [SerializeField]
    private Sprite[] cake_Default = null;
    [SerializeField]
    private Sprite[] cake_Selected = null;

	// 0 = 딸기 , 1 = 체리, 2 = 메론, 3 = 쿠키, 4 = 오렌지
	[SerializeField]
	private Sprite[] smallCake = new Sprite[5];

    public enum PieceStatus
    {
        CREATED, // 생성.
        DEFAULT, // 기본.
        SELECTED, // 선택.
        Destory, // 제거.
    }

    private Sprite statusDefault; // 기본 상태.
    private Sprite statusSelected; // 선택 상태.

    private SpriteRenderer spriteRenderer;
    public int sortingOrder; // 뎁스.

    public int arrayIndex; // 배열 인덱스. 연결 가능 위치 판정.
    public int typeNumber; // 종류. 연결 가능 모양 판정.

    public PieceStatus pieceStatus; // 조각 상태.

    void Update()
    {
        if (pieceStatus == PieceStatus.CREATED) CreateAnim(); // 생성 직후 확대 애니.
        else if (pieceStatus == PieceStatus.Destory) DestroyAnim(); // 제거 직전 축소 애니.
    }
    
    // 모양 및 위치 설정.
    public void create(Vector3 vector3, int arrayIndex, int sortingOrder, int prevNum)
    {
        do
        {
            typeNumber = Random.Range(0, 5); // 랜덤 타입 생성.
        }
        while (typeNumber == prevNum); // 이전 타입과 동일하면 재생성.

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.position = vector3;
        statusDefault = cake_Default[typeNumber];
        statusSelected = cake_Selected[typeNumber];
        spriteRenderer.sprite = statusDefault;
        spriteRenderer.sortingOrder =  this.sortingOrder = sortingOrder; // 뎁스.
        gameObject.name = CAKE_NAMES[typeNumber]; // 이름.
        this.arrayIndex = arrayIndex; // 배열 인덱스.
    }

    // 선택.
    public void select()
    {
        if (spriteRenderer == null) return;
        pieceStatus = PieceStatus.SELECTED;
        spriteRenderer.sprite = statusSelected;
		SpriteRenderer smallSprite = GameObject.Find ("SmallCake").GetComponent<SpriteRenderer> ();
		smallSprite.sprite = smallCake[typeNumber];
        SoundManager.clickSound();
    }
    
    // 선택 해제.
    public void deselect()
    {
        if (spriteRenderer == null) return;
        pieceStatus = PieceStatus.DEFAULT;
        spriteRenderer.sprite = statusDefault;
    }

    [SerializeField]
    private GameObject boom = null;

    // 이펙트.
    public IEnumerator Boom()
    {
        GameObject pre_boom = (GameObject)Instantiate(boom);
        Vector3 vector3 = transform.position;
        vector3.y += 0.9f; // 위치 조정.
        pre_boom.transform.position = vector3;
        Animator animator = pre_boom.GetComponent<Animator>();
        animator.SetTrigger("boom");
        yield return new WaitForSeconds(2); // 2초 후 제거.
        Destroy(pre_boom);
        Resources.UnloadUnusedAssets();
    }

    // 생서 애니메이션.
    void CreateAnim()
    {
        Vector3 vector3 = gameObject.transform.localScale;
        if (vector3.x < 1) // 1보다 작은면 확대.
        {
            vector3.x += 0.05f;
            vector3.y += 0.05f;
            gameObject.transform.localScale = vector3;
        }
        else
        {
            pieceStatus = PieceStatus.DEFAULT;
            vector3.x = 1;
            vector3.y = 1;
            gameObject.transform.localScale = vector3;
        }
    }

    // 제거 애니메이션.
    void DestroyAnim()
    {
        Vector3 vector3 = gameObject.transform.localScale;
        if (vector3.x > 0) // 0보다 크면 계속 축소.
        {
            vector3.x -= 0.05f;
            vector3.y -= 0.05f;
            gameObject.transform.localScale = vector3;
        } else
        {
            Destroy(gameObject);

            SpawnManager mgr = GameObject.Find("TableManager").GetComponent<SpawnManager>(); // 조각 생성 매니저.
            mgr.removeToDestroyQueue(this);
        }
    }
}
