using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Physics;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public LichtPhysicsObject PhysicsObject;

    public int MaxHP;
    public int CurrentHP { get; private set; }
    private void OnEnable()
    {
        CurrentHP = MaxHP;
        if (PhysicsObject != null)
        {
            PhysicsObject.AddCustomObject(this);
        }
    }

    private void OnDisable()
    {
        PhysicsObject.RemoveCustomObject<CharacterStats>();
    }

    public struct DamageEventHandler
    {
        public int PreviousHP;
        public int Damage;
        public int CurrentHP;
        public bool Deadly;
    }

    public struct HealEventHandler
    {
        public int PreviousHP;
        public int Amount;
        public int CurrentHP;
    }

    public event Action<DamageEventHandler> OnDamage;
    public event Action<HealEventHandler> OnHeal;

    public void Damage(int damage)
    {
        var previousHp = CurrentHP;
        CurrentHP = Math.Max(0, CurrentHP - damage);
        OnDamage?.Invoke(new DamageEventHandler
        {
            PreviousHP =  previousHp,
            CurrentHP = CurrentHP,
            Damage = damage,
            Deadly = CurrentHP == 0
        });
    }

    public void Heal(int heal)
    {
        var previousHp = CurrentHP;
        CurrentHP = Math.Min(CurrentHP + heal, MaxHP);
        OnHeal?.Invoke(new HealEventHandler
        {
            PreviousHP = previousHp,
            CurrentHP = CurrentHP,
            Amount = heal
        });
    }
}
