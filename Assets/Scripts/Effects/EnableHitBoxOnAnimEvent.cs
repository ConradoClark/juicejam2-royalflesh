using System;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class EnableHitBoxOnAnimEvent : BaseGameObject
{
    public CustomAnimationEventListener AnimEventListener;
    public CharacterAnimController CharacterAnimController;
    public LichtPhysicsObject PhysicsObject;
    public Collider2D Collider;
    public LayerMask LayerToCheck;
    private Collider2D[] _results;
    public string EventName;

    public event Action<Collider2D[]> OnCollide;
    private Vector3 _originalLocalPosition;

    protected override void OnAwake()
    {
        base.OnAwake();
        _originalLocalPosition = Collider.transform.localPosition;
        Collider.enabled = false;
    }
    void OnEnable()
    {
        _results = new Collider2D[10];
        this.ObserveEvent<CustomAnimationEventListener.AnimationEventType,
            CustomAnimationEventListener.CustomAnimationEventHandler>(CustomAnimationEventListener.AnimationEventType.OnCustomAnimationEvent,
                OnEvent);

        this.ObserveEvent<CustomAnimationEventListener.AnimationEventType,
            CustomAnimationEventListener.CustomAnimationEventHandler>(CustomAnimationEventListener.AnimationEventType.OnCustomAnimationEventExit,
            OnEventExit);
    }

    private void OnEventExit(CustomAnimationEventListener.CustomAnimationEventHandler obj)
    {
        if (obj.Source != AnimEventListener) return;
        if (obj.AnimEvent.EventName != EventName) return;
        Collider.enabled = false;
    }

    private void OnEvent(CustomAnimationEventListener.CustomAnimationEventHandler obj)
    {
        if (obj.Source != AnimEventListener) return;
        if (obj.AnimEvent.EventName != EventName) return;

        var latestDirection = CharacterAnimController != null
            ? CharacterAnimController.LatestDirection
            : PhysicsObject != null ? PhysicsObject.LatestDirection
                : Vector2.one;

        Collider.enabled = true;
        Collider.transform.localPosition = new Vector3(_originalLocalPosition.x * Mathf.Sign(latestDirection.x),
            _originalLocalPosition.y,
            _originalLocalPosition.z);

        DefaultMachinery.AddBasicMachine(HandleCollider());
    }

    private IEnumerable<IEnumerable<Action>> HandleCollider()
    {
        while (Collider.enabled)
        {
            var collision = Collider.OverlapCollider(new ContactFilter2D
            {
                layerMask = LayerToCheck,
                useLayerMask = true
            }, _results);

            if (collision > 0)
            {
                OnCollide?.Invoke(_results);
                yield return TimeYields.WaitMilliseconds(GameTimer, 25);
                Collider.enabled = false;
            }
            else yield return TimeYields.WaitOneFrameX;
        }
    }

    void OnDisable()
    {
        this.StopObservingEvent<CustomAnimationEventListener.AnimationEventType,
            CustomAnimationEventListener.CustomAnimationEventHandler>(CustomAnimationEventListener.AnimationEventType.OnCustomAnimationEvent,
            OnEvent);
    }
}
