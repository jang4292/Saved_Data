using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonSource : MonoBehaviour {

    // 결과 버튼
    // 홈, 맵, 다시시작, 정지버튼
    [SerializeField]
    private SpriteRenderer[] resultBtn = null;

    // 결과 선택, 정지 모음 
    [SerializeField]
    private Sprite[] resultSelect = null;

    // 결과 비선택, 정지 모음
    [SerializeField]
    private Sprite[] resultDeselect = null;

    [SerializeField]
    private TimeSource time = null;

    [SerializeField]
    private ProgressSoucre progress = null;

    // 결과, 정지 버튼 클릭
    public void btnResultSeleted(int number)
    {
        resultBtn[number].sprite = resultSelect[number];
    }

    // 결과, 정지 버튼 클릭 해제
    public void btnResultDeseleted(int number)
    {
        resultBtn[number].sprite = resultDeselect[number];
    }

    // 결과, 정지 버튼 이벤트
    public void btnResultChange(int number, HandMouseSource.HandMouseStatus status)
    {
        string stageName = SceneManager.GetActiveScene().name;
        // 체크 해제
        if (status == HandMouseSource.HandMouseStatus.CLICK && resultBtn[number].sprite == resultDeselect[number])
        {
            btnResultSeleted(number);
        }

        // 이벤트
        if (status == HandMouseSource.HandMouseStatus.IDLE && resultBtn[number].sprite == resultSelect[number])
        {
            switch (number)
            {
                // 홈, 맵, 다시시작, 정지
                case 0:
                    AutoFade.LoadLevel("Intro", 1, 1, Color.white);
                    KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Intro;
                    break;
                case 1:
                    AutoFade.LoadLevel("Map", 1, 1, Color.white);
                    KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Intro;
                    break;
                case 2:
                    AutoFade.LoadLevel(stageName, 1, 1, Color.white);
                    break;
                case 3:
                    time.status = TimeSource.TimeStatus.Stop;
                    time.time_minute = 0;
                    time.time_second = 0;
                    time.secondCheck = true;
                    progress.progressStatus = ProgressSoucre.ProgressStatus.READY;
                    break;
            }
            SoundManager.clickSound();
            btnResultDeseleted(number);
        }
    } 
}
