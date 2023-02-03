using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class DamageOnCollide : BaseGameObject
{
    public Basic2DCollisionDetector HitBoxDetector;
    public int Damage;

    private bool _enabled;
    private LichtPhysics _physics;
    private SpriteRenderer[] _sprites;

    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(HandleDamage());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleDamage()
    {
        while (_enabled)
        {
            var hits = HitBoxDetector.Triggers.Where(t => t.TriggeredHit).ToArray();
            foreach (var hit in hits)
            {
                if (!_physics.TryGetPhysicsObjectByCollider(hit.Collider, out var target)) continue;
                if (!target.TryGetCustomObject<CharacterStats>(out var stats)) continue;
                stats.Damage(Damage);
            }
            
            yield return TimeYields.WaitOneFrameX;
        }
    }
}
