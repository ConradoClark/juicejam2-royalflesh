using System;
using System.Linq;
using Licht.Impl.Generation;
using Licht.Interfaces.Generation;
using UnityEngine;
using Random = UnityEngine.Random;

public class LimbDropGenerator : MonoBehaviour, IGenerator<int, float>
{
    [Serializable]
    public struct DroppableLimb : IWeighted<float>
    {
        public LimbInventory.LimbType Type;
        public CustomAnimationEventListener.AnimatingLimb LimbType;
        public BonusStats BonusStatsMin;
        public BonusStats BonusStatsMax;
        public LimbInventory.LimbRarity MaxRarity;
        public string Name;
        public string[] NamePrefixes;
        public string[] NameSuffixes;
        public float Chance;
        public float CurrentCombo;

        public float Weight => Chance *
                               (Type == LimbInventory.LimbType.Nothing ? 1 : 1 + Math.Max(0, CurrentCombo - 1) * 0.15f);
        public int Value;
    }

    public DroppableLimb[] Droppables;
    private CharacterCombo _comboTracker;

    private static LimbInventory.LimbRarity[] _rarityOrder = new[]
    {
        LimbInventory.LimbRarity.Common,
        LimbInventory.LimbRarity.Magic,
        LimbInventory.LimbRarity.Rare,
        LimbInventory.LimbRarity.Special
    };

    private void Awake()
    {
        _comboTracker = CharacterComboRef.Instance(true).ComboTracker;
    }

    public LimbInventory.LimbItem GenerateLimb()
    {
        if (Droppables.Length == 0) return null;

        var limbsWithWeight = Droppables.Select(drop =>
            new DroppableLimb
            {
                Type = drop.Type,
                LimbType = drop.LimbType,
                BonusStatsMin = drop.BonusStatsMin,
                BonusStatsMax = drop.BonusStatsMax,
                MaxRarity = drop.MaxRarity,
                Name = drop.Name,
                NamePrefixes = drop.NamePrefixes,
                NameSuffixes = drop.NameSuffixes,
                Chance = drop.Chance,
                CurrentCombo = _comboTracker.CurrentCombo,
                Value = drop.Value
            }
        ).ToArray();

        var generator = new WeightedDice<DroppableLimb>(limbsWithWeight, this);

        var limb = generator.Generate();

        if (limb.Type == LimbInventory.LimbType.Nothing) return null;

        var maxRarity = limb.MaxRarity;
        var rarity = _rarityOrder[Random.Range(0, _rarityOrder
            .TakeWhile(r => r != maxRarity).Take(1).ToArray().Length + 1)];

        var item = new LimbInventory.LimbItem
        {
            Limb = limb.Type,
            LimbType = limb.LimbType,
            Name = GenerateName(limb, rarity),
            Rarity = rarity,
            Stats = GenerateBonusStats(limb),
            Value = limb.Value
        };
        item.Value = GetValue(item, limb);
        return item;
    }

    private int GetValue(LimbInventory.LimbItem item, DroppableLimb original)
    {
        var value = item.Value;
        return item.Rarity switch
        {
            LimbInventory.LimbRarity.Common => (int)(value * CalculateBonusStatsMultiplier(item.Stats, original.BonusStatsMin, original.BonusStatsMax)),
            LimbInventory.LimbRarity.Magic => (int)(value * 1.5f * CalculateBonusStatsMultiplier(item.Stats, original.BonusStatsMin, original.BonusStatsMax)),
            LimbInventory.LimbRarity.Rare => (int)(value * 3f * CalculateBonusStatsMultiplier(item.Stats, original.BonusStatsMin, original.BonusStatsMax)),
            LimbInventory.LimbRarity.Special => (int)(value * 5f * CalculateBonusStatsMultiplier(item.Stats, original.BonusStatsMin, original.BonusStatsMax)),
            _ => (int)(value * CalculateBonusStatsMultiplier(item.Stats, original.BonusStatsMin, original.BonusStatsMax))
        };
    }

    private float CalculateBonusStatsMultiplier(BonusStats stats, BonusStats min, BonusStats max)
    {
        var minValue = min.Calcium + min.Attack + min.Dexterity + min.Footwork;
        var maxValue = max.Calcium + max.Attack + max.Dexterity + max.Footwork;
        var statsValue = stats.Calcium + stats.Attack + stats.Dexterity + stats.Footwork;

        return Math.Max(1, statsValue + statsValue * (minValue / maxValue));
    }

    private BonusStats GenerateBonusStats(DroppableLimb limb)
    {
        return new BonusStats
        {
            Calcium = Random.Range((int)limb.BonusStatsMin.Calcium, (int)limb.BonusStatsMax.Calcium + 1),
            Attack = Random.Range((int)limb.BonusStatsMin.Attack, (int)limb.BonusStatsMax.Attack + 1),
            Dexterity = Random.Range((int)limb.BonusStatsMin.Dexterity, (int)limb.BonusStatsMax.Dexterity + 1),
            Footwork = Random.Range((int)limb.BonusStatsMin.Footwork, (int)limb.BonusStatsMax.Footwork + 1),
        };
    }

    private string GenerateName(DroppableLimb limb, LimbInventory.LimbRarity rarity)
    {
        var whichToUse = 0;
        if (rarity == LimbInventory.LimbRarity.Common) return limb.Name;
        if (rarity == LimbInventory.LimbRarity.Rare || rarity == LimbInventory.LimbRarity.Special)
        {
            whichToUse = 2;
        }

        if (whichToUse == 0)
        {
            whichToUse = Random.Range(0, 2);
        }

        var prefix = whichToUse == 1 || limb.NamePrefixes.Length == 0
            ? ""
            : limb.NamePrefixes[Random.Range(0, limb.NamePrefixes.Length)];

        var suffix = whichToUse == 0 || limb.NameSuffixes.Length == 0
            ? ""
            : limb.NameSuffixes[Random.Range(0, limb.NameSuffixes.Length)];

        return $"{prefix} {limb.Name} {suffix}".Trim();
    }

    public int Seed { get; set; }
    public float Generate()
    {
        return Random.value;
    }
}
