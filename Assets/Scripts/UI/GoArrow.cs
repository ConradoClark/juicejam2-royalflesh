using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class GoArrow : BaseGameObject
{
    private bool _showing;
    public float VisibleFrequencyInMs;
    public float InvisibleFrequencyInMs;

    public SpriteRenderer GoSignSprite;
    private WaveZoneManager _waveZoneManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _waveZoneManager = WaveZoneManager.Instance();
    }

    private void OnEnable()
    {
        GoSignSprite.enabled = false;
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
            _showing = true;
            DefaultMachinery.AddBasicMachine(Blink());
        }
        else
        {
            _showing = false;
        }
    }

    private IEnumerable<IEnumerable<Action>> Blink()
    {
        if (VisibleFrequencyInMs == 0 || InvisibleFrequencyInMs == 0) yield break;
        GoSignSprite.enabled = true;
        while (_showing)
        {
            yield return TimeYields.WaitMilliseconds(GameTimer, VisibleFrequencyInMs);
            GoSignSprite.enabled = false;

            yield return TimeYields.WaitMilliseconds(GameTimer, InvisibleFrequencyInMs);
            GoSignSprite.enabled = true;
        }

        GoSignSprite.enabled = false;
    }
}
