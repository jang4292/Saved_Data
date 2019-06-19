using UnityEngine;

namespace Soccer
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource bgmSound;
        private AudioSource kickSound;
        private AudioSource succesSound;
        private AudioSource endSound;

        public AudioClip bgm;
        public AudioClip kick;
        public AudioClip success;
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

        public void StartKickSound()
        {
            if (kickSound == null) kickSound = gameObject.AddComponent<AudioSource>();

            SetSoundLoopAndClip(kickSound, false, kick);
        }

        public void StartSuccesSound()
        {
            if (succesSound == null) succesSound = gameObject.AddComponent<AudioSource>();

            SetSoundLoopAndClip(succesSound, false, success);
        }

        public void StartEndSound()
        {
            if (endSound == null) endSound = gameObject.AddComponent<AudioSource>();

            SetSoundLoopAndClip(endSound, false, end);
        }

    }
}
