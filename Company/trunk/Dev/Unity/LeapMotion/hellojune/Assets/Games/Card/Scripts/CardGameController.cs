using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardGameController : AbstractGameController
{
    private string gameName = "card";

    public Texture[] colorTextures = new Texture[4];
    public string[] colorTexts = new string[4] { "Red", "Blue", "Green", "Pink" };
    public Color[] colors = new Color[4];

    public Texture[] animals = new Texture[16];

    public GameObject[] cards = new GameObject[16];
    public GameObject[] cardImgs = new GameObject[16];

    public int nowGameLevel = 1;
    public bool nowTouchable;
    private int[] cardIdx = new int[3] {8, 12, 16};
    private int[] shuffledInt = new int[16];
    private int remainCardCnt = 8;

    private int nowTouchedCardCount = 0;
    private GameObject firstTouchedCard;
    private GameObject secondTouchedCard;

    public Text gameLevelText;
    public Text gameGoalText;

    //시간, 점수
    public Text durationText;
    public Text pointText;
    private int sumOfPlayTime = 0;
    private int points = 0;

    public GameObject gameOverUI;
    public GameObject resetUI;
    public GameObject quitUI;

    private bool isGameOver = false;

    void Start()
    {
        nowTouchable = false;

        getShuffledInt(cardIdx[nowGameLevel - 1]);
        MakeCards();

        StartCoroutine(UpdateUIText());
    }

    IEnumerator UpdateUIText()
    {
        gameLevelText.text = "Level " + nowGameLevel;

        if(nowGameLevel != 1)
        {
            gameGoalText.text = "5초 뒤에 동물 카드가 숨겨집니다. 카드의 짝을 마추어 주세요.";
        }

        yield return new WaitForSeconds(5.0f);
        ToBackCards();
        nowTouchable = true;

        while (!isGameOver)
        {
            durationText.text = sumOfPlayTime + " sec";
            pointText.text = points + " point";

            yield return new WaitForSeconds(1.0f);
            sumOfPlayTime += 1;
        }
    }
   
    void getShuffledInt(int arrayLength)
    {
        for (int i = 0; i < arrayLength; i++)
        {
            shuffledInt[i] = Random.Range(0, arrayLength);
            for (int j = 0; j < i; j++)
            {
                if (shuffledInt[i] == shuffledInt[j])
                {
                    i--;
                    break;
                }
            }
        }
    }

    void MakeCards()
    {
        int idx = nowGameLevel - 1;
        remainCardCnt = cardIdx[idx];

        for (int i = 0; i < cardIdx[idx]; i++)
        {
            int tmp = shuffledInt[i];

            if (nowGameLevel == 1)
            {
                if(tmp < 4)
                {
                    cardImgs[i].GetComponent<MeshRenderer>().material.mainTexture = colorTextures[tmp];
                }
                else
                {
                    cardImgs[i].GetComponent<MeshRenderer>().material.color = colors[tmp - 4];
                    cardImgs[i].tag = colorTexts[tmp-4];
                }

                cards[4].transform.localPosition = new Vector3(3.12f, 0.0f, -2.53f);
            }
            else
            {
                cardImgs[i].GetComponent<MeshRenderer>().material.mainTexture = animals[tmp];
                cards[4].transform.localPosition = new Vector3(1.56f, 0.0f, -1.26f);
            }

            cards[i].SetActive(true);
        }
    }

    void ToBackCards()
    {
        int idx = nowGameLevel - 1;
        for (int i = 0; i < cardIdx[idx]; i++)
        {
            cards[i].GetComponent<Animation>().Play("ToBack");
        }
    }


    //오른손 중지에서 트리거이벤트로 호출됨
    public void controlNowTouchedCard(GameObject other)
    {
        if (nowTouchable)
        {
            if (other.transform.localRotation.z == -1 && nowTouchedCardCount < 2)
            {
                other.gameObject.GetComponent<Animation>().Play("ToFront");

                setTwoCardImg(other);

                if (nowTouchedCardCount == 2)
                {
                    Invoke("matchTwoCardImg", 0.8f);
                }
            }
        }
    }

    void setTwoCardImg(GameObject other)
    {
        if(nowTouchedCardCount == 0)
        {
            firstTouchedCard = other;
        }
        else
        {
            secondTouchedCard = other;
        }

        nowTouchedCardCount += 1;
    }

    void matchTwoCardImg()
    {
        int idx1 = int.Parse(firstTouchedCard.name.Replace("Card", ""));
        int idx2 = int.Parse(secondTouchedCard.name.Replace("Card", ""));

        //카드이름 1부터 시작함으로 0부터 인덱스처리
        Material m1 = cardImgs[idx1 - 1].GetComponent<MeshRenderer>().material;
        Material m2 = cardImgs[idx2 - 1].GetComponent<MeshRenderer>().material;

        string firstCardName = m1.mainTexture == null ? cardImgs[idx1 -1].tag : m1.mainTexture.name;
        string secondCardName = m2.mainTexture == null ? cardImgs[idx2 - 1].tag : m2.mainTexture.name;

        if(firstCardName == secondCardName)
        {
            m1.color = Color.white;
            m2.color = Color.white;

            firstTouchedCard.SetActive(false);
            secondTouchedCard.SetActive(false);

            points += 1;
            remainCardCnt -= 2;

            if (remainCardCnt == 0)
            {

                if(nowGameLevel != 3)
                {
                    nowGameLevel += 1;
                    Start();
                }
                else
                {
                    isGameOver = true;
                    gameOverUI.SetActive(true);
                    resetUI.SetActive(true);
                    quitUI.SetActive(true);
                }
            }
        }
        else
        {
            firstTouchedCard.GetComponent<Animation>().Play("ToBack");
            secondTouchedCard.GetComponent<Animation>().Play("ToBack");
        }

        nowTouchedCardCount = 0;
    }



    override public int getTime()
    {
        return sumOfPlayTime;
    }

    override public int getPoints()
    {
        return points;
    }

    override public string getGameName()
    {
        return gameName;
    }

}
