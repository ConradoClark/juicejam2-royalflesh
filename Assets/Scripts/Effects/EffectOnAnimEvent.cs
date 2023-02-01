using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Events;
using Licht.Unity.Effects;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;

public class EffectOnAnimEvent : MonoBehaviour
{
    public Transform SpawnReference;
    public Vector3 SpawnOffset;
    public Vector3 SpawnOffsetOnFlip;
    public bool UseSpawnOffsetOnFlipX;
    public bool MultiplyXOffsetBySpeed;
    public bool MultiplyYOffsetBySpeed;
    public CustomAnimationEventListener AnimEventListener;
    public ScriptPrefab EffectPrefab;
    public string EventName;
    public LichtPhysicsObject PhysicsObject;

    private EffectsManager _effects;
    private SpriteRenderer _refSprite;

    void Awake()
    {
        _effects = EffectsManager.Instance();
        _refSprite = SpawnReference.GetComponentInChildren<SpriteRenderer>(true);
    }

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
        if (!_effects.Effects[EffectPrefab].TryGetFromPool(out var effect)) return;

        var spawnOffset = SpawnOffset;
        if (UseSpawnOffsetOnFlipX && _refSprite.flipX)
        {
            spawnOffset = SpawnOffsetOnFlip;
        }

        if (PhysicsObject!=null)
        {
            spawnOffset = new Vector3(
                spawnOffset.x * (MultiplyXOffsetBySpeed ? PhysicsObject.CalculatedSpeed.normalized.x : 1),
                spawnOffset.y * (MultiplyYOffsetBySpeed ? PhysicsObject.CalculatedSpeed.normalized.y : 1),
                spawnOffset.z);
        }

        effect.Component.transform.position = SpawnReference.transform.position + spawnOffset;
    }

    void OnDisable()
    {
        this.StopObservingEvent<CustomAnimationEventListener.AnimationEventType,
            CustomAnimationEventListener.CustomAnimationEventHandler>(CustomAnimationEventListener.AnimationEventType.OnCustomAnimationEvent,
            OnEvent);
    }
}
