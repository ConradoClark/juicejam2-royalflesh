using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class KillableObject : EffectPoolable
{
    public CharacterStats Stats;
    public ScriptPrefab Effect;
    public SpriteRenderer SpriteRef;

    private DeathEffectPoolManager _effectsManager;
    private DeathEffectPool _pool;
    protected override void OnAwake()
    {
        base.OnAwake();

        _effectsManager = DeathEffectPoolManagerRef.Instance(true).PoolManager;
        _pool = _effectsManager.GetEffect(Effect);
    }

    private void OnEnable()
    {
        Stats.OnDamage += Stats_OnDamage;
    }

    private void OnDisable()
    {
        Stats.OnDamage -= Stats_OnDamage;
    }

    private void Stats_OnDamage(CharacterStats.DamageEventHandler obj)
    {
        if (!obj.Deadly) return;

        if (_pool.TryGetFromPool(out var deathEffect))
        {
            deathEffect.Flipped = SpriteRef.flipX;
            deathEffect.transform.position = transform.position;
        }

        EndEffect();
        gameObject.SetActive(false); // temp to test zombies
    }

    public override void OnActivation()
    {
    }
}
