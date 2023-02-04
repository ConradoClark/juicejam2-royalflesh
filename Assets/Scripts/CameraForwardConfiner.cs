using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class CameraForwardConfiner : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    private float _startingX;
    private CinemachineFramingTransposer _transposer;

    private void Awake()
    {
        _transposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _startingX = _transposer.TrackedPoint.x - transform.position.x;
    }

    private void Update()
    {
        if (transform.position.x < _transposer.TrackedPoint.x - _startingX)
        {
            transform.position = new Vector3(_transposer.TrackedPoint.x - _startingX, transform.position.y);
        }
    }
}
