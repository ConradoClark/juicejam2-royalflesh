using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Pooling;
using UnityEngine;

public class DeathEffect : EffectPoolable
{
    public SpriteRenderer DeathSprite;
    public SpriteRenderer ShadowSprite;
    public bool Flipped;

    public float XKnockBack;
    public float YKnockBack;
    public float FlashFrequencyInMs;

    public float DeathTimeInSeconds;
    public float DissolveTarget;

    public override void OnActivation()
    {
        DefaultMachinery.AddBasicMachine(ExecuteEffect());
    }

    private IEnumerable<IEnumerable<Action>> Flash()
    {
        DeathSprite.enabled = true;
        while (!IsEffectOver)
        {
            yield return TimeYields.WaitMilliseconds(GameTimer, FlashFrequencyInMs);
            DeathSprite.enabled = !DeathSprite.enabled;
        }
        DeathSprite.enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> Dissolve()
    {
        DeathSprite.material.SetFloat("_Dissolve", 0f);

        var characterDissolve = DeathSprite.GetAccessor().Material("_Dissolve")
            .AsFloat()
            .SetTarget(DissolveTarget)
            .Over(DeathTimeInSeconds)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();

        ShadowSprite.material.SetFloat("_Dissolve", 0f);

        var shadowDissolve = ShadowSprite.GetAccessor().Material("_Dissolve")
            .AsFloat()
            .SetTarget(DissolveTarget)
            .Over(DeathTimeInSeconds)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();

        yield return characterDissolve.Combine(shadowDissolve);
    }

    private IEnumerable<IEnumerable<Action>> ArcMovement()
    {
        var arcX = transform.GetAccessor().Position
            .X
            .Increase(XKnockBack * (Flipped ? 1 : -1))
            .Over(DeathTimeInSeconds)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        var arcY = DeathSprite.transform.GetAccessor().Position
            .Y
            .Increase(YKnockBack)
            .Over(DeathTimeInSeconds * 0.3f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        var arcY2 = DeathSprite.transform.GetAccessor().Position
            .Y
            .Decrease(YKnockBack)
            .FromUpdatedValues()
            .Over(DeathTimeInSeconds * 0.7f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .UsingTimer(GameTimer)
            .Build();

        yield return arcX.Combine(arcY.Then(arcY2));
    }

    private IEnumerable<IEnumerable<Action>> ExecuteEffect()
    {
        yield return TimeYields.WaitOneFrameX;

        DeathSprite.flipX = Flipped;
        DefaultMachinery.AddBasicMachine(Flash());

        yield return Dissolve().Combine(ArcMovement());

        EndEffect();
    }
}
