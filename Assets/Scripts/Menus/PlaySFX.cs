using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EndlessRunner.Menus
{
    public class PlaySFX : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource sourceSFX, sourceMusic;
        [SerializeField, Tooltip("0-bucket\n1-good item\n2-bad item\n3-death\n4-click\n5-jump\n6-mop")] private AudioClip[] sounds;
        [SerializeField, Tooltip("0-menu\n1-game\n2-death")] private AudioClip[] songs;

        public static PlaySFX instance = null;

        private bool soundReady = true, songsReady = true;
        [SerializeField] private int soundCount = 7, songsCount = 3;

        #region Awake set instance
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }
        #endregion

        void Start()
        {
            soundReady=CheckSounds(sounds, soundCount);
            songsReady= CheckSounds(songs, songsCount);
        }

        #region check clip arrays are filled
        /// <summary>
        /// Checks a given array of clips to see if any are null, then returns true if array is filled out.
        /// </summary>
        /// <param name="_set">array of clips being tested for null</param>
        /// <param name="_count">required length of array</param>
        /// <returns>referenced true if array is filled out</returns>
        private bool CheckSounds(AudioClip[] _set, int _count)
        {
            int num = _set.Length;
            if (num <= 0)
            {
                Debug.LogError("No sound effects attached to " + _set.ToString());
                return false;
            }
            else if (num < _count)
            {
                Debug.LogError("Not enough sound effects attached to " + _set.ToString());
                return  false;
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    if (!_set[i])
                    {
                        Debug.LogError("Sound is empty at index " + i.ToString());
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region music methods
        /// <summary>
        /// Switch music track being played.
        /// </summary>
        /// <param name="_index">0-menu, 1-game, 2-death</param>
        public void SwitchTrack(int _index)
        {
            if (!songsReady)
            {
                return;
            }
            sourceMusic.clip = songs[_index];
            sourceMusic.Play();
        }
        #endregion

        #region sfx methods
        private void PlaySound(int _index)
        {
            if (!soundReady)
            {
                return;
            }
            sourceSFX.PlayOneShot(sounds[_index]);
        }
        public void BucketSound()
        {
            PlaySound(0);
        }
        public void GoodItemSound()
        {
            PlaySound(1);
        }
        public void BadItemSound()
        {
            PlaySound(2);
        }
        public void DeathSound()
        {
            PlaySound(3);
            SwitchTrack(2);
        }
        public void ClickSound()
        {
            PlaySound(4);
        }
        public void JumpSound()
        {
            PlaySound(5);
        }
        public void MopSound()
        {
            PlaySound(6);
        }
        #endregion
    }
}