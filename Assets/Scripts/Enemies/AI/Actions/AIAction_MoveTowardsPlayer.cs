using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;

public class AIAction_MoveTowardsPlayer : BaseAIAction
{
    public float TimeInSeconds;
    public float Speed;

    public override IEnumerable<IEnumerable<Action>> Execute(BaseEnemyAI source, Func<bool> breakCondition)
    {
        source.Animator.SetWalking(true);
        yield return TimeYields.WaitSeconds(GameTimer, TimeInSeconds, _ =>
        {
            var direction = (PlayerIdentifier.Instance().transform.position - source.Reference.transform.position)
                .normalized;
            source.Reference.ApplySpeed(direction * Speed);
        });
        source.Animator.SetWalking(false);
    }

    public override void OnInterrupt(BaseEnemyAI source)
    {
        source.Animator.SetWalking(false);
    }
}
