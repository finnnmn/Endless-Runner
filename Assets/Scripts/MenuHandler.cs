using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    /// <summary>
    /// Loads scene 0 (the main menu)
    /// </summary>
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads scene 1 (the game)
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Loads scene 2 (the game over screen)
    /// </summary>
    public void LoadGameOverScene()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
