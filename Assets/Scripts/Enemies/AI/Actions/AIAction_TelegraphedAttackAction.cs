using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Effects;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class AIAction_TelegraphedAttackAction : BaseAIAction
{
    public Vector3 Offset;
    public ScriptPrefab TelegraphIcon;
    public float TelegraphTimeInSeconds;

    private EffectsManager _effects;
    private PrefabPool _pool;

    protected override void OnAwake()
    {
        base.OnAwake();
        _effects = EffectsManager.Instance();
        _pool = _effects.GetEffect(TelegraphIcon);
    }

    public override IEnumerable<IEnumerable<Action>> Execute(BaseEnemyAI source, Func<bool> breakCondition)
    {
        if (_pool.TryGetFromPool(out var effect))
        {
            effect.Component.transform.SetParent(transform);
            effect.Component.transform.localPosition = Offset;
        }
        yield return TimeYields.WaitMilliseconds(GameTimer, TelegraphTimeInSeconds);
    }

    public override void OnInterrupt(BaseEnemyAI source)
    {
    }
}
