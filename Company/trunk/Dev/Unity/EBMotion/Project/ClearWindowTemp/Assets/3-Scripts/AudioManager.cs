using UnityEngine;

namespace ClearWindow
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource bgmSound;
        private AudioSource cleanSound;
        private AudioSource endSound;

        public AudioClip bgm;
        public AudioClip clean;
        public AudioClip end;

        void Start()
        {
            StartBgmSound();
        }

        private void SetSoundLoopAndClip(AudioSource audio, bool loopCheck, AudioClip clip)
        {
            audio.loop = loopCheck;
            audio.clip = clip;
            audio.Play();
        }

        public void StartBgmSound()
        {
            if (bgmSound == null) bgmSound = gameObject.AddComponent<AudioSource>();

            SetSoundLoopAndClip(bgmSound, true, bgm);
        }

        public void StartCleanSound()
        {
            if (cleanSound == null) cleanSound = gameObject.AddComponent<AudioSource>();

            if (cleanSound.isPlaying) return;

            SetSoundLoopAndClip(cleanSound, false, clean);
        }

        public void StartEndSound()
        {
            if (endSound == null) endSound = gameObject.AddComponent<AudioSource>();

            SetSoundLoopAndClip(endSound, false, end);
        }
    }
}
