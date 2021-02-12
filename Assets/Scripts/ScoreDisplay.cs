using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Text distanceText;
    public Text collectibleText;


    public void UpdateDistanceText(float _value)
    {
        distanceText.text = "Distance: " + Mathf.FloorToInt(_value);
    }

    public void UpdateCollectibleText(float _value)
    {
        collectibleText.text = "Collectibles: " + _value;
    }


}
