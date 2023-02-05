using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;

public abstract class WaveZone : BaseGameObject
{
    public bool Cleared;
    public abstract WaveZoneManager.ZoneState Type { get; }
    private WaveZoneManager _manager;
    protected override void OnAwake()
    {
        base.OnAwake();
        _manager = WaveZoneManager.Instance(true);
    }

    private void OnEnable()
    {
        _manager.AddZone(this);
    }

    public abstract IEnumerable<IEnumerable<Action>> RunWave();
}
