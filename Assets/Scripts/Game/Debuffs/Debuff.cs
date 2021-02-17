using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Gameplay
{
    public class Debuff : MonoBehaviour
    {
        [SerializeField] ObjectDebuffType type;
        public ObjectDebuffType GetDebuffType() => type;

        MeshRenderer mr;
        DebuffData data;

        private void Start()
        {
            mr = GetComponent<MeshRenderer>();
            data = Game.instance.debuffData;
            Setup();
        }
        void Setup()
        {
            //sets the debuff type to a random one
            if (type == ObjectDebuffType.Random)
            {
                type = (ObjectDebuffType)Random.Range(1, 4);
            }
            
            //changes the colour based on the debuff type (temporary, will be changed with models)
            switch (type)
            {
                case ObjectDebuffType.Slow: mr.material = data.slowMaterial; break;

                case ObjectDebuffType.Speed: mr.material = data.speedMaterial; break;

                case ObjectDebuffType.Blind: mr.material = data.blindMaterial; break;

            }
        }

    }
}