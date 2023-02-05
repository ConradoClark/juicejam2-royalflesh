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
    public bool Flip { get; private set; }

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
        Flip = CharacterController.LatestDirection.x < 0;
        Animator.SetBool("FlipX", Flip);
        foreach (var sprite in _sprites)
        {
            sprite.flipX = Flip;
        }

        Animator.speed = (float) GameTimer.Multiplier;
    }

    public void TriggerRightAttack()
    {
        Animator.SetTrigger("RightAtk");
    }
    
    public void TriggerLeftAttack()
    {
        Animator.SetTrigger("LeftAtk");
    }

    public void TriggerJump()
    {
        Animator.SetTrigger("Jump");
    }

    public void SetGrounded(bool grounded)
    {
        Animator.SetBool("Grounded", grounded);
    }

    public void SetHurting(bool hurting)
    {
        Animator.SetBool("Hurting", hurting);
    }
}
