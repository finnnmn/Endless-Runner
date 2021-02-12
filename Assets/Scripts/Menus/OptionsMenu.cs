using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Menus
{
    [AddComponentMenu("Endless Runner/Menu Components/Options Menu Control")]
    public class OptionsMenu : MonoBehaviour
    {
        private BaseMenuControl menu;
        void Start()
        {
            menu = FindObjectOfType<BaseMenuControl>();
        }

        void Update()
        {

        }

        //sound settings here
    }
}