using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class ApplySpeedOnAnimEvent : BaseGameObject
{
    public LichtPhysicsObject Target;
    public LatestDirectionRef LastDirectionRef;
    public float Speed;
    public float DurationInMs;

    public CustomAnimationEventListener AnimEventListener;
    public string EventName;

    void OnEnable()
    {
        this.ObserveEvent<CustomAnimationEventListener.AnimationEventType,
            CustomAnimationEventListener.CustomAnimationEventHandler>(CustomAnimationEventListener.AnimationEventType.OnCustomAnimationEvent,
            OnEvent);
    }

    private void OnEvent(CustomAnimationEventListener.CustomAnimationEventHandler obj)
    {
        if (obj.Source != AnimEventListener) return;
        if (obj.AnimEvent.EventName != EventName) return;

        DefaultMachinery.AddBasicMachine(ApplySpeed());
    }

    IEnumerable<IEnumerable<Action>> ApplySpeed()
    {
        yield return TimeYields.WaitMilliseconds(GameTimer, DurationInMs, _ =>
        {
            Target.ApplySpeed(new Vector2(LastDirectionRef.LatestDirection.x * Speed,0));
        });
    }

    void OnDisable()
    {
        this.StopObservingEvent<CustomAnimationEventListener.AnimationEventType,
            CustomAnimationEventListener.CustomAnimationEventHandler>(CustomAnimationEventListener.AnimationEventType.OnCustomAnimationEvent,
            OnEvent);
    }
}
