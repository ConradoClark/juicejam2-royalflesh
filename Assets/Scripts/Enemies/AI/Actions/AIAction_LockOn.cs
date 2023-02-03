using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using UnityEngine;

public class AIAction_LockOn : BaseAIAction
{
    public float TimeInSeconds;
    public override IEnumerable<IEnumerable<Action>> Execute(BaseEnemyAI source, Func<bool> breakCondition)
    {
        source.Animator.SetLockOn(true);
        yield return TimeYields.WaitSeconds(GameTimer, TimeInSeconds, breakCondition: breakCondition);
    }

    public override void OnInterrupt(BaseEnemyAI source)
    {
    }
}
