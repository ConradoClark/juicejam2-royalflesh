using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class CharacterKnockbackOnHit : BaseGameObject
{
    public LichtTopDownMoveController MoveController;
    public CharacterAnimController AnimController;
    public Basic2DCollisionDetector HurtBoxDetector;
    public LichtPhysicsObject PhysicsObject;
    public Transform LimbsReference;
    public CharacterStats Stats;

    public float KnockBackDurationInSeconds;
    public float YKnockBackStrengthMulti;

    private bool _enabled;

    public float Strength;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(KnockBack());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> KnockBack()
    {
        while (_enabled)
        {
            var trigger = HurtBoxDetector.Triggers.FirstOrDefault(t => t.TriggeredHit);
            if (trigger.TriggeredHit && Stats.BeingHit)
            {
                var speed = Strength * trigger.Direction.x * trigger.Collider.sharedMaterial.bounciness;
                MoveController.BlockMovement(this);
                AnimController.SetHurting(true);
                var xKnockBack = PhysicsObject.GetSpeedAccessor(new Vector2(speed,0))
                    .X
                    .SetTarget(0f)
                    .Over(KnockBackDurationInSeconds)
                    .Easing(EasingYields.EasingFunction.CubicEaseOut)
                    .UsingTimer(GameTimer)
                    .Build();

                var yKnockBackStart = LimbsReference.GetAccessor()
                    .Position
                    .Y
                    .Increase(Mathf.Abs(speed * YKnockBackStrengthMulti))
                    .Over(KnockBackDurationInSeconds * 0.3f)
                    .Easing(EasingYields.EasingFunction.CubicEaseOut)
                    .UsingTimer(GameTimer)
                    .Build();

                var yKnockBackEnd = LimbsReference.GetAccessor()
                    .Position
                    .Y
                    .FromUpdatedValues()
                    .Decrease(Mathf.Abs(speed * YKnockBackStrengthMulti))
                    .Over(KnockBackDurationInSeconds * 0.2f)
                    .Easing(EasingYields.EasingFunction.CubicEaseIn)
                    .UsingTimer(GameTimer)
                    .Build();

                yield return xKnockBack.Combine(yKnockBackStart.Then(yKnockBackEnd));

                AnimController.SetHurting(false);
                MoveController.UnblockMovement(this);
            }

            else yield return TimeYields.WaitOneFrameX;
        }
    }
}
