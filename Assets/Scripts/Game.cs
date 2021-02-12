using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// <summary>
    /// time it takes after the player dies before changing scenes
    /// </summary>
    [SerializeField] float deathTime = 1;

    /// <summary>
    /// reference to the platformManager in the scene
    /// </summary>
    [Header("References")]
    [SerializeField] public PlatformSpawner platformManager;
    /// <summary>
    /// reference to the menuHandler in the scene
    /// </summary>
    [SerializeField] MenuHandler menuHandler;
    /// <summary>
    /// reference to the ui panel that comes up when the player dies
    /// </summary>
    [SerializeField] GameObject deathScreen;

    private void Start()
    {
        //hide the death panel
        deathScreen.SetActive(false);
    }

    /// <summary>
    /// Handles the changing of scenes after the player dies
    /// </summary>
    public void PlayerDeath()
    {
        StartCoroutine(GameOverScreen());
    }

    IEnumerator GameOverScreen()
    {
        //set the death screen to active
        deathScreen.SetActive(true);
        //wait for deathTime
        yield return new WaitForSeconds(deathTime);
        //change scenes
        menuHandler.LoadGameOverScene();
    }
}
