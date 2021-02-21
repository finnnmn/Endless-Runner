using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Menus
{
    [AddComponentMenu("Endless Runner/Menu Components/Menu Control for Start Scene")]
    public class StartMenus : BaseMenuControl
    {
        void Start()
        {
            SwitchToPanel(0); //press any key panel
            PlaySFX.instance.SwitchTrack(0); //play menu song
        }

        void Update()
        {
            #region press any key
            if (panelsInScene[0].activeSelf)
            {
                if (Input.anyKey)
                {
                    SwitchToPanel(1);
                }
            }
            #endregion
        }

        #region canvas methods
        public void PlayGame()
        {
            PlaySFX.instance.ClickSound();
            LoadScene(1); //game scene has index 1
        }
        public void OpenSettings()
        {
            PlaySFX.instance.ClickSound();
            SwitchToPanel(2); //switch to options panel
        }
        public void OpenCredits()
        {
            PlaySFX.instance.ClickSound();
            SwitchToPanel(3); //switch to credits panel
        }
        public void BackButton()
        {
            PlaySFX.instance.ClickSound();
            SwitchToPanel(1); //switch to start menu
        }
        #endregion
    }
}