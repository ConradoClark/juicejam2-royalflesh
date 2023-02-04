using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class CameraForwardConfiner : BaseGameObject
{
    public CinemachineVirtualCamera VirtualCamera;
    private float _startingX;
    private CinemachineFramingTransposer _transposer;
    private WaveZoneManager _waveZoneManager;
    private bool _enabled;

    protected override void OnAwake()
    {
        base.OnAwake();
        _waveZoneManager = WaveZoneManager.Instance();
        _transposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _startingX = _transposer.TrackedPoint.x - transform.position.x;
    }
    private void OnEnable()
    {
        _enabled = true;
        DefaultMachinery.AddBasicMachine(HandleConfiner());
    }

    private void OnDisable()
    {
        _enabled = false;
    }

    private IEnumerable<IEnumerable<Action>> HandleConfiner()
    {
        while (_enabled)
        {
            if (_waveZoneManager.CurrentZoneState != WaveZoneManager.ZoneState.GoMode)
            {
                yield return TimeYields.WaitOneFrameX;
            }

            yield return TimeYields.WaitMilliseconds(GameTimer, 1500);

            while (_waveZoneManager.CurrentZoneState == WaveZoneManager.ZoneState.GoMode)
            {
                if (transform.position.x < _transposer.TrackedPoint.x - _startingX)
                {
                    transform.position = new Vector3(_transposer.TrackedPoint.x - _startingX, transform.position.y);
                }
                yield return TimeYields.WaitOneFrameX;
            }
        }
    }
}
