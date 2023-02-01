using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Unity.Effects;
using Licht.Unity.Objects;
using UnityEngine;

public class EffectOnAttackContact : BaseGameObject
{
    public ScriptPrefab EffectPrefab;
    public EnableHitBoxOnAnimEvent HitBoxEvent;
    [SerializeField] private EffectSpawnType _spawnType;
    private EffectsManager _effects;

    [Serializable]
    private enum EffectSpawnType
    {
        Once,
        OnePerCollider,
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        _effects = EffectsManager.Instance();
    }

    private void OnEnable()
    {
        HitBoxEvent.OnCollide += HitBoxEvent_OnCollide;
    }

    private void HitBoxEvent_OnCollide(UnityEngine.Collider2D[] colliderArray)
    {
        if (colliderArray.Length == 0) return;
        switch (_spawnType)
        {
            case EffectSpawnType.Once:
                SpawnEffect(new[] { colliderArray[0] });
                break;
            case EffectSpawnType.OnePerCollider:
                SpawnEffect(colliderArray);
                break;
            default:
                break;
        }
    }

    private void SpawnEffect(IReadOnlyList<Collider2D> points)
    {
        var validPoints = points.Where(point => point != null).ToArray();
        if (!_effects.Effects[EffectPrefab].TryGetManyFromPool(validPoints.Length, out var spawnedEffects)) return;
        for (var index = 0; index < validPoints.Length; index++)
        {
            var point = validPoints[index];
            var effect = spawnedEffects[index];

            effect.Component.transform.position = new Vector3(point.bounds.center.x, point.bounds.center.y,
                effect.Component.transform.position.z);
        }
    }

    private void OnDisable()
    {
        HitBoxEvent.OnCollide -= HitBoxEvent_OnCollide;
    }
}
