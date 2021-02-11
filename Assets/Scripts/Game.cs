using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;

    #region instance
    private void Awake()
    {
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

    [Header("Paramaters")]
    [SerializeField] public float[] lanes = new float[5];
    [SerializeField] float deathTime = 1;

    [Header("References")]
    [SerializeField] public PlatformSpawner platformManager;
    [SerializeField] MenuHandler uiHandler;
    [SerializeField] GameObject deathScreen;

    private void Start()
    {
        deathScreen.SetActive(false);
    }

    public void PlayerDeath()
    {
        StartCoroutine(GameOverScreen());
    }

    IEnumerator GameOverScreen()
    {
        deathScreen.SetActive(true);
        yield return new WaitForSeconds(deathTime);
        uiHandler.LoadGameOverScene();
    }
}
