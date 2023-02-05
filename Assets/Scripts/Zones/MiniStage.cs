using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Pooling;
using UnityEngine;

public class MiniStage : EffectPoolable
{
    public WaveZone Zone;
    private PlayerIdentifier _player;
    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
    }

    public override void OnActivation()
    {
        DefaultMachinery.AddBasicMachine(HandleDestruction());
    }

    private IEnumerable<IEnumerable<Action>> HandleDestruction()
    {
        while (Vector2.Distance(_player.transform.position, transform.position) < 16f)
        {
            yield return TimeYields.WaitOneFrameX;
        }
        EndEffect();
        Zone.Cleared = false;
    }
}
