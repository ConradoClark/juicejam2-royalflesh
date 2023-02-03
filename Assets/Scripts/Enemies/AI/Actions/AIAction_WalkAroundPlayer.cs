using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Random = UnityEngine.Random;

public class AIAction_WalkAroundPlayer : BaseAIAction
{
    public float TimeInSeconds;
    public float SpeedMultiplier = 1;

    public override IEnumerable<IEnumerable<Action>> Execute(BaseEnemyAI source, Func<bool> breakCondition)
    {
        source.Animator.SetWalking(true);
        var direction = Random.insideUnitCircle;
        yield return TimeYields.WaitSeconds(GameTimer, TimeInSeconds, _ =>
        {
            source.Reference.ApplySpeed(direction * source.Stats.Speed * SpeedMultiplier);
        });
        source.Animator.SetWalking(false);
    }

    public override void OnInterrupt(BaseEnemyAI source)
    {
        source.Animator.SetWalking(false);
    }
}
