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

public class PayUpZone : WaveZone
{
    public override WaveZoneManager.ZoneState Type => WaveZoneManager.ZoneState.Fight;
    private PayUp _payUp;

    protected override void OnAwake()
    {
        base.OnAwake();
        _payUp = PayUp.Instance(true);
    }

    public override IEnumerable<IEnumerable<Action>> RunWave()
    {
        _payUp.Activate();
        yield return TimeYields.WaitSeconds(GameTimer, 2);
    }
}

