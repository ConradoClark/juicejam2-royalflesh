using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using UnityEngine;

public class CharacterAnimController : BaseGameObject
{
    public LichtTopDownMoveController CharacterController;
    public Animator Animator;

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
}
