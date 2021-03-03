using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.Gameplay {
    public class DebuffUI : MonoBehaviour
    {
        [SerializeField] Text debuffName;
        [SerializeField] Image debuffIcon;
        [SerializeField] Text debuffTimer;

        public void SetDebuffNameText(string _text) => debuffName.text = _text;
        public void SetDebuffIcon(Sprite _sprite) => debuffIcon.sprite = _sprite;
        public void SetDebuffTimerText(float _value) => debuffTimer.text = (Mathf.FloorToInt(_value) + 1) + "s";
    }
}