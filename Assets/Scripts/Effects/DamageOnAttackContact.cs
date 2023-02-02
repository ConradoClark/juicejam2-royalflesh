using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;

public class DamageOnAttackContact : BaseGameObject
{
    public CharacterStats Stats;
    public int Damage;
    public EnableHitBoxOnAnimEvent HitBoxEvent;

    private LichtPhysics _physics;
    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
    }

    private void OnEnable()
    {
        HitBoxEvent.OnCollide += HitBoxEvent_OnCollide;
    }

    private void OnDisable()
    {
        HitBoxEvent.OnCollide -= HitBoxEvent_OnCollide;
    }
    private void HitBoxEvent_OnCollide(UnityEngine.Collider2D[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj == null) continue;
            if (!_physics.TryGetPhysicsObjectByCollider(obj, out var target)) continue;
            if (!target.TryGetCustomObject(out CharacterStats stats)) continue;
            stats.Damage(Damage);
        }
    }
}

