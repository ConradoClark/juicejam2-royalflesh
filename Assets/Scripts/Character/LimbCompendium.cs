using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Unity.Objects;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class LimbCompendium : SceneObject<LimbCompendium>
    {
        [Serializable]
        public struct LimbAssociation
        {
            public LimbRef Limb;
            public Sprite IconSprite;
            public LimbInventory.LimbType Type;
        }

        public LimbAssociation[] Associations;
        public Dictionary<LimbInventory.LimbType, (LimbRef LimbRef, Sprite Icon)> Dictionary;

        private void Awake()
        {
            Dictionary = Associations.ToDictionary(kvp => kvp.Type, kvp => (kvp.Limb, kvp.IconSprite));
        }
    }
}
