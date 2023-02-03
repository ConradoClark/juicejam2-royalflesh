using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;

public class AIAction_PerformRightAttack : AIAction_TelegraphedAttackAction
{
    public float CooldownTimeInMs;

    public override IEnumerable<IEnumerable<Action>> Execute(BaseEnemyAI source, Func<bool> breakCondition)
    {
        yield return base.Execute(source, breakCondition).AsCoroutine();

        source.Animator.TriggerRightAttack();
        yield return TimeYields.WaitMilliseconds(GameTimer, CooldownTimeInMs);
    }

    public override void OnInterrupt(BaseEnemyAI source)
    {
    }
}
