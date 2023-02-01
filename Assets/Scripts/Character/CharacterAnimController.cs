using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using UnityEngine;

public class CharacterAnimController : BaseGameObject
{
    public Vector2 LatestDirection => CharacterController.LatestDirection;

    public LichtTopDownMoveController CharacterController;
    public Animator Animator;
    private SpriteRenderer[] _sprites;
    private bool _flip;

    protected override void OnAwake()
    {
        base.OnAwake();
        _sprites = GetComponentsInChildren<SpriteRenderer>(true);
    }

    public void OnEnable()
    {
        this.ObserveEvent<LichtTopDownMoveController.LichtTopDownMoveEvents,
            LichtTopDownMoveController.LichtTopDownMoveEventArgs>(
            LichtTopDownMoveController.LichtTopDownMoveEvents.OnStartMoving, 
            OnStartMoving);

        this.ObserveEvent<LichtTopDownMoveController.LichtTopDownMoveEvents,
            LichtTopDownMoveController.LichtTopDownMoveEventArgs>(
            LichtTopDownMoveController.LichtTopDownMoveEvents.OnStopMoving,
            OnStopMoving);
    }

    private void OnStopMoving(LichtTopDownMoveController.LichtTopDownMoveEventArgs obj)
    {
        if (obj.Source != CharacterController) return;
        Animator.SetBool("Walking", false);
    }

    private void OnStartMoving(LichtTopDownMoveController.LichtTopDownMoveEventArgs obj)
    {
        if (obj.Source != CharacterController) return;
        Animator.SetBool("Walking", true);
    }

    public void OnDisable()
    {
        this.StopObservingEvent<LichtTopDownMoveController.LichtTopDownMoveEvents,
            LichtTopDownMoveController.LichtTopDownMoveEventArgs>(
            LichtTopDownMoveController.LichtTopDownMoveEvents.OnStartMoving, OnStartMoving);

        this.StopObservingEvent<LichtTopDownMoveController.LichtTopDownMoveEvents,
            LichtTopDownMoveController.LichtTopDownMoveEventArgs>(
            LichtTopDownMoveController.LichtTopDownMoveEvents.OnStopMoving, OnStopMoving);
    }

    public void Update()
    {
        _flip = CharacterController.LatestDirection.x < 0;
        Animator.SetBool("FlipX", _flip);
        foreach (var sprite in _sprites)
        {
            sprite.flipX = _flip;
        }
    }

    public void TriggerRightAttack()
    {
        Animator.SetTrigger("RightAtk");
    }
}
