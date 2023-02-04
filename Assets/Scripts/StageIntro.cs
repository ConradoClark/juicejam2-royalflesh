using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class StageIntro : BaseGameObject
{
    public GameObject LevelIntroEffect;
    public SpriteRenderer Curtains;
    public GameObject Portrait;
    public Vector3 PortraitInitialPosition;
    public CharacterIntro CharacterIntro;
    private void OnEnable()
    {
        DefaultMachinery.AddBasicMachine(RunIntro());
    }

    private IEnumerable<IEnumerable<Action>> RunIntro()
    {
        var curtains = Curtains.GetAccessor()
            .Color
            .A.SetTarget(0)
            .Over(2)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .UsingTimer(GameTimer)
            .Build();

        Portrait.transform.localPosition = PortraitInitialPosition;
        var portrait = Portrait.transform.GetAccessor()
            .LocalPosition.X
            .SetTarget(0)
            .Over(3f)
            .Easing(EasingYields.EasingFunction.BounceEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        LevelIntroEffect.SetActive(true);

        yield return TimeYields.WaitSeconds(GameTimer, 4)
            .Combine(curtains)
            .Combine(portrait)
            .Combine(CharacterIntro.RunIntro().AsCoroutine());

        LevelIntroEffect.SetActive(false);
    }
}
