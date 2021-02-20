using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace EndlessRunner.Menus
{
    public class PlaySFX : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource source;
        [SerializeField, Tooltip("0-bucket\n1-good item\n2-bad item\n3-death\n4-click")] private AudioClip[] clips;

        public static PlaySFX instance = null;

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

        void Start()
        {
            #region check sounds are attached
            int num = clips.Length;
            if (num <= 0)
            {
                Debug.LogError("No sound effects attached.");
            }
            else if (num < 5)
            {
                Debug.LogError("Not enough sound effects attached.");
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    if (!clips[0])
                    {
                        Debug.LogError("Sound is empty at index "+i.ToString());
                    }
                }
            }
            #endregion
        }

        void Update()
        {

        }

        public void CollectBucketSound()
        {
            source.clip = clips[0];
            source.Play();
        }
        public void GoodItemSound()
        {
            source.clip = clips[1];
            source.Play();
        }
        public void BadItemSound()
        {
            source.clip = clips[2];
            source.Play();
        }
        public void DeathSound()
        {
            source.clip = clips[3];
            source.Play();
        }
        public void ClickSound()
        {
            source.clip = clips[4];
            source.Play();
        }
    }
}