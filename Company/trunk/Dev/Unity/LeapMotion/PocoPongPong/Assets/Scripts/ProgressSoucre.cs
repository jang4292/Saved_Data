using UnityEngine;
using System.Collections;

public class ProgressSoucre : MonoBehaviour {

	public enum ProgressStatus
	{
		READY, // 준비.
		STOP // 정지.
	}

	// 난이도
	public enum ProgressStep
	{
		EASY, // 쉬움 
		NORAML, // 보통
		HARD // 어려움
	}

	// 게이지바 
	// 0 = 시작, 1 = 중간, 2 = 마지막
	[SerializeField]
	private GameObject[] progress = new GameObject[3];

    // 왼쪽 오른쪽 UI
    [SerializeField]
    private GameObject[] all_UI = new GameObject[1];

	// 상태
	public ProgressStatus progressStatus;

	// 난이도
	public ProgressStep step;

    [SerializeField]
    private GameObject resultBackground = null; // 결과창
    [SerializeField]
    private ResultManager result = null;
    [SerializeField]
    private SpawnManager spawn = null;
    [SerializeField]
    private ScoreSource score = null;
    [SerializeField]
    private TimeSource time = null;

    // Kinect 데이터에 따른 움직임 오브젝트
    private static GameObject kid_Move = null;

    // 엔딩화면
    // 0 = 배경, 1 = 성공, 2 = 성공애니, 3 = 실패, 4 = 실패애니
    public GameObject[] endingBackground = null;

    void Start(){
		progressStatus = ProgressStatus.READY;
        resultBackground.SetActive(false);
        if(kid_Move != null) kid_Move.SetActive(true);
    }

    void Update(){
		// 엔딩화면
		endingAugment(endingBackground[1]);
    }

    // 게이지바 증가 
    public void progressAugment(){
		// 0 = 중간 크기, 1 = 중간 위치, 2 = 마지막 위치
		Vector3[] vector = new Vector3[3];
		vector[0] = progress [1].transform.localScale;
		vector[1] = progress [1].transform.position;
		vector[2] = progress [2].transform.position;

		if (vector [0].x == 0) 
		{
			progress [0].transform.localScale = new Vector3 (1, 1, 1);
			progress [1].transform.localScale = new Vector3 (5, 1, 1);
			progress [2].transform.localScale = new Vector3 (1, 1, 1);
		}
		else if (vector[0].x > 0)
 		{
			// 4번
			if (step == ProgressStep.EASY) {
				ProgressSet (vector, 47f, 0.23f, 0.46f);
			}
			// 8번
			if (step == ProgressStep.NORAML) {
				ProgressSet (vector, 20f, 0.1f, 0.2f);
			}
			// 15번
			if (step == ProgressStep.HARD) {
				ProgressSet (vector, 10f, 0.05f, 0.1f);
			}

			progress [1].transform.localScale = vector[0];
			progress [1].transform.position = vector[1];
			progress [2].transform.position = vector[2];

			if (progress [1].transform.localScale.x > 144) {
                StartCoroutine(ending (true));
                SoundManager.successSound();
                spawn.allRemoveCollider2D();
            }
        }
	}
	private void ProgressSet(Vector3[] vector, float scale, float pos, float endpos){
		vector [0].x += scale;
		vector [1].x += pos;
		vector [2].x += endpos;
	}
   

    // 게이지바가 꽉 채워지면 엔딩 프리팹 활성화 및 왼쪽, 오른쪽 UI 제거 콜린더 제거
    public IEnumerator ending(bool prefab){
        KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Stop;

        progressStatus = ProgressStatus.STOP;
        endingBackground[0].SetActive(true);

        yield return new WaitForSeconds (1f);

        spawn.allRemoveCollider2D();

        if (prefab)
        {
            endingBackground[1].SetActive(true);
            endingBackground[2].SetActive(true);
        }
        else if (prefab == false)
        {
            endingBackground[3].SetActive(true);
            endingBackground[4].SetActive(true);
        }

        if (score.smallCount == 0 && time.time_minute == 0 && Mathf.FloorToInt(time.time_second) == 0)
        {
            result.resultRefresh("0점", "TimeOver", score.smallCount);
        }else if (score.smallCount != 0 && time.time_minute == 0 && Mathf.FloorToInt(time.time_second) == 0)
        {
            result.resultRefresh(score.smallCount + ",000점", "TimeOver", score.smallCount);
        }
        else if(score.smallCount != 0 && (time.time_minute != 0 || Mathf.FloorToInt(time.time_second) != 0))
        {
            result.resultRefresh(score.smallCount + ",000점", time.time_minute / 10 + "" + time.time_minute % 10 + ":" + Mathf.FloorToInt(time.time_second) / 10 + "" + Mathf.FloorToInt(time.time_second) % 10, score.smallCount);
        }

        Destroy(all_UI[0]);
        Destroy(all_UI[1]);

        if(kid_Move == null) kid_Move = GameObject.Find("Kinect_Kid_Move");

        kid_Move.SetActive(false);

        yield return new WaitForSeconds(5f);

        KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Result;

        Destroy(endingBackground[0]);
        if (prefab)
        {
            Destroy(endingBackground[1]);
            Destroy(endingBackground[2]);
        }
        else if (prefab == false)
        {
            Destroy(endingBackground[3]);
            Destroy(endingBackground[4]);
        }

        resultBackground.SetActive(true);

    }

    // 증가 성공 , 실패
    public void endingAugment(GameObject background){
		if (background != null && background.activeInHierarchy && background.transform.localScale.x < 1) {
			Vector2 vector = background.transform.localScale;
			vector.x += 0.03f;
			vector.y += 0.03f;
			background.transform.localScale = vector;
		}
	}
}


