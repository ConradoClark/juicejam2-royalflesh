using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class KillableObject : EffectPoolable
{
    public LimbDropGenerator DropRate;
    public CharacterStats Stats;
    public ScriptPrefab Effect;
    public SpriteRenderer SpriteRef;
    public ScriptPrefab LimbDrop;

    private DeathEffectPoolManager _effectsManager;
    private DeathEffectPool _pool;

    private LimbDropPoolManager _limbDropManager;

    public Vector3 DropOffset;

    protected override void OnAwake()
    {
        base.OnAwake();

        _effectsManager = DeathEffectPoolManagerRef.Instance(true).PoolManager;
        _limbDropManager = LimbDropPoolManagerRef.Instance(true).PoolManager;
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
        var drop = DropRate.GenerateLimb();
        if (drop == null || !_limbDropManager.GetEffect(LimbDrop).TryGetFromPool(out var limb)) return;
        
        limb.Item = drop;
        limb.transform.position = transform.position + DropOffset;
    }

    public override void OnActivation()
    {
    }
}
