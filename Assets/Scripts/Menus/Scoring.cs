using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.Menus
{
    public class Scoring : MonoBehaviour
    {
        [SerializeField] private Text score, stats;

        void Start()
        {

        }

        void Update()
        {

        }

        public void ScoreDisplay(float _distance, int _buckets)
        {
            int far = Mathf.RoundToInt(_distance);
            stats.text = string.Format("Buckets Collected: {0}\n\nDistance Travelled: {1}m", _buckets.ToString(), far.ToString());

            int number = _buckets * 5 + far;
            score.text = string.Format("You Rusted with a Score of: {0}", number);
        }
    }
}