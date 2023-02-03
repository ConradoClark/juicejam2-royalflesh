using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AICondition_WhileCloseToPlayer : BaseAICondition
{
    public float TargetDistance;
    public Transform Ref;
    private PlayerIdentifier _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
    }

    public override bool CheckCondition()
    {
        return Vector2.Distance(Ref.position, _player.ShadowRef.transform.position) < TargetDistance;
    }
}
