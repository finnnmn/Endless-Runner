using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    /// <summary>
    /// array of every possible platform that can be spawned
    /// </summary>
    [Header("Platform Array")]
    [SerializeField] GameObject[] platforms;
    /// <summary>
    /// array containing all the platforms that can currently be spawned
    /// </summary>
    GameObject[] platformPool;
    /// <summary>
    /// Size of each platform on the z axis (all platforms should be the same size)
    /// </summary>
    [Header("Parameters")]
    [SerializeField] public float platformSize;
    /// <summary>
    /// How many platforms should be spawned at the start (this is also how many will be spawned at any one time throughout the game)
    /// </summary>
    [SerializeField] [Min(2)] public int startingPlatforms = 3;
    /// <summary>
    /// can empty platforms be randomly picked from the list after the beginning
    /// </summary>
    [SerializeField] bool canSpawnEmpty;
    /// <summary>
    /// list of all currently spawned platforms
    /// </summary>
    List<GameObject> currentPlatforms = new List<GameObject>();

    /// <summary>
    /// position to spawn the platform, incremented with each one spawned
    /// </summary>
    float platformPos;

    private void Start()
    {
        //all platforms can be spawned
        platformPool = platforms;
        SpawnStartingPlatforms();
    }

    /// <summary>
    /// picks a random platform from the pool to spawn
    /// </summary>
    public void SpawnRandomPlatform()
    {
        //only allow 0 to be chosen if canSpawnEmpty is true
        int randomStart = (canSpawnEmpty ? 0 : 1);
        //get a random platform to spawn
        int random = Random.Range(randomStart, platformPool.Length);
        SpawnPlatform(random);
    }

    /// <summary>
    /// spawns a platform in the correct position
    /// </summary>
    /// <param name="platformNumber">which platform in the array to spawn</param>
    void SpawnPlatform(int platformNumber)
    {
        //instantiate the platform from the pool at the z position platformPos
        GameObject newPlatform = Instantiate(platformPool[platformNumber], new Vector3(0, 0, platformPos), Quaternion.identity);
        //add the spawned platform to the list of current platforms
        currentPlatforms.Add(newPlatform);
        //increment the platformPos by the size of each platform for spawning the next one
        platformPos += platformSize;

        //if there are more platforms than the starting amount
        if (currentPlatforms.Count > startingPlatforms)
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
        SpawnPlatform(0);
        //for each other starting platform pick a random one to spawn
        for (int i = 1; i < startingPlatforms; i++)
        {
            SpawnRandomPlatform();
        }
    }

}
