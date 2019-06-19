using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour {

    //배열 순서.
    // 0, 1, 2, 3, 4,
    // 5, 6, 7, 8, 9,
    // 10, 11, 12, 13, 14,
    // 15, 16, 17, 18, 19,
    // 20, 21, 22, 23, 24

    private GameObject[] objArrays = new GameObject[25]; // 조각 저장.
    private int[] typeArrays = new int[25]; // 조각 타입 저장.

    // 조각 위치.
    private Vector3[] spawnPoints = new Vector3[25]
    {
        new Vector3(0f, 3.8f, 0), new Vector3(1.2f, 3.1f, 0), new Vector3(2.4f, 2.4f, 0), new Vector3(3.6f, 1.7f, 0), new Vector3(4.8f, 1f, 0),

        new Vector3(-1.2f, 3.1f, 0), new Vector3(0f, 2.4f, 0), new Vector3(1.2f, 1.7f, 0), new Vector3(2.4f, 1f, 0), new Vector3(3.6f, 0.3f, 0),

        new Vector3(-2.4f, 2.4f, 0), new Vector3(-1.2f, 1.7f, 0), new Vector3(0f, 1f, 0), new Vector3(1.2f, 0.3f, 0), new Vector3(2.4f, -0.4f, 0),

        new Vector3(-3.6f, 1.7f, 0), new Vector3(-2.4f, 1f, 0), new Vector3(-1.2f, 0.3f, 0), new Vector3(0f, -0.4f, 0), new Vector3(1.2f, -1.1f, 0),

        new Vector3(-4.8f, 1f, 0), new Vector3(-3.6f, 0.3f, 0), new Vector3(-2.4f, -0.4f, 0), new Vector3(-1.2f, -1.1f, 0), new Vector3(0f, -1.8f, 0)
    };
    
    // 조각 뎁스.
    private int[] sortingOrders = new int[25]
    {
        1, 2, 3, 4, 5,
        2, 3, 4, 5, 6,
        3, 4, 5, 6, 7,
        4, 5, 6, 7, 8,
        5, 6, 7, 8, 9
    };

    public GameObject tutorial = null;

    public GameObject stopBtn = null;

    private bool check = false;

    private ArrayList destoryQueue = new ArrayList(); // 큐.

    private int stageNumber = 0;

    void Awake()
    {
        int.TryParse(SceneManager.GetActiveScene().name.Substring(6), out stageNumber);

        if (PlayerPrefs.GetString("tutorial").Equals("ON"))
        {
            StartCoroutine(CreatePieces());
            stopBtn.SetActive(false);
        }
        else
        {
            Destroy(tutorial);
            spawnPieces(); // 조각 생성.
        }
    }
    
    IEnumerator CreatePieces()
    {
        while (check == false)
        {
            if (tutorial == null)
            {
                check = true;
                stopBtn.SetActive(true);
                spawnPieces(); // 조각 생성.
            }
            yield return null;
        }
    }

    // 전체 조각 생성.
    public void spawnPieces()
    {
        do
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
				if (objArrays[i] != null) {
					
					Destroy(objArrays[i]);
				}

                Vector3 vector3 = spawnPoints[i];
                GameObject piece = null;

                // 케이크 아이템
                if (stageNumber < 3)
                {
                    piece = Instantiate(Resources.Load("Prefabs/DessertPiece")) as GameObject;
                }
                else if (stageNumber < 6)
                {
                    piece = Instantiate(Resources.Load("Prefabs/DessertPiece2")) as GameObject;
                }
                // 옷 아이템
                else if (stageNumber < 9)
                {
                    piece = Instantiate(Resources.Load("Prefabs/ClothesPiece")) as GameObject;
                }
                else if (stageNumber < 11)
                {
                    piece = Instantiate(Resources.Load("Prefabs/ClothesPiece2")) as GameObject;
                }
				
				PieceSource source = piece.GetComponent<PieceSource>();
                source.create(vector3, i, sortingOrders[i], -1);
                typeArrays[i] = source.typeNumber;
                objArrays[i] = piece;
				piece.transform.localScale = new Vector3(0,0,0);
            }
        }
        while (!hasLinkablePieces()); // 연결 가능한 조각이 없으면 재생성.
    }

    // 개별 조각 생성.
    public void spawnPiece(PieceSource source)
    {
        GameObject piece = null;
        
        // 케이크 아이템
        if (stageNumber < 3)
        {
            piece = Instantiate(Resources.Load("Prefabs/DessertPiece")) as GameObject;
        }
        else if(stageNumber < 6)
        {
            piece = Instantiate(Resources.Load("Prefabs/DessertPiece2")) as GameObject;
        }
        // 옷 아이템
        else if (stageNumber < 9)
        {
            piece = Instantiate(Resources.Load("Prefabs/ClothesPiece")) as GameObject;
        }
        else if (stageNumber < 11)
        {
            piece = Instantiate(Resources.Load("Prefabs/ClothesPiece2")) as GameObject;
        }
        PieceSource dest = piece.GetComponent<PieceSource>();
        dest.create(source.transform.position, source.arrayIndex, source.sortingOrder, source.typeNumber);
        typeArrays[source.arrayIndex] = dest.typeNumber;
        objArrays[source.arrayIndex] = piece;

		piece.transform.localScale = new Vector3(0, 0, 0);
    }
    
    // 전체 조각을 대상으로 연결 가능 여부 판정.
    public bool hasLinkablePieces()
    {
        for(int i = 0; i < 25; i++)
        {
            int typeNumber = typeArrays[i];

            if (HorizontalLinkable(i, typeNumber) || VerticalLinkable(i, typeNumber) || CenterLinkable(i, typeNumber))
            {
                return true;
            }
        }
        return false;
    }

    // 가로 연결 가능 여부. 5줄 밖에 없으므로 3개가 선택되기 위해서는 가운데 줄은 필수로 선택되어야 함.
    private bool HorizontalLinkable(int index, int typeNumber)
    {
        if((index -2) % 5 == 0) // 2, 7, 12, 17, 22.
        {
            if (typeNumber == typeArrays[index - 2] && typeNumber == typeArrays[index - 1]) // @@@##
            {
                return true;
            }
            else if (typeNumber == typeArrays[index + 1] && typeNumber == typeArrays[index + 2]) // ##@@@
            {
                return true;
            }
            else if (typeNumber == typeArrays[index -1] && typeNumber == typeArrays[index + 1]) // #@@@#
            {
                return true;
            }
        }
        return false;
    }

    // 세로 연결 가능 여부. 5줄 밖에 없으므로 3개가 선택되기 위해서는 가운데 줄은 필수로 선택되어야 함.
    private bool VerticalLinkable(int index, int typeNumber)
    {
        if(index > 9 && index < 15) // 10, 11, 12, 13, 14.
        {
            if (typeNumber == typeArrays[index - 10] && typeNumber == typeArrays[index - 5])
            {
                // @
                // @
                // @
                // #
                // #
                return true;
            }
            else if (typeNumber == typeArrays[index + 5] && typeNumber == typeArrays[index + 10])
            {
                // #
                // #
                // @
                // @
                // @
                return true;
            }
            else if(typeNumber == typeArrays[index - 5] && typeNumber == typeArrays[index + 5])
            {
                // #
                // @
                // @
                // @
                // #
                return true;
            }
        }
        return false;
    }

    // 가로, 세로 한칸 씩 연결 가능 여부.
    private bool CenterLinkable(int index, int typeNumber)
    {
        // #@#
        // @@@
        // #@#
        if(((index % 5 != 0 && index - 1 >= 0 && typeArrays[index - 1] == typeNumber) // 0, 5, 10, 15, 20 제외.
            || ((index -4) % 5 != 0 && index + 1 < 25 && typeArrays[index + 1] == typeNumber)) // 4, 9, 14, 19, 24 제외.

            && ((index > 4 && typeArrays[index - 5] == typeNumber) // 0, 1, 2, 3, 4 제외.
            || (index + 5 < 25 && typeArrays[index + 5] == typeNumber))) // 20, 21, 22, 23, 24 제외.
        {
            return true;
        }
        return false;
    }

    // 큐에 저장.
    public void addToDestoryQueue(PieceSource source)
    {
        destoryQueue.Add(source);
    }

    // 큐에서 제거.
    public void removeToDestroyQueue(PieceSource source)
    {
        destoryQueue.Remove(source);
        spawnPiece(source); // 새로운 조각 생성.
        if(destoryQueue.Count == 0)
        {
            if (!hasLinkablePieces()) // 연결 가능한 조각이 없으면 재생성.
            {
                spawnPieces();
            }
        }
    }

	// 전체 조각 콜린더 제거
	public void allRemoveCollider2D()
	{
		for (int i = 0; i < objArrays.Length; i++) {
			Destroy (objArrays[i].GetComponent<CircleCollider2D> ());
			BoxCollider2D[] boxColl = objArrays[i].GetComponents<BoxCollider2D> ();

			for (int a = 0; a < boxColl.Length; a++) {
                Destroy (boxColl[a]);	
			}
		}
	}
}
