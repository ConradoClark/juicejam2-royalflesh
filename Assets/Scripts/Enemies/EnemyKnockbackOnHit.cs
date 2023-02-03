using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class EnemyKnockbackOnHit : BaseGameObject
{
    public EnemyAnimController AnimController;
    public Basic2DCollisionDetector HurtBoxDetector;
    public LichtPhysicsObject PhysicsObject;

    private bool _enabled;

    public float Speed;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(Flash());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        while (_enabled)
        {
            var trigger = HurtBoxDetector.Triggers.FirstOrDefault(t => t.TriggeredHit);
            if (trigger.TriggeredHit)
            {
                AnimController.SetKnockBack(true);
                yield return PhysicsObject.GetSpeedAccessor(new Vector2(Speed * trigger.Direction.x * trigger.Collider.sharedMaterial.bounciness, 0))
                    .X
                    .SetTarget(0f)
                    .Over(0.4f)
                    .Easing(EasingYields.EasingFunction.CubicEaseOut)
                    .UsingTimer(GameTimer)
                    .Build();
                AnimController.SetKnockBack(false);
            }

            else yield return TimeYields.WaitOneFrameX;
        }
    }
}
