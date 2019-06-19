using UnityEngine;
using System.Collections;

public class MapBtnManager : MonoBehaviour {

    // 맵 버튼
    // 이전, 시작 , 다음 , 홈
    [SerializeField]
    private SpriteRenderer[] mapBtn = null;

    // 맵 선택 모음
    [SerializeField]
    private Sprite[] mapBtnSelect = null;

    // 맵 비선택 모음
    [SerializeField]
    private Sprite[] mapBtnDeselect = null;

    // 맵 스테이지 버튼
    [SerializeField]
    private GameObject[] stages = null;

    // 현재 스테이지
    // 0부터 스테이지 1
    private int stageNumber = 0;

    // - 스테이지 이름 - 
    //      케이크
    // 0, 1, 2, 3, 4, 5
    //        옷
    // 6, 7, 8, 9, 10
    private const int stageSize = 11;
    private string[] stageNames = new string[stageSize];

    void Awake()
    {
        Animator anim = stages[stageNumber].GetComponent<Animator>();

        anim.SetInteger("stage", stageNumber);

        for (int i = 0; i < stageNames.Length; i++)
        {
            stageNames[i] = "Stage " + i;
        }
    }

    // Next 스테이지
    private void StageNext()
    {
        if (stageNumber < 10)
        {
            stageNumber++;
            Animator anim = stages[stageNumber].GetComponent<Animator>();

            anim.SetInteger("stage", stageNumber);

            if (stageNumber - 1 > -1)
            {
                anim = stages[stageNumber - 1].GetComponent<Animator>();
                anim.SetInteger("stage", -1);
            }
        }
    }

    // Previous 이전 스테이지
    private void StagePrevious()
    {
        if (stageNumber > 0)
        {
            stageNumber--;
            Animator anim = stages[stageNumber].GetComponent<Animator>();

            anim.SetInteger("stage", stageNumber);

            if (stageNumber + 1 < 11)
            {
                anim = stages[stageNumber + 1].GetComponent<Animator>();
                anim.SetInteger("stage", -1);
            }
        }
    }

    // 맵 버튼 클릭
    public void btnMapSeleted(int number)
    {
        mapBtn[number].sprite = mapBtnSelect[number];
    }

    // 맵 버튼 클릭 해제
    public void btnMapDeseleted(int number)
    {
        mapBtn[number].sprite = mapBtnDeselect[number];
    }

    // 맵 버튼 이벤트
    public void btnMapChange(int number, HandMouseSource.HandMouseStatus status)
    {
        // 체크 해제
        if (status == HandMouseSource.HandMouseStatus.CLICK && mapBtn[number].sprite == mapBtnDeselect[number])
        {
            btnMapSeleted(number);
        }

        // 이벤트
        if (status == HandMouseSource.HandMouseStatus.IDLE && mapBtn[number].sprite == mapBtnSelect[number])
        {
            switch (number)
            {
                // 이전 , 시작 , 다음 , 홈
                case 0:
                    StagePrevious();
                    break;
                case 1:
                    if (stageSize > stageNumber)
                    {
                        AutoFade.LoadLevel(stageNames[stageNumber], 1, 1, Color.white);
                    }
                    break;
                case 2:
                    StageNext();
                    break;
                case 3:
                    AutoFade.LoadLevel("Intro", 1, 1, Color.white);
                    break;
            }
            SoundManager.clickSound();
            btnMapDeseleted(number);
        }
    }

}
