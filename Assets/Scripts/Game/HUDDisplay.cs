using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Menus;

namespace EndlessRunner.Gameplay
{
    public class HUDDisplay : MonoBehaviour
    {
        [Header("UI References")]
        public Text distanceText;
        public Text bucketText;
        public Image mopChargeImage;
        public Image rustBarImage;
        [SerializeField] GameObject blindPanel;
        [Header("Game Over Screen UI References")]
        [SerializeField] GameMenus gameMenu;

        private void Start()
        {
            SetBlindPanelVisibility(false);
        }

        public void UpdateDistanceText(float _value)
        {
            distanceText.text = "Distance: " + Mathf.FloorToInt(_value);
        }

        public void UpdateBucketText(float _value)
        {
            bucketText.text = _value.ToString();
        }

        public void SetMopChargeImage(float _fillAmount)
        {
            mopChargeImage.fillAmount = _fillAmount;
        }

        public void SetRustBarImage(float _fillAmount)
        {
            rustBarImage.fillAmount = _fillAmount;
        }

        public void SetBlindPanelVisibility(bool _visible)
        {
            blindPanel.SetActive(_visible);
        }

        public void PlayerDeath()
        {
            gameMenu.DeathDisplay();
          
        }

    }

}
