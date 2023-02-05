using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
public class CharacterSwapManager : BaseGameObject
{
    private PlayerIdentifier _player;
    private WaveZoneManager _waveZoneManager;
    private WannaChangeYourBones _wannaChangeYourBones;
    protected override void OnAwake()
    {
        base.OnAwake();
        _player = PlayerIdentifier.Instance(true);
        _waveZoneManager = WaveZoneManager.Instance(true);
        _wannaChangeYourBones = WannaChangeYourBones.Instance(true);
    }

    private void OnEnable()
    {
        _waveZoneManager.OnZoneStateChanged += WaveZoneManager_OnZoneStateChanged;
    }
    private void OnDisable()
    {
        _waveZoneManager.OnZoneStateChanged -= WaveZoneManager_OnZoneStateChanged;
    }

    private void WaveZoneManager_OnZoneStateChanged(WaveZoneManager.ZoneStateEvent obj)
    {
        if (obj.ToState == WaveZoneManager.ZoneState.GoMode)
        {
            DefaultMachinery.AddBasicMachine(HandleZoneChanged());
        }
    }

    private IEnumerable<IEnumerable<Action>> HandleZoneChanged()
    {
        yield return TimeYields.WaitSeconds(GameTimer, 2);

        if (_player.Inventory.Inventory.Any(item => item.New))
        {
            _wannaChangeYourBones.Activate();
        }
    }
}
