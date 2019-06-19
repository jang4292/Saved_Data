using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSource : MonoBehaviour {

    public enum TimeStatus
    {
        Idle, // 기본
        Stop // 정지
    }

    public TimeStatus status;

    // 숫자 sprite
    // 0,1,2,3,4,5,6,7,8,9
    [SerializeField]
    private Sprite[] timeNumber = new Sprite[10];

    // 숫자 spriteRnederer
    // 0 0 : 0 0
    [SerializeField]
    private SpriteRenderer[] timeSprite = new SpriteRenderer[4];

    // 10초이하 임펙트
    [SerializeField]
    private GameObject timeEffect = null;

    [SerializeField]
    private ProgressSoucre progress = null;

    [SerializeField]
    private SpawnManager spawn = null;

    public bool secondCheck = false;

    // 분
    [SerializeField]
    private int minute;

    // 초
    [SerializeField]
    private float second;

    public int time_minute { get { return minute; } set { minute = value; } }
    public float time_second { get { return second; } set { second = value; } }

    // 튜토리얼
    public GameObject tutorial = null;
    public Animator tutorialAnim = null;

    void Start(){
        timeSprite[1].sprite = timeNumber[minute];
        status = TimeStatus.Idle;
        KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Stage;
    }

    void Update () {
        // 튜토리얼 끝나면 제거
        if (tutorial != null && tutorialAnim.GetBool("tutorial")) Destroy(tutorial);

		// 분 > 0 , 초 = 0
		if (minute > 0 && Mathf.FloorToInt (second) == 0 && tutorial == null) RemoveMinute ();
		// 게이지 상태 레디  , 초 > 0
		else if (progress.progressStatus == ProgressSoucre.ProgressStatus.READY && Mathf.FloorToInt (second) > 0 && tutorial == null) RemoveSecond ();

		// 분 = 0, 초 = 10
		if (minute == 0 && Mathf.FloorToInt (second) == 10 && secondCheck.Equals(false)) {
            SoundManager.timeSoundStart();
            timeEffect.SetActive (true);
			secondCheck = true;
		}
		// 분 = 0, 초 = 0, secondCheck = true
		else if (minute == 0 && Mathf.FloorToInt (second) == 0 && secondCheck && progress.progressStatus == ProgressSoucre.ProgressStatus.READY) {
            SoundManager.failSound();
            Destroy(timeEffect);
			secondCheck = false;
            spawn.allRemoveCollider2D();
            StartCoroutine(progress.ending(false));
		}

		progress.endingAugment (progress.endingBackground[3]);
	}

	// 분 제거
	private void RemoveMinute(){
		minute -= 1;	
		second = 60;

		if (minute < 10) {
			timeSprite [1].sprite = timeNumber [minute];
		} else if(minute > 9){
			timeSprite [0].sprite = timeNumber [minute / 10];
			timeSprite [1].sprite = timeNumber [minute % 10];
		}
	}

	// 초 제거
	private void RemoveSecond(){
        if (status == TimeStatus.Idle)
        {
            second -= Time.deltaTime;

            timeSprite[2].sprite = timeNumber[Mathf.FloorToInt(second) / 10];
            timeSprite[3].sprite = timeNumber[Mathf.FloorToInt(second) % 10];
        }
	}
}
