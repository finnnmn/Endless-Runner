using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    //array of every possible platform
    [Header("Platform Array")]
    [SerializeField] GameObject[] platforms;
    //for if certain obstacles should be added later into the game
    GameObject[] platformPool;
    [Header("Parameters")]
    //size in units of each platform on the z axis
    [SerializeField] public float platformSize;
    //How many platforms are spawned ahead of the player (not including the one the player is standing on)
    [SerializeField] [Min(2)] public int startingPlatforms = 3;
    //whether the first object in the platforms list can be spawned after the beginning
    [SerializeField] bool canSpawnEmpty;
    //list of all currently active platforms
    List<GameObject> currentPlatforms = new List<GameObject>();

    float platformPos;

    private void Start()
    {
        platformPool = platforms;
        SpawnStartingPlatforms();
    }

    public void SpawnRandomPlatform()
    {
        int randomStart = (canSpawnEmpty ? 0 : 1);
        int random = Random.Range(randomStart, platformPool.Length);
        SpawnPlatform(random);
    }

    void SpawnPlatform(int platformNumber)
    {
        GameObject newPlatform = Instantiate(platformPool[platformNumber], new Vector3(0, 0, platformPos), Quaternion.identity);
        currentPlatforms.Add(newPlatform);
        platformPos += platformSize;

        if (currentPlatforms.Count > startingPlatforms)
        {
            GameObject platformToBeExecuted = currentPlatforms[0];
            currentPlatforms.Remove(platformToBeExecuted);
            Destroy(platformToBeExecuted);
        }
    }

    void SpawnStartingPlatforms()
    {
        SpawnPlatform(0);
        for (int i = 1; i < startingPlatforms; i++)
        {
            SpawnRandomPlatform();
        }
    }

}
