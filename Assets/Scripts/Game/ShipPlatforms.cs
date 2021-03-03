using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EndlessRunner.Gameplay {
    
    [CreateAssetMenu(fileName = "NewShipPlatforms", menuName = "EndlessRunner/Ship Platforms")]
    public class ShipPlatforms : ScriptableObject
    {
        public GameObject[] platformsToAdd;
        public GameObject[] platformsToRemove;
    }
}
