using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Gameplay
{

    public class Game : MonoBehaviour
    {
        /// <summary>
        /// Instance of Game object in the scene
        /// </summary>
        public static Game instance;

        #region instance
        private void Awake()
        {
            //sets this object to the instance if it doesn't already exist so it can be referenced
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        #endregion

        /// <summary>
        /// array of x positions of each lane
        /// </summary>
        [Header("Paramaters")]
        [SerializeField] public float[] lanes = new float[5];

        [Header("References")]
        [SerializeField] public PlatformSpawner platformManager;
        [SerializeField] public HUDDisplay hudDisplay;
        [SerializeField] public DebuffData debuffData;
        [SerializeField] public Transform debuffLocation;

        [Header("Prefabs")]
        [SerializeField] public GameObject debuffPrefab;

    }
}
