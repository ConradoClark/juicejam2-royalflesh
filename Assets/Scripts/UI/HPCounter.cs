using Licht.Unity.Objects;
using UnityEngine;

public class HPCounter : BaseUIObject
{
    public CharacterStats StatsRef;
    public SpriteRenderer BarSprite;
    public SpriteRenderer ShadowSprite;
    public float FullHealthSize;
    public float EmptyHealthSize;

    public bool AutoLink;

    private void OnEnable()
    {
        if (AutoLink)
        {
            LinkEvents();
        }
    }
    private void OnDisable()
    {
        if (StatsRef == null) return;
        StatsRef.OnDamage -= StatsRef_OnDamage;
        StatsRef.OnHeal -= StatsRef_OnHeal;
    }

    public void LinkEvents()
    {
        if (StatsRef == null) return;
        AdjustBarSize(StatsRef.MaxHP);
        StatsRef.OnDamage += StatsRef_OnDamage;
        StatsRef.OnHeal += StatsRef_OnHeal;
    }

    private void StatsRef_OnHeal(CharacterStats.HealEventHandler obj)
    {
        AdjustBarSize(obj.CurrentHP);
    }

    private void StatsRef_OnDamage(CharacterStats.DamageEventHandler obj)
    {
        AdjustBarSize(obj.CurrentHP);
    }

    private void AdjustBarSize(int hp)
    {
        BarSprite.size = new Vector2(CalculateSize(hp), BarSprite.size.y);
    }

    private float CalculateSize(int hp)
    {
        if (StatsRef.MaxHP == 0) return EmptyHealthSize;
        return EmptyHealthSize + hp / (float)StatsRef.MaxHP * (FullHealthSize - EmptyHealthSize);
    }
}
