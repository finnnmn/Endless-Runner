using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace EndlessRunner.Menus
{
    public class BaseMenuControl : MonoBehaviour
    {
        #region Variables
        [SerializeField, Tooltip("Order the panels like this:" + "\n\nIn Game:\n" + "0-pause panel, 1-options panel, exclude HUD" + "\n\nStart Menu:\n" + "0-press any key, 1-start panel, 2-options panel, 3-credits panel")]
        protected GameObject[] panelsInScene;

        [Header("Loading Objects")]
        [SerializeField] protected GameObject loadingPanel;
        [SerializeField] protected Text loadingText;
        [SerializeField] protected Image loadingBarFill;
        #endregion

        void Start()
        {

        }


        void Update()
        {

        }

        #region switch to panel
        /// <summary>
        /// Disable all panels, enable one panel based on passed index.
        /// </summary>
        /// <param name="_index">index of panel to enable</param>
        protected void SwitchToPanel(int _index)
        {
            if (_index > panelsInScene.Length) //index out of bounds catch
            {
                Debug.LogError("tried to show a non-existant panel");
            }
            else
            {
                for (int i = 0; i < panelsInScene.Length; i++)
                {
                    panelsInScene[i].SetActive(false);
                }
                if (_index >= 0) //negative values only disable
                {
                    panelsInScene[_index].SetActive(true);
                }
            }
        }
        #endregion

        #region quit
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
        #endregion

        #region loading
        //load scene async with loading bar
        /// <summary>
        /// does not use loading screen yet
        /// </summary>
        /// <param name="_index">index of scene being loaded</param>
        protected void LoadScene(int _index)
        {
            SceneManager.LoadScene(_index);
        }
        #endregion

        
    }
}