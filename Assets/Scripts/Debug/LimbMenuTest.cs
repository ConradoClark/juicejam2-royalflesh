using System;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LimbMenuTest : BaseGameObject
{
    private WannaChangeYourBones _wannaChangeYourBones;
    private PlayerInput _playerInput;

    protected override void OnAwake()
    {
        base.OnAwake();
        _playerInput = FindObjectOfType<PlayerInput>();
        _wannaChangeYourBones = WannaChangeYourBones.Instance(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.actions["debug_limbmenu"].WasPerformedThisFrame() 
                && !_wannaChangeYourBones.isActiveAndEnabled)
        {
            _wannaChangeYourBones.Activate();
        }
    }
}
