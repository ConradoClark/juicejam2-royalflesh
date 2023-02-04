using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class Float : BaseGameObject
{
    public float Amount;
    public float FrequencyInMs;
    private float _initialY;
    private bool _enabled;

    protected override void OnAwake()
    {
        base.OnAwake();
        
    }

    private void OnEnable()
    {
        _initialY = transform.position.y;
        _enabled = true;
        DefaultMachinery.AddBasicMachine(PerformFloat());
    }

    private void OnDisable()
    {
        _enabled = false;
        transform.position = new Vector3(transform.position.x, _initialY, transform.position.z);
    }

    private IEnumerable<IEnumerable<Action>> PerformFloat()
    {
        var frequency = Math.Max(1, FrequencyInMs);
        while (_enabled)
        {
            yield return transform.GetAccessor()
                .Position
                .Y
                .FromUpdatedValues()
                .Increase(Amount)
                .Over(frequency * 0.5f * 0.001f)
                .BreakIf(() => !_enabled)
                .UsingTimer(GameTimer)
                .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
                .Build();

            yield return transform.GetAccessor()
                .Position
                .Y
                .FromUpdatedValues()
                .Decrease(Amount)
                .Over(frequency * 0.5f * 0.001f)
                .BreakIf(() => !_enabled)
                .UsingTimer(GameTimer)
                .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
                .Build();
        }
    }

}
