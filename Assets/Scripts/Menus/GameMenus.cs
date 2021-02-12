using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Menus
{
    [AddComponentMenu("Endless Runner/Menu Components/Menu Control for Game Scene")]
    public class GameMenus : BaseMenuControl
    {
        #region Variables
        [Header("Game Scene Specific Objects")]
        [SerializeField] private GameObject headsUpDisplay;
        #endregion

        void Start()
        {
            headsUpDisplay.SetActive(true);
            SwitchToPanel(-1); //disable menu panels
            Pause(false); //run time
        }

        void Update()
        {
            #region show pause menu
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                Pause(true);
                SwitchToPanel(0); //pause panel
            }
            #endregion

        }

        #region pause/unpause
        /// <summary>
        /// time and cursor only
        /// </summary>
        /// <param name="_true">pause?</param>
        protected void Pause(bool _true)
        {
            if (_true)
            {
                Time.timeScale = 0; //stop time
                Cursor.lockState = CursorLockMode.None; //free cursor
                Cursor.visible = true; //reveal cursor
            }
            else
            {
                Time.timeScale = 1; //resume time
                Cursor.lockState = CursorLockMode.Locked; //freeze cursor
                Cursor.visible = false; //hide cursor
            }
        }
        #endregion

        #region canvas methods
        public void ResumeGame()
        {
            SwitchToPanel(-1); //close panels
            Pause(false); //unpause
        }
        public void OpenSettings()
        {
            SwitchToPanel(1); //options panel
            Pause(true); //make sure paused
        }
        public void BackButton()
        {
            SwitchToPanel(0); //pause panel
            Pause(true); //make sure paused
        }
        public void RetryNow()
        {
            LoadScene(1); //gameplay scene has index 1
        }
        public void ReturnToMenu()
        {
            LoadScene(0); //menu scene has index 0
        }
        #endregion
    }
}