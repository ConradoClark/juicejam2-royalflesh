using System;
using System.Collections.Generic;
using Licht.Unity.Objects;

public class LimbInventory : SceneObject<LimbInventory>
{
    [Serializable]
    public enum LimbType
    {
        Nothing,
        MrBonesTorso,
        MrBonesLeftArm,
        MrBonesRightArm,
        MrBonesFeet,
        ZombieTorso,
        ZombieLeftArm,
        ZombieRightArm,
        ZombieFeet
    }

    [Serializable]
    public enum LimbRarity
    {
        Common,
        Magic,
        Rare,
        Special
    }

    [Serializable]
    public class LimbItem
    {
        public string Name;
        public LimbType Limb;
        public CustomAnimationEventListener.AnimatingLimb LimbType;
        public BonusStats Stats;
        public LimbRarity Rarity;
        public int Value;
        public bool New;
        public bool Equipped;
    }

    public List<LimbItem> Inventory;
}
