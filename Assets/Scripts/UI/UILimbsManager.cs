using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Licht.Unity.Objects;

public class UILimbsManager : SceneObject<UILimbsManager>
{
    public LimbCompendium UILimbCompendium;

    public void ChangeLimb(LimbInventory.LimbItem item)
    {
        var limbRef = UILimbCompendium.Dictionary[item.Limb].LimbRef;
        var allSiblings = limbRef.transform.parent
            .GetComponentsInChildren<LimbRef>();
        foreach (var limb in allSiblings)
        {
            limb.gameObject.SetActive(false);
        }

        limbRef.gameObject.SetActive(true);
    }
}
