using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectDebuffType
{
    Random,
    Slow,
    Speed,
    Blind,
}

public enum DebuffType
{
    SpeedChange,
    Blind
}



namespace EndlessRunner.Gameplay
{
    /// <summary>
    /// For information on debuffs e.g. colours
    /// Will probably be redone with models
    /// </summary>
    public class DebuffData : MonoBehaviour
    {
        [Header("Debuff Data")]
        public Material slowMaterial;
        public Material speedMaterial;
        public Material blindMaterial;
    }

    [System.Serializable]
    public class DebuffInfo
    {
        [Header("Name of the debuff")]
        public string name;
        [Header("What the debuff does")]
        public DebuffType type;
        [Header("Sprite icon for the debuff")]
        public Sprite icon;
        [Header("How long the debuff lasts")]
        public float time;
        [Header("Speed change amount (Ignored on other debuffs)")]
        public float value;
        public bool IsActive { get; set; }
        public float Timer { get; set; }
        public DebuffUI DebuffUI { get; set; }

    }

}
