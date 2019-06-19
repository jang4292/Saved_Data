using UnityEngine;
using System.Collections;

public class ScoreSource : MonoBehaviour {

	// 큰 숫자 모음
	[SerializeField]
	private Sprite[] bigNumber = new Sprite[10];
	// 작은 숫자 모음 "," 포함 10 = comma
	[SerializeField]
	private Sprite[] smallNumber = new Sprite[10];
	// 큰 케이크 숫자 
	// 0 = x, 1 = 첫번째 숫자, 2 = 두번째 숫자
	[SerializeField]
	private GameObject[] topNumber = new GameObject[3];

	// 작은 숫자 오브젝트 모음
	// 0 = 첫번째, 1 = 두번째, 2 = 세번째
	[SerializeField]
	private GameObject[] middleNumber = new GameObject[6];

	// Score 이펙트.
	public IEnumerator scoreEffect()
	{
		GameObject score = GameObject.Find ("Effect_Score");
		Animator animator = score.GetComponent<Animator>();
		animator.SetBool("score",true);
		yield return new WaitForSeconds(1f); 
		animator.SetBool("score",false);
	}

	// 큰 숫자 변경
	public void scoreBig(int count){
		Vector3[] vector = new Vector3[2];
		vector [0] = topNumber [0].transform.position;
		vector [1] = topNumber [2].transform.position;

		SpriteRenderer[] sprite = new SpriteRenderer[2];
		sprite [0] = topNumber [1].GetComponent<SpriteRenderer> ();
		sprite [1] = topNumber [2].GetComponent<SpriteRenderer> ();


		if (count < 10) {
			topNumber [1].SetActive (false);

			vector [0].x = -7.66f;
			vector [1].x = -7.1f;

			sprite [1].sprite = bigNumber [count];
		} else if (count > 9) {
			topNumber [1].SetActive (true);

			vector [0].x = -7.75f;
			vector [1].x = -6.9f;

			sprite[0].sprite = bigNumber[count/10];
			sprite [1].sprite = bigNumber [count % 10];
		}

		topNumber [0].transform.position = vector [0];
		topNumber [2].transform.position = vector [1];
	}

	private int smallSize = 0;

    public int smallCount { get { return smallSize; } }

	public void scoreSmall(int count){
		smallSize += count;

		SpriteRenderer[] sprite = new SpriteRenderer[3];
		sprite [0] = middleNumber [0].GetComponent<SpriteRenderer> ();
		sprite [1] = middleNumber [1].GetComponent<SpriteRenderer> ();
		sprite [2] = middleNumber [2].GetComponent<SpriteRenderer> ();

		if (middleNumber [2].activeInHierarchy == false) {
			middleNumber [2].SetActive (true);
			middleNumber [3].SetActive (true);
			middleNumber [4].SetActive (true);
			middleNumber [5].SetActive (true);
		}

		if (smallSize < 10) {
			sprite [2].sprite = smallNumber [smallSize];
		}

		else if (smallSize > 9 && smallSize < 100) {
			if (middleNumber [1].activeInHierarchy == false) {
				middleNumber [1].SetActive (true);
			}

			sprite [1].sprite = smallNumber [smallSize/10];
			sprite [2].sprite = smallNumber [smallSize%10];
		}

		else if (smallSize > 99) {
			if (middleNumber [0].activeInHierarchy == false) {
				middleNumber [0].SetActive (true);
			}

			sprite [0].sprite = smallNumber [smallSize/100];
			sprite [1].sprite = smallNumber [smallSize%100/10];
			sprite [2].sprite = smallNumber [smallSize%100%10];
		}
	}

    // 토끼 애니 오브젝트
    [SerializeField]
    private GameObject bunnyAnim = null;

    // 토끼옷 상태
    [SerializeField]
    private SkinnedMeshRenderer clotheRenderer = null;

    // 토끼 옷을 골라줄 스몰 옷
    [SerializeField]
    private SpriteRenderer smallSprite = null;

    // 토끼 옷
    [SerializeField]
    private Material[] clothes = null;

    [SerializeField]
    private GameObject spin = null;

    private Animator spinAnim = null;

    public IEnumerator eatAnimation()
    {
        Animator anim = bunnyAnim.GetComponent<Animator>();
        anim.SetTrigger("bunny");

        if(spin != null)
        {
            spinAnim = spin.GetComponent<Animator>();
            spinAnim.SetTrigger("change");

            yield return new WaitForSeconds(0.5f);

            if(clotheRenderer != null) {

                switch (smallSprite.sprite.name)
                {
                    case "small_clothes1":
                        clotheRenderer.material = clothes[0];
                        break;
                    case "small_clothes2":
                        clotheRenderer.material = clothes[1];
                        break;
                    case "small_clothes3":
                        clotheRenderer.material = clothes[2];
                        break;
                    case "small_clothes4":
                        clotheRenderer.material = clothes[3];
                        break;
                    case "small_clothes5":
                        clotheRenderer.material = clothes[4];
                        break;
                    case "small_clothes6":
                        clotheRenderer.material = clothes[5];
                        break;
                    case "small_clothes7":
                        clotheRenderer.material = clothes[6];
                        break;
                    case "small_clothes8":
                        clotheRenderer.material = clothes[7];
                        break;
                    case "small_clothes9":
                        clotheRenderer.material = clothes[8];
                        break;
                    case "small_clothes10":
                        clotheRenderer.material = clothes[9];
                        break;
                }
            }

        }
        
        yield return new WaitForSeconds(1f);

        if (spin != null)
        {
            spinAnim.SetTrigger("change");
        }

        if(bunnyAnim != null)
        {
            anim.SetTrigger("bunny");
        }
    }
}
