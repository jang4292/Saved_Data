using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private static AudioSource bgmSource;
    public AudioClip menuBgm;
    public AudioClip baseballBgm;
    public AudioClip escapeBgm;

    private static AudioSource escapeSource;
    public AudioClip escapeBurning;

    private static AudioSource clickSource;
    public AudioClip clickSound;

    private AudioSource shootSource;
    public AudioClip baseBallShootSound;
    public AudioClip sighSound;

    private AudioSource gameSource;
    public AudioClip catchSound;
    public AudioClip missSound;
    public AudioClip smallBreakSound;
    public AudioClip middleBreakSound;
    public AudioClip bigBreakSound;

    private static AudioSource successSource;
    public AudioClip baseBallSuccess;
    public AudioClip escapeSuccess;

    private static AudioSource failSource;
    public AudioClip failSound;

    private static bool addCheck = false;

    void Awake()
    {
        if (addCheck)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            addCheck = true;
        }

        Instance = this;
    }

    public void GetBgmSound()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Menu":
                bgmSource.clip = menuBgm;
                break;
            case "BaseBall":
                bgmSource.clip = baseballBgm;
                break;
            case "Escape":
                bgmSource.clip = escapeBgm;
                break;
        }

        bgmSource.loop = true;
        bgmSource.volume = 0.5f;
        bgmSource.Play();
    }

    public void GetEscapeBurning()
    {
        if(escapeSource == null)
        {
            escapeSource = gameObject.AddComponent<AudioSource>();
            escapeSource.clip = escapeBurning;
            escapeSource.loop = true;
            escapeSource.volume = 0.5f;
        }

        escapeSource.Play();
    }

    public void StopBgmSound()
    {
        bgmSource.Stop();
    }

    public void GetClickSound()
    {
        if (clickSource == null)
        {
            clickSource = gameObject.AddComponent<AudioSource>();
            clickSource.clip = clickSound;
        }

        clickSource.Play();
    }

    public void GetShootOrSighSound()
    {
        if (shootSource == null)
        {
            shootSource = gameObject.AddComponent<AudioSource>();
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "BaseBall":
                shootSource.clip = baseBallShootSound;
                break;
            case "Escape":
                shootSource.clip = sighSound;
                break;
        }

        shootSource.Play();
    }

    private void GetGameSource()
    {
        if (gameSource == null)
        {
            gameSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void GetCatchSound()
    {
        GetGameSource();

        gameSource.clip = catchSound;
        gameSource.Play();
    }

    public void GetMissSound()
    {
        GetGameSource();

        gameSource.clip = missSound;
        gameSource.Play();
    }

    public void GetSmallBreakSound()
    {
        GetGameSource();

        gameSource.clip = smallBreakSound;
        gameSource.Play();
    }

    public void GetMiddleBreakSound()
    {
        GetGameSource();

        gameSource.clip = middleBreakSound;
        gameSource.Play();
    }

    public void GetBigBreakSound()
    {
        GetGameSource();

        gameSource.clip = bigBreakSound;
        gameSource.Play();
    }

    public void GetSuccessSound()
    {
        if (successSource == null)
        {
            successSource = gameObject.AddComponent<AudioSource>();
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "BaseBall":
                successSource.clip = baseBallSuccess;
                break;
            case "Escape":
                successSource.clip = escapeSuccess;
                break;
        }

        StopBgmSound();

        successSource.Play();
    }

    public void GetFailSound()
    {
        if (failSource == null)
        {
            failSource = gameObject.AddComponent<AudioSource>();
            failSource.clip = failSound;
        }

        StopBgmSound();

        failSource.Play();
    }
}
