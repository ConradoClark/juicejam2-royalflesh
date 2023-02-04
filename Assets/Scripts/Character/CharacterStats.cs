using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class CharacterStats : BaseGameObject
{
    public LichtPhysicsObject PhysicsObject;

    public bool CanBeHit;
    public float Speed;
    public bool BeingHit { get; private set; }

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

    public event Action OnConnectingHit;

    public void CountHit()
    {
        OnConnectingHit?.Invoke();
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
        if (!CanBeHit) return;
        BeingHit = true;
        DefaultMachinery.AddBasicMachine(HitCooldown());
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

    private IEnumerable<IEnumerable<Action>> HitCooldown()
    {
        yield return TimeYields.WaitMilliseconds(GameTimer, 100);
        BeingHit = false;
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
