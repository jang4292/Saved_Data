using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    // 0 = 클릭, 1 = 10초미만, 성공, 실패, 2 = 케이크 이펙트 사운드, 케이크 노이펙트 사운드 , 3 = 사이렌
    private static AudioSource[] soundSource = new AudioSource[4];

    // 0 = 클릭, 1 = 10초미만, 2 = 성공, 3 = 실패, 4 = 케이크 없앰, 5 = 케이크 해제, 6 = 사이렌
    private static AudioClip[] soundClip = new AudioClip[7];

    private static GameObject soundObj = null;

    // 인스턴스화
    private static SoundManager m_Instance = null;
    // 효과가 진행되고 있는지
    private bool m_Fading = false;

    // 생성 되어있는지
    private static bool check = false;

    // 다른 클래스에서 인스턴스화를 하지 않고 불러도 되게
    private static SoundManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = soundObj.GetComponent<SoundManager>();
                for (int i = 0; i < soundSource.Length; i++)
                {
                    soundSource[i] = soundObj.AddComponent<AudioSource>();
                    soundSource[i].volume = 0.5f;
                    soundSource[i].playOnAwake = false;
                }
            }
            return m_Instance;
        }
    }

    // 효과가 진행되고 있는지 체크
    public static bool fading
    {
        get
        {
            if (soundObj != null)
            {
                return Instance.m_Fading;
            }
            else
            {
                return true;
            }
        }
    }

    void Awake()
    {
        if(check == false)
        {
            DontDestroyOnLoad(this);
            check = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        soundObj = GameObject.Find("SoundManager");
        
        soundClip[0] = Resources.Load("Sound/Effect/Click") as AudioClip;
        soundClip[1] = Resources.Load("Sound/Effect/Time") as AudioClip;
        soundClip[2] = Resources.Load("Sound/Effect/Success") as AudioClip;
        soundClip[3] = Resources.Load("Sound/Effect/Fail") as AudioClip;
        soundClip[4] = Resources.Load("Sound/Effect/CakeDestroy") as AudioClip;
        soundClip[5] = Resources.Load("Sound/Effect/CakeNoDestroy") as AudioClip;
        soundClip[6] = Resources.Load("Sound/Effect/Siren") as AudioClip;
    }

    // 클릭 사운드
    public static void clickSound()
    {
        if (fading) return;
        if (!soundObj.activeInHierarchy) return;
        soundSource[0].clip = soundClip[0];
        soundSource[0].Play();
    }

    // 10초 미만 사운드
    public static void timeSoundStart()
    {
        if (soundObj == null) return;
        if (!soundObj.activeInHierarchy) return;
        soundSource[1].clip = soundClip[1];
        soundSource[1].loop = true;
        soundSource[1].Play();
    }

    // 성공 사운드
    public static void successSound()
    {
        if (soundObj == null) return;
        if (!soundObj.activeInHierarchy) return;
        soundSource[1].clip = soundClip[2];
        soundSource[1].loop = false;
        soundSource[1].Play();
    }

    // 실패 사운드
    public static void failSound()
    {
        if (soundObj == null) return;
        if (!soundObj.activeInHierarchy) return;
        soundSource[1].clip = soundClip[3];
        soundSource[1].loop = false;
        soundSource[1].Play();
    }

    // 케이크 없애는 사운드
    public static void cakeDestroySound()
    {
        if (soundObj == null) return;
        if (!soundObj.activeInHierarchy) return;
        soundSource[2].clip = soundClip[4];
        soundSource[2].Play();
    }

    // 케이크 선택해제 사운드
    public static void cakeClickRelease()
    {
        if (soundObj == null) return;
        if (!soundObj.activeInHierarchy) return;
        soundSource[2].clip = soundClip[5];
        soundSource[2].Play();
    }

    // 경고 사운드
    public static void sirenSound()
    {
        if (Instance.m_Fading)
        if (!soundObj.activeInHierarchy) soundObj.SetActive(true);
        
        soundSource[3].clip = soundClip[6];
        soundSource[3].loop = true;
        soundSource[3].Play();

        KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Pause;

    }

    public static bool sirenSoundisPlay()
    {
        return soundSource[3].isPlaying;
    }

    // 경고 사운드 스탑
    public static void sirenStopSound()
    {
        if (!soundObj.activeInHierarchy) return;

        soundSource[3].Stop();

        if(SceneManager.GetActiveScene().name.Equals("Intro") || SceneManager.GetActiveScene().name.Equals("Map"))
        {
            KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Intro;
        }
        else
        {
            if (KinectNotDestroy.soundName.Equals("Result"))
            {
                KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Result;
            }
            else
            {
                KinectNotDestroy.state = KinectNotDestroy.BackGroundMusic.Stage;
            }
        }

        if (PlayerPrefs.GetString("effect").Equals("OFF"))
        {
            soundObj.SetActive(false);
        }
    }
}
