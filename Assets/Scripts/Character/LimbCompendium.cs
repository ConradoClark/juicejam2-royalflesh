using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Unity.Objects;
using UnityEngine;

namespace Assets.Scripts.Character
{

    public class LimbCompendium : MonoBehaviour
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

        public event Action OnLimbsChanged;

        private void Awake()
        {
            if (Dictionary != null) return;
            Dictionary = Associations.ToDictionary(kvp => kvp.Type, kvp => (kvp.Limb, kvp.IconSprite));
        }

        public void Refresh()
        {
            Awake();
        }

        public void ChangeLimbs(IReadOnlyList<LimbInventory.LimbItem> items)
        {
            if (items.Count == 0) return;
            var limbRef = Dictionary[items.First().Limb].LimbRef;
            var allSiblings = limbRef.transform.parent.transform.parent
                .GetComponentsInChildren<LimbRef>();
            foreach (var limb in allSiblings)
            {
                limb.gameObject.SetActive(false);
            }

            foreach (var item in items.Where(i=>i.Equipped))
            {
                Dictionary[item.Limb].LimbRef.gameObject.SetActive(true);
            }
            OnLimbsChanged?.Invoke();
        }
    }
}
