using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using UnityEngine;

public class PlayerIdentifier : SceneObject<PlayerIdentifier>
{
    public Transform ShadowRef;
    public CharacterAnimController AnimController;
    public LimbInventory Inventory { get; private set; }
    private void Awake()
    {
        Inventory = LimbInventory.Instance(true);
    }
}
