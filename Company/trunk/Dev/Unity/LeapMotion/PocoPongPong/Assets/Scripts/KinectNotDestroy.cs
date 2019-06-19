using UnityEngine;
using System.Collections;

public class KinectNotDestroy : MonoBehaviour {

    public enum BackGroundMusic
    {
        Idle,
        Intro,
        Stage,
        Result,
        Pause,
        Stop,
        ON,
        OFF
    }

    public static BackGroundMusic state;

    public static string soundName {
        get
        {
            if (PlayerPrefs.GetString("bgm").Equals("OFF")) return "";
            return sound.clip.name;
        }
    }

    private static AudioSource sound;

    [SerializeField]
    private AudioClip introMap_Sound = null;

    [SerializeField]
    private AudioClip stage_Sound = null;

    [SerializeField]
    private AudioClip result_Sound = null;

    private static bool count = false;

    void Awake()
    {
        if (count == false)
        {
            DontDestroyOnLoad(gameObject);
            sound = gameObject.AddComponent<AudioSource>();
            state = BackGroundMusic.Intro;
            count = true;
        }
        else {
            Destroy(gameObject);
        }
    }

    private bool soundCheck = true;

    void Update()
    {
        if (state == BackGroundMusic.ON)
        {
            soundCheck = true;
            state = BackGroundMusic.Intro;
        }
        else if (state == BackGroundMusic.OFF)
        {
            soundCheck = false;
            sound.Stop();
            state = BackGroundMusic.Idle;
        }

        if (soundCheck)
        {
            switch (state)
            {
                case BackGroundMusic.Intro:
                    SoundChage(introMap_Sound);
                    state = BackGroundMusic.Idle;
                    break;
                case BackGroundMusic.Stage:
                    SoundChage(stage_Sound);
                    state = BackGroundMusic.Idle;
                    break;
                case BackGroundMusic.Result:
                    if (SoundManager.sirenSoundisPlay())
                    {
                        sound.clip = result_Sound;
                    }
                    else
                    {
                        SoundChage(result_Sound);
                    }
                    state = BackGroundMusic.Idle;
                    break;
                case BackGroundMusic.Pause:
                    sound.Pause();
                    state = BackGroundMusic.Idle;
                    break;
                case BackGroundMusic.Stop:
                    sound.Stop();
                    state = BackGroundMusic.Idle;
                    break;
            }
        }
    }

    private void SoundChage(AudioClip clip)
    {
        if (sound.ignoreListenerPause) sound.UnPause();
        else
        {
            sound.clip = clip;
            sound.loop = true;
            sound.Play();
        }
    }
}
