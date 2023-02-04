using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Effects;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;
using UnityEngine.Rendering;

public class Collectable : EffectPoolable
{
    public ScriptPrefab EffectOnPickup;
    public Collider2D Collider;
    public ContactFilter2D ContactFilter;
    
    private EffectsManager _effectsManager;
    private PrefabPool _prefabPool;
    private LichtPhysics _physics;

    public bool PickedUp;
    public bool Debug;

    public SpriteRenderer CollectableSprite;

    protected override void OnAwake()
    {
        base.OnAwake();
        _effectsManager = EffectsManager.Instance();
        _prefabPool = _effectsManager.GetEffect(EffectOnPickup);
        _physics = this.GetLichtPhysics();
    }

    private void OnEnable()
    {
        if (Debug)
        {
            OnActivation();
        }
    }

    public override void OnActivation()
    {
        DefaultMachinery.AddBasicMachine(HandlePickup());
    }

    protected virtual void OnDropEffect()
    {

    }

    protected virtual void OnStartEffect()
    {

    }
    protected virtual void OnPickup()
    {

    }

    private IEnumerable<IEnumerable<Action>> HandlePickup()
    {
        if (IsEffectOver) yield break;
        PickedUp = false;
        var result = new List<Collider2D>();

        OnStartEffect();

        yield return TimeYields.WaitOneFrameX;

        CollectableSprite.material.SetColor("_Flash", Color.white);
        var appear = CollectableSprite.GetAccessor().Material("_Flash")
            .AsColor()
            .ToColor(new Color(0, 0, 0, 0))
            .Over(1.5f)
            .Easing(EasingYields.EasingFunction.CubicEaseIn)
            .UsingTimer(GameTimer)
            .Build();

        var originalY = CollectableSprite.transform.position.y;

        var jumpUp = CollectableSprite.transform.GetAccessor()
            .Position
            .Y
            .FromUpdatedValues()
            .Increase(0.3f)
            .Over(1f)
            .Easing(EasingYields.EasingFunction.CubicEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        var jumpDown = CollectableSprite.transform.GetAccessor()
            .Position
            .Y
            .FromUpdatedValues()
            .Decrease(0.3f)
            .Over(1f)
            .Easing(EasingYields.EasingFunction.CubicEaseIn)
            .UsingTimer(GameTimer)
            .Build();

        CollectableSprite.transform.position = new Vector3(CollectableSprite.transform.position.x, originalY,
            CollectableSprite.transform.position.z);

        yield return appear.Combine(jumpUp.Then(jumpDown));

        OnDropEffect();

        while (!IsEffectOver)
        {
            if (Collider.OverlapCollider(ContactFilter, result)>0 && 
                result.Any(r=> ShadowComparer.IsZIndexInRange(_physics, Collider, r)))
            {
                PickedUp = true;
                if (_prefabPool.TryGetFromPool(out var effect))
                {
                    effect.Component.transform.position = transform.position;
                }
                OnPickup();
                EndEffect();
            }

            yield return TimeYields.WaitMilliseconds(GameTimer, 50);
        }
    }
}

