using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;

public class AIAction_PerformRightAttack : BaseAIAction
{
    public float CooldownTimeInMs;

    public override IEnumerable<IEnumerable<Action>> Execute(BaseEnemyAI source, Func<bool> breakCondition)
    {
        source.Animator.TriggerRightAttack();
        yield return TimeYields.WaitMilliseconds(GameTimer, CooldownTimeInMs);
    }

    public override void OnInterrupt(BaseEnemyAI source)
    {
    }
}
