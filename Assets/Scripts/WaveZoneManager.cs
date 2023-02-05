using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class WaveZoneManager : SceneObject<WaveZoneManager>
{
    public CinemachineVirtualCamera VirtualCamera;
    public StageIntro Intro;
    public List<WaveZone> Zones;

    private ZoneState _currentZoneState;

    public ZoneState CurrentZoneState
    {
        get => _currentZoneState;

        set
        {
            var prevState = _currentZoneState;
            _currentZoneState = value;
            OnZoneStateChanged?.Invoke(new ZoneStateEvent
            {
                FromState = prevState,
                ToState = _currentZoneState
            });
        }
    }

    public event Action<ZoneStateEvent> OnZoneStateChanged;

    public struct ZoneStateEvent
    {
        public ZoneState FromState;
        public ZoneState ToState;
    }

    private bool _enabled;
    private PlayerIdentifier _player;

    [Serializable]
    public enum ZoneState
    {
        GoMode,
        Fight
    }

    private void Awake()
    {
        Zones = new List<WaveZone>();
        _player = PlayerIdentifier.Instance(true);
    }

    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.Instance(true).MachineryRef.Machinery.AddBasicMachine(HandleZones());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    public void AddZone(WaveZone zone)
    {
        Zones.Add(zone);
    }

    private IEnumerable<IEnumerable<Action>> HandleZones()
    {
        while (!Intro.IsOver) yield return TimeYields.WaitOneFrameX;

        CurrentZoneState = ZoneState.GoMode;
        var orderedZones = Zones.OrderBy(zone => zone.transform.position.x).ToArray();
        var currentZoneIndex = 0;
        while (currentZoneIndex < orderedZones.Length)
        {
            var currentZone = orderedZones[currentZoneIndex];
            while (_player.transform.position.x < currentZone.transform.position.x)
            {
                yield return TimeYields.WaitOneFrameX;
            }

            CurrentZoneState = currentZone.Type;
            VirtualCamera.Follow = currentZone.transform;

            yield return currentZone.RunWave().AsCoroutine();

            currentZoneIndex++;

            CurrentZoneState = ZoneState.GoMode;
            VirtualCamera.Follow = _player.transform;
        }
    }
}
