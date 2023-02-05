using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class CharacterIntro : BaseGameObject
{
    public LichtTopDownMoveController MoveController;
    public CharacterAnimController AnimController;
    public Transform LimbsRef;

    public Vector3 InitialPosition;
    public Vector3 TargetPosition;
    public float JumpHeight;
    public float IntroDurationInSeconds;
    public float IntroDelayInSeconds;

    public bool UseIntro;

    public IEnumerable<IEnumerable<Action>> RunIntro()
    {
        if (!UseIntro) yield break;

        transform.position = InitialPosition;
        MoveController.BlockMovement(this);
        AnimController.SetGrounded(false);
        AnimController.TriggerJump();

        yield return TimeYields.WaitSeconds(GameTimer, IntroDelayInSeconds);

        var xMovement = transform.GetAccessor()
            .Position.X
            .SetTarget(TargetPosition.x)
            .Over(IntroDurationInSeconds)
            .UsingTimer(GameTimer)
            .Build();

        var yMovementStart = LimbsRef.GetAccessor()
            .Position.Y
            .Increase(JumpHeight)
            .Over(IntroDurationInSeconds * 0.45f)
            .Easing(EasingYields.EasingFunction.CubicEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        var yMovementEnd = LimbsRef.GetAccessor()
            .Position.Y
            .FromUpdatedValues()
            .Decrease(JumpHeight)
            .Over(IntroDurationInSeconds * 0.45f)
            .Easing(EasingYields.EasingFunction.CubicEaseIn)
            .UsingTimer(GameTimer)
            .Build();

        yield return xMovement.Combine(yMovementStart.Then(yMovementEnd));

        AnimController.SetGrounded(true);
        MoveController.UnblockMovement(this);
    }
}
