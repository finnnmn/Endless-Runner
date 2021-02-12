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

        #endregion
    }
}