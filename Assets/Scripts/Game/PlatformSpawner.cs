using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Gameplay
{

    public class PlatformSpawner : MonoBehaviour
    {

        [System.Serializable]
        struct Platforms 
        {
            public GameObject[] platformsToAdd;
            public GameObject[] platformsToRemove;
        }
        /// <summary>
        /// array of every possible platform that can be spawned
        /// </summary>
        [Header("Platform Array (Each new array will be added with each ship)")]
        [SerializeField] Platforms[] platforms;
        [SerializeField] GameObject emptyPlatform;
        [SerializeField] GameObject shipStartPlatform;
        [SerializeField] GameObject shipEndPlatform;
        [SerializeField] GameObject shipGap;
        [SerializeField] GameObject bucketPlatform;
        /// <summary>
        /// array containing all the platforms that can currently be spawned
        /// </summary>
        List<GameObject> platformPool = new List<GameObject>();
        /// <summary>
        /// Size of each platform on the z axis (all platforms should be the same size)
        /// </summary>
        [Header("Platform settings")]
        [SerializeField] public float platformSize;
        /// <summary>
        /// How many platforms should be spawned at the start (this is also how many will be spawned at any one time throughout the game)
        /// </summary>
        [SerializeField] [Min(2)] public int startingPlatforms = 3;
        /// <summary>
        /// list of all currently spawned platforms
        /// </summary>
        List<GameObject> currentPlatforms = new List<GameObject>();

        [Header("Ship settings")]
        [SerializeField] [Min(3)] public const int shipLength = 10;

        /// <summary>
        /// Each new platform will have this incremented by 1
        /// </summary>
        int platformNum;
        /// <summary>
        /// The current ship the player is on
        /// </summary>
        int shipNum;
        /// <summary>
        /// position to spawn the platform, incremented with each one spawned
        /// </summary>
        float platformPos;

        private void Start()
        {
            //platforms in the first pool can be spawned
            AddNextPlatformsToPool();
            SpawnStartingPlatforms();
        }

        /// <summary>
        /// picks a random platform from the pool to spawn
        /// </summary>
        public void SpawnRandomPlatform()
        {
            //get a random platform to spawn
            int random = Random.Range(0, platformPool.Count);
            SpawnPlatform(random);
        }

        /// <summary>
        /// spawns a platform in the correct position
        /// </summary>
        /// <param name="_platformID">which platform in the array to spawn</param>
        void SpawnPlatform(int _platformID)
        {
            SpawnPlatform(platformPool[_platformID]);
        }

        void SpawnPlatform(GameObject _platform)
        {
            platformNum += 1;
            GameObject platformToSpawn = _platform;
            if (platformNum == 8)
            {
                platformToSpawn = bucketPlatform;
            }
            else if (platformNum > 1)
            {
                //Find which platform number this is relative to the ship size
                switch (platformNum % shipLength)
                {
                    case 0:
                        //Gap in between ships
                        platformToSpawn = shipGap;
                        break;
                    case 1:
                        //First platform on the ship
                        platformToSpawn = shipStartPlatform;
                        //Add new possible platforms for the next ship
                        AddNextPlatformsToPool();
                        break;
                    case (shipLength - 1):
                        //Last platform on the ship
                        platformToSpawn = shipEndPlatform;
                        break;
                }
            }

            //instantiate the platform from the pool at the z position platformPos
            GameObject newPlatform = Instantiate(platformToSpawn, new Vector3(0, 0, platformPos), Quaternion.identity);
            //add the spawned platform to the list of current platforms
            currentPlatforms.Add(newPlatform);
            //increment the platformPos by the size of each platform for spawning the next one
            platformPos += platformSize;

            //if there are too many platforms destroy the back ones
            if (currentPlatforms.Count > startingPlatforms + 1)
            {
                //destroy the furthest back platform and remove it from the list
                GameObject platformToBeExecuted = currentPlatforms[0];
                currentPlatforms.Remove(platformToBeExecuted);
                Destroy(platformToBeExecuted);
            }
        }

        /// <summary>
        /// spawn the starting platforms at the beginning of the game
        /// </summary>
        void SpawnStartingPlatforms()
        {
            //spawn an empty platform for the player to start on
            SpawnPlatform(emptyPlatform);
            //for each other starting platform pick a random one to spawn
            for (int i = 1; i < startingPlatforms; i++)
            {
                SpawnRandomPlatform();
            }
        }

        void AddNextPlatformsToPool()
        {
            if (platforms.Length > shipNum)
            {
                foreach (GameObject platform in platforms[shipNum].platformsToAdd)
                {
                    platformPool.Add(platform);
                }
                foreach (GameObject platform in platforms[shipNum].platformsToRemove)
                {
                    if (platformPool.Contains(platform))
                        platformPool.Remove(platform);
                }
            }
            
            
            shipNum += 1;
        }

    }
}
