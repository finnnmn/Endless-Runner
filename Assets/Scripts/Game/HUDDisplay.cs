using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Text distanceText;
    public Text collectibleText;
    public Image mopChargeImage;
    [Header("Game Over Screen UI References")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text gameOverstats;
    
    public void UpdateDistanceText(float _value)
    {
        distanceText.text = "Distance: " + Mathf.FloorToInt(_value);
    }

    public void UpdateCollectibleText(float _value)
    {
        collectibleText.text =  _value.ToString();
    }

    public void SetMopChargeImage(float _fillAmount)
    {
        mopChargeImage.fillAmount = _fillAmount;
    }

    public void PlayerDeath(float _distance, float _collectibles)
    {
        gameOverScreen.SetActive(true);
        gameOverstats.text = "Buckets collected: " + _collectibles + "\n\nDistance achieved: " + Mathf.FloorToInt(_distance) + "\n\nTop Speed: N/A";
    }
   


}
